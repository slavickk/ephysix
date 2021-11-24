using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ParserLibrary.DummyProtocol1.DummyProtocol1Frames;
using Serilog;
using Serilog.Context;
using YamlDotNet.Serialization;

namespace ParserLibrary
{
    public class DummyProtocol1Receiver : Receiver, IDisposable
    {
        private readonly IDisposable pushProperty;
        private IPEndPoint endpoint;
        private DummyProtocol1Frame Frame;

        public DummyProtocol1Receiver()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            pushProperty = LogContext.PushProperty("reciever", "DummyProtocol1");
        }

        public int dummyProtocol1Frame = 5;//; { get; set; }
/*                {
                    get => Frame.FrameNum;
                    set { Frame = DummyProtocol1Frame.GetFrame(value); }
                }*/

        //        [YamlMember(Alias = "Port")]
        public int port=15001;//{ get; set; }
/*              {
                  set => endpoint = new IPEndPoint(IPAddress.Any, value);
                  get => endpoint.Port;
              }*/

        public void Dispose()
        {
            pushProperty.Dispose();
        }

        public override async Task sendResponseInternal(string response, object context)
        {
            NetworkStream networkStream = context as NetworkStream;
            await Frame.SerializeFromJson(networkStream, response);
        }

        public override async Task startInternal()
        {
            Frame = DummyProtocol1Frame.GetFrame(dummyProtocol1Frame);
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
                        string DummyProtocol1MessageJson;
                        try
                        {
                            DummyProtocol1MessageJson = await Frame.DeserializeToJson(clientStream, cancellationToken);
                        }
                        catch (Exception e)
                        {
                            Log.Error(e, "Exception in DummyProtocol1");
                            continue;
                        }

                        await signal(DummyProtocol1MessageJson, clientStream);
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
                    Log.Error(e, "Exception in DummyProtocol1Reciever");
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