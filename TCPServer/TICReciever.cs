using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Serilog.Context;
using TCPServer.TICFrames;

namespace TCPServer
{
    public class TICReciever : IDisposable
    {
        private static uint CurrentTasks;
        private readonly IPEndPoint endPoint;
        private readonly TICFrame frame;
        private readonly IDisposable pushProperty;
        private readonly TcpListener tcpListener;

        public TICReciever(IPAddress address, int port, int framenum)
        {
            endPoint = new IPEndPoint(address, port);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            pushProperty = LogContext.PushProperty("reciever", "TIC");
            frame = TICFrame.GetFrame(framenum);
            tcpListener = new TcpListener(endPoint);
        }

        public void Dispose()
        {
            pushProperty.Dispose();
        }

        public async Task StartServing(CancellationToken cancellationToken)
        {
            var taskFactory = new TaskFactory(cancellationToken,
                TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning,
                TaskContinuationOptions.None, TaskScheduler.Current);
            tcpListener.Start();
            Log.Information("Start listening {endpoint}", endPoint);
            cancellationToken.Register(tcpListener.Stop);
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var tcpClient = await tcpListener.AcceptTcpClientAsync();
                    taskFactory.StartNew(() => ClientHandler(tcpClient, cancellationToken), cancellationToken);
                    Interlocked.Increment(ref CurrentTasks);
                }
            }
            catch (SocketException e) when (e.ErrorCode == 125)
            {
                if (CurrentTasks > 0) await Task.Delay(1000);
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
                        var TICMessageJson = await frame.DeserializeToJson(clientStream, cancellationToken);
                        var res_mock = await Mock(TICMessageJson);
                        await frame.SerializeFromJson(clientStream, res_mock);
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
                    Interlocked.Decrement(ref CurrentTasks);
                }
            }
        }


        public Task<string> Mock(string message)
        {
            return Task.FromResult(message);
        }
    }
}