/******************************************************************
 * File: TICSender.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading.Channels;
using CCFAProtocols.TIC;
using ParserLibrary.TIC.TICFrames;
using Serilog;
using UniElLib;

namespace ParserLibrary
{
    public class TICSender : Sender, IDisposable
    {
        // public static Meter _meter = new("TIC.TICSender");
        public static Metrics.MetricCount? outgoing_messages = null;
        public static Metrics.MetricHistogram? response_time = null;
        private readonly ActivitySource _activitySource = new("TIC.TICSender");
        private TICFrame Frame;

        private Exception SenderBroken = null;


        public int ticFrame = 6;
        public TimeSpan timeOutms = TimeSpan.FromSeconds(60);
        private CancellationTokenSource TokenSource = new();
        private TcpClient? twfaclient;
        public string twfaHost = "10.74.28.30";
        public int twfaPort = 5595;

        public override TypeContent typeContent => TypeContent.json;


        private Channel<TICMessage> Canal { get; set; } = Channel.CreateUnbounded<TICMessage>();
        private ConcurrentDictionary<uint?, TICMessage?> Responses { get; set; } = new();

        public void Dispose()
        {
            _activitySource.Dispose();
            TokenSource.Dispose();
            twfaclient?.Dispose();
        }

        public override void Init(Pipeline owner)
        {
            base.Init(owner);
            Frame = TICFrame.GetFrame(ticFrame);
            outgoing_messages ??=
                (Metrics.MetricCount)Metrics.metric.getMetricCount("tic.sender.outgoing_messages",
                    "Count waited request");
            response_time ??=
                (Metrics.MetricHistogram)Metrics.metric.getMetricHistogram("tic.sender.response_time", "Request time");
            twfaclient = new TcpClient();
            try
            {
                twfaclient.Connect(twfaHost, twfaPort);
                Log.Information("Connect to twfa: {twfa} . Frame: {FrameNum}. LocalEndpoint: {LocalAddr}",
                    twfaHost + ':' + twfaPort,
                    Frame.FrameNum, twfaclient.Client.LocalEndPoint);
                TokenSource.Token.Register(() =>
                {
                    foreach (var resp in Responses)
                    {
                        Responses[resp.Key] = null;
                    }
                });
                SendWorker(TokenSource.Token);
                ReadWorker(TokenSource.Token).ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Log.Error(task.Exception, "Exception in ReadWorker!");
                        TokenSource.Cancel();
                        SenderBroken = task.Exception;
                    }
                });
            }
            catch (SocketException e) when (e.SocketErrorCode == SocketError.ConnectionRefused)
            {
                Log.Error("Cannot connect to {twfa}", twfaHost + ':' + twfaPort);
                throw;
            }
            catch (Exception e)
            {
                Log.Error(e, e.Message);
                throw;
            }
        }

        private async Task SendWorker(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var json = await Canal.Reader.ReadAsync(token);
                // Log.Debug("Request: {request}", res.json);
                await Frame.Serialize(twfaclient.GetStream(), json, token);
                Log.Debug("Sended: {SystemTraceAuditNumber}", json.TraceAuditNumber);
            }
        }

        private async Task ReadWorker(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var resp = await Frame.Deserialize(twfaclient.GetStream(), token);
                    if (resp is null)
                    {
                        twfaclient.Close();
                        try
                        {
                            Log.Error("TWFA reset connection! Try reconnect.");
                            twfaclient = new TcpClient(twfaHost, twfaPort);
                        }
                        catch (Exception exception)
                        {
                            throw exception;
                        }

                        Log.Information("Reconnected: {LocalAddr}", twfaclient.Client.LocalEndPoint);
                        continue;
                    }

                    Responses.TryUpdate(resp.TraceAuditNumber, resp, null);

                    Log.Debug("Readed: {SystemTraceAuditNumber}", resp.TraceAuditNumber);
                }
                catch (IOException e) when (e.InnerException is SocketException t &&
                                            t.SocketErrorCode == SocketError.ConnectionReset)
                {
                    twfaclient.Close();
                    try
                    {
                        Log.Error("TWFA reset connection! Try reconnect.");
                        twfaclient = new TcpClient(twfaHost, twfaPort);
                    }
                    catch (Exception exception)
                    {
                        throw exception;
                    }

                    Log.Information("Reconnected: {LocalAddr}", twfaclient.Client.LocalEndPoint);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public override async Task<string?> send(string JsonBody, ContextItem context)
        {
            if (SenderBroken is not null)
            {
                throw SenderBroken;
            }

            Activity? activity = _activitySource.StartActivity();
            var time = DateTime.Now;
            try
            {
                var ticMessage = TICMessage.FromJSON(JsonBody);
                activity?.AddBaggage("traceAudit", ticMessage.TraceAuditNumber.ToString());

                Responses.AddOrUpdate(ticMessage.TraceAuditNumber, u => null, (u, message) => null);
                await Canal.Writer.WriteAsync(ticMessage);
                outgoing_messages.Increment();

                TICMessage response;
                var _ctsource =
                    CancellationTokenSource.CreateLinkedTokenSource(((TICContext)context.context).CancellationToken);
                _ctsource.CancelAfter(timeOutms);

                while (Responses.TryGetValue(ticMessage.TraceAuditNumber, out response) && response is null &&
                       !_ctsource.IsCancellationRequested)
                {
                }

                if (_ctsource.IsCancellationRequested)
                {
                    Log.Warning("Transaction {SystemTraceAuditNumber} timeout exceeded: {timeout}!",
                        ticMessage.TraceAuditNumber, timeOutms);
                }

                Responses.TryRemove(ticMessage.TraceAuditNumber, out response);
                outgoing_messages.Decrement();

                response_time?.Add(time);

                return response?.ToJSON();
            }
            catch (InvalidDataException e)
            {
                Log.Error(e, e.Message);
            }
            catch (Exception e)
            {
                Log.Error(e, "Unknown exception in TICReciever");
            }

            return null;
        }
    }
}