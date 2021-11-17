using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ParserLibrary.TIC.TICFrames;
using Serilog;
using Serilog.Context;

namespace ParserLibrary
{
    public class TICReciever : Receiver, IDisposable
    {
        private readonly IDisposable pushProperty;
        private IPEndPoint endpoint;
        private TICFrame Frame;

        public TICReciever()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            pushProperty = LogContext.PushProperty("reciever", "TIC");
        }

        public int TicFrame
        {
            get => Frame.FrameNum;
            set { Frame = TICFrame.GetFrame(value); }
        }

        public int Port
        {
            set => endpoint = new IPEndPoint(IPAddress.Any, value);
            get => endpoint.Port;
        }

        public void Dispose()
        {
            pushProperty.Dispose();
        }

        public override async Task sendResponse(string response, object context)
        {
            NetworkStream networkStream = context as NetworkStream;
            await Frame.SerializeFromJson(networkStream, response);
        }

        public override async Task start()
        {
            CancellationToken cancellationToken = CancellationToken.None;

            var taskFactory = new TaskFactory(CancellationToken.None,
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
                    // Interlocked.Increment(ref CurrentTasks);
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
            using (LogContext.PushProperty("client", client.Client.RemoteEndPoint))
            {
                Log.Information("Accepting client");
                using var clientStream = client.GetStream();
                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var TICMessageJson = await Frame.DeserializeToJson(clientStream, cancellationToken);
                        await signal(TICMessageJson, clientStream);
                    }
                }
                catch (IOException e) when (e.InnerException.GetType().Equals(typeof(SocketException)))
                {
                    throw;
                }
                catch (OperationCanceledException e)
                {
                }
                catch (Exception e)
                {
                    Log.Error(e, "Exception in TIC");
                    throw;
                }
                finally
                {
                    Log.Information("Disconnect client");
                    client.Close();
                    // Interlocked.Decrement(ref CurrentTasks);
                }
            }
        }
    }
}