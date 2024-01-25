using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ParserLibrary.TIC.TICFrames;
using Serilog;
using Serilog.Context;
using Serilog.Core.Enrichers;
using Exception = System.Exception;

namespace ParserLibrary
{
    public class TICReceiver : Receiver
    {
        static Metrics.MetricCount? current_clients = null;
        private readonly ActivitySource _activitySource = new("TIC.TICReciever");

        private IPEndPoint endpoint;
        private TICFrame Frame;

        public int ticFrame = 5; //; { get; set; }

        public TICReceiver()
        {
            if (port == -1)
                port = 15001;
            cantTryParse = true;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            current_clients =
                (Metrics.MetricCount)Metrics.metric.getMetricCount("tic.reciever.current_clients", "Connected Clients");
        }

        public override ProtocolType protocolType => ProtocolType.tcp;


        protected override async Task sendResponseInternal(string response, object context)
        {
            TICContext ticContext = context as TICContext;

            if (ticContext.Client.Client?.Connected ?? false)
            {
                await Frame.SerializeFromJson(ticContext.Client.GetStream(), response);
            }
            else
            {
                Log.Error("Client:{client} disconnected!", ticContext.Client.Client?.RemoteEndPoint);
            }
        }

        protected override async Task startInternal()
        {
            Frame = TICFrame.GetFrame(ticFrame);
            endpoint = new IPEndPoint(IPAddress.Any, port);
            CancellationToken cancellationToken = CancellationToken.None;

            var taskFactory = new TaskFactory(cancellationToken,
                TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning,
                TaskContinuationOptions.None, TaskScheduler.Current);

            TcpListener tcpListener = new TcpListener(endpoint);
            tcpListener.Start();
            Log.Information("Start listening {endpoint}", endpoint);
            cancellationToken.Register(tcpListener.Stop);
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var tcpClient = await tcpListener.AcceptTcpClientAsync();
                    taskFactory.StartNew(() => ClientHandler(tcpClient, cancellationToken), cancellationToken);
                    current_clients?.Increment();
                }
            }
            catch (SocketException e) when (e.ErrorCode == 125)
            {
                // if (CurrentTasks > 0) await Task.Delay(1000);
            }
            finally
            {
                tcpListener.Stop();
                Log.Information("Stop Listening");
            }
        }

        public async Task ClientHandler(TcpClient client, CancellationToken cancellationToken)
        {
            Thread.CurrentThread.Name = "Processor: " + client.Client.RemoteEndPoint;
            using (LogContext.Push(new PropertyEnricher("client", client.Client.RemoteEndPoint), new PropertyEnricher(
                       "frame",
                       Frame.FrameNum)))
            {
                Log.Information("Accepting client: {client}. Frame: {frame}", client.Client.RemoteEndPoint,
                    Frame.FrameNum);
                var clientCancelToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                using var clientStream = client.GetStream();
                try
                {
                    while (!clientCancelToken.IsCancellationRequested)
                    {
                        using (_activitySource.StartActivity(ActivityKind.Client))
                        {
                            var context = new TICContext()
                            {
                                Client = client,
                                CancellationToken = clientCancelToken.Token
                            };


                            var TICMessage = await Frame.Deserialize(clientStream, clientCancelToken.Token);
                            if (TICMessage is null) break;
                            using var _ =
                                LogContext.PushProperty("SystemTraceAuditNumber", TICMessage.TraceAuditNumber);
                            signal(TICMessage.ToJSON(), context).ContinueWith(task =>
                            {
                                if (task.IsCanceled || task.IsFaulted) clientCancelToken.Cancel();
                            });
                        }
                    }
                }
                catch (InvalidDataException e)
                {
                    Log.Error(e, e.Message);
                }
                catch (IOException e) when (e.InnerException is SocketException)
                {
                    Log.Information("Remote client close connection: {client}", client.Client.RemoteEndPoint);
                }
                catch (OperationCanceledException e)
                {
                }
                catch (Exception e)
                {
                    Log.Error(e, "Unknown exception in TICReciever");
                }
                finally
                {
                    Log.Information("Disconnect client: {client}", client.Client.RemoteEndPoint);
                    current_clients?.Decrement();
                    client.Close();
                }
            }
        }
    }
}