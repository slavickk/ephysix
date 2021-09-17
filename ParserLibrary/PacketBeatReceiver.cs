using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using zlib;
using System.Collections.Concurrent;
using System.Threading;

namespace ParserLibrary
{
    public class PacketBeatReceiver : Receiver
    {
        public int port = 15001;
        public async override Task start()
        {
            Start(port);
        }
        public PacketBeatReceiver()
        {
//            this.saver = new ReplaySaver() { path = @"C:\D\Out" };
        }

        int Start(int port)
        {
            try
            {
                StartListener(port).Wait();
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return -1;
            }
        }

        ConcurrentBag<LumberJackHandler> handlers = new ConcurrentBag<LumberJackHandler>();
        LumberJackHandler getHandler()
        {
            LumberJackHandler retValue;
            if (handlers.TryTake(out retValue))
            {
                retValue.clear();
                return retValue;
            }
            if(debugMode)
                Console.WriteLine("add new handler");
            retValue = new LumberJackHandler(this);
            retValue.clear();
            return retValue;
        }
        private async Task StartListener(int port)
        {
            //            NewServer serv = new NewServer();
            var tcpListener = TcpListener.Create(port);
            tcpListener.Start();
            for (; ; )
            {
                if (debugMode)
                    Console.WriteLine("[Server] waiting for clients on port "+port+"...");
//                using (var tcpClient = await tcpListener.AcceptTcpClientAsync())
                {
                    var tcpClient = await tcpListener.AcceptTcpClientAsync();
                    Console.WriteLine("accept");
                    Action<object> action =async  (client1) => {
                        LumberJackHandler serv = null;
                        TcpClient client = client1 as TcpClient;
                        try
                        {

                            serv = getHandler();
                            await serv.Handle(client);
                        }
                        catch (Exception e77)
                        {
                            Console.WriteLine("[Server] client connection lost" + e77.ToString());
                        }
                        finally
                        {
                            if (serv != null)
                            {
                                handlers.Add(serv);
                                serv = null;
                            }
                            client?.Dispose();

                        }

                        //                        client.Close();
                    };
                    
                    await new TaskFactory().StartNew(action,tcpClient, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

/*                    LumberJackHandler serv = null;
                    try
                    {
                        serv = getHandler();
                        var t=Task.Run(async () => {
                            try
                            {
                                await serv.Handle(tcpClient);
                            }
                            catch (Exception e77)
                            {
                                Console.WriteLine("[Server] client connection lost" + e77.ToString());
                            }
                            finally
                            {
                                if (serv != null)
                                {
                                    handlers.Add(serv);
                                    serv = null;
                                }

                            }

                        });
                    }*/

                }
            }
        }

    }


    public class LumberJackHandler
    {
        public PacketBeatReceiver owner;
        public LumberJackHandler(PacketBeatReceiver owner1)
        {
            owner = owner1;
        }
        public class LumberFrame
        {
            public int version;
            public int frameType;

            public uint sequenceNumber;
            public int pairCount;
            public uint keyLength;
            public uint valueLength;
            public string strJson;
        }

        async Task sendAck(NetworkStream sr, uint sequenceNumber)
        {
            byte[] ackBytes = new byte[6];
            byte[] bts = BitConverter.GetBytes(sequenceNumber);
            ackBytes[0] = 0x32;
            ackBytes[1] = 0x41;
            Array.Copy(bts, 0, ackBytes, 2, 4);
            Array.Reverse(ackBytes, 2, 4);
            /*            if(final)
                            ackBytes[0] |= 0b10000000;*/
            sr.Write(ackBytes, 0, ackBytes.Length);
            //    Console.WriteLine(DateTime.Now + " +ack end");
            sr.Flush();
            if(owner.debugMode)
                Console.WriteLine(DTWM() + "*ack sent*" + sequenceNumber);
            //            File.WriteAllBytes(@"c:\d\answer.bn", ackBytes);


            //    Console.WriteLine(DateTime.Now + " -ack end");
            //            Console.Flush();
        }
        System.Text.Encoding encoder = new System.Text.ASCIIEncoding();
        uint windowSize = 0;
        //        byte[] ackBytes = new byte[6];
        uint lastFrameToAck = 0;
        uint frameReceivedCount = 0;
        public static void CopyStream(System.IO.Stream input, System.IO.Stream output)
        {
            byte[] buffer = new byte[2000];
            int len;
            while ((len = input.Read(buffer, 0, 2000)) > 0)
            {
                output.Write(buffer, 0, len);
            }
            output.Flush();
        }

        public void clear()
        {
            windowSize = 0;
            //        byte[] ackBytes = new byte[6];
            lastFrameToAck = 0;
            frameReceivedCount = 0;
        }
        static int orderFiles = 0;
        static string DTWM()
        {
            var time = DateTime.Now;
            return time.ToString() + "." + time.Millisecond;
        }
        async Task<byte[]> ReadAll(Stream sr, NetworkStream sw,uint compressedSize)
        {
            int allData = 0;
            byte[] inData = new byte[compressedSize];
            do
            {
                int readBytes = await sr.ReadAsync(inData, allData, inData.Length-allData);

                if (readBytes <=0) //error
                {
                    Console.WriteLine("not all data received "+allData+" "+readBytes);
                    await sendAck(sw, lastFrameToAck); // We ack the last frame we received then crash the connection
                    return null;
                }
                allData += readBytes;
            } while (allData < compressedSize);
            return inData;
        }
        public static bool multiThreadMode = true;
        public async Task parseStream(Stream sr, NetworkStream sw,List<Task> tasks)
        {
            {
                LumberFrame frame = new LumberFrame();
                byte[] head = new byte[2];
                await sr.ReadAsync(head, 0, head.Length);
                frame.version = head[0];

                if (frame.version == -1)
                {
                    {
                        throw new NotImplementedException("Connection terminated ");

                    }
                }
                if (frame.version < 0x31)
                {
                    //sr.CanRead
                    //                    continue;// new 
                    throw new NotImplementedException("Frame version not supported " + frame.version);

                }

                frame.frameType = head[1]; // Determine type of frame
                                           //              Console.WriteLine(DateTime.Now + " frame:" + ((char)frame.version) + ":" + frame.frameType.ToString("X2"));
                                           //  allReceivedBytes.Add((byte)frame.frameType);
                if (frame.frameType == 0x57)
                {

                    byte[] btsWindowSize = new byte[4];
                    int readBytes = await sr.ReadAsync(btsWindowSize, 0, btsWindowSize.Length);
                    if (readBytes != btsWindowSize.Length) //error
                    {
                        await sendAck(sw, lastFrameToAck); // We ack the last frame we received then crash the connection
                        return;
                    }
                    //  allReceivedBytes.AddRange(btsWindowSize);

                    Array.Reverse(btsWindowSize);
                    windowSize = BitConverter.ToUInt32(btsWindowSize, 0); // the number of frames before ack.
                    frameReceivedCount = 0; // reset the frame-received-counter

                    if (owner.debugMode)
                        Console.WriteLine(DTWM() + "window size: " + windowSize);
                    return;
                    //goto start;
                }
                else if (frame.frameType == 0x43)
                {
                    byte[] btsWindowSize = new byte[4];
                    int readBytes = await sr.ReadAsync(btsWindowSize, 0, btsWindowSize.Length);

                    if (readBytes != btsWindowSize.Length) //error
                    {
                        await sendAck(sw, lastFrameToAck); // We ack the last frame we received then crash the connection
                        return;
                    }
                    //allReceivedBytes.AddRange(btsWindowSize);

                    Array.Reverse(btsWindowSize);
                    uint compressedSize = BitConverter.ToUInt32(btsWindowSize, 0); // the number of frames before ack.
                    if (owner.debugMode)
                        Console.WriteLine("compressed wnd size:" + compressedSize);
                    byte[] inData =await ReadAll(sr, sw, compressedSize);
                    if (inData == null)
                        return;
                    /*
                    byte[] inData = new byte[compressedSize];
                    readBytes = await sr.ReadAsync(inData, 0, inData.Length);

                    if (readBytes != inData.Length) //error
                    {
                        Console.WriteLine("not all data received");
                        await sendAck(sw, lastFrameToAck); // We ack the last frame we received then crash the connection
                        return;
                    }*/
                    //                    allReceivedBytes.AddRange(inData);

                    //                    byte[] outData;
                    using (MemoryStream outMemoryStream = new MemoryStream())
                    using (ZOutputStream outZStream = new ZOutputStream(outMemoryStream))
                    using (Stream inMemoryStream = new MemoryStream(inData))
                    {
                        CopyStream(inMemoryStream, outZStream);
                        outZStream.finish();
                        outMemoryStream.Position = 0;
                        int i = 0;
                        DateTime time1 = DateTime.Now;
//                        List<Task> tasks = new List<Task>();
                        while (outMemoryStream.Position < outMemoryStream.Length)
                        {
                            i++;
                            await parseStream(outMemoryStream, sw,tasks);
                        }
                        var diff = (DateTime.Now - time1).TotalMilliseconds;
                        if (owner.debugMode)
                            Console.WriteLine("unpack " + i + " compressed files;"+diff+" ms ");
                        //                    Console.WriteLine("return from compressed " + isMemStream);
                        //                      return;
                        //                        outData = outMemoryStream.ToArray();
                    }

                    frameReceivedCount = 0;

//                    Console.WriteLine(DTWM() + "!ack sent");

                    //                    Console.WriteLine("return from compressed 2 "+isMemStream);

                    /*                    lastFrameToAck = frame.sequenceNumber;
                                        frameReceivedCount++;

                                        if (frameReceivedCount >= windowSize)
                                        {
                                            sendAck(sr, lastFrameToAck);
                                            frameReceivedCount = 0;

                                            Console.WriteLine("ack sent");
                                        }*/

                    /*                  Console.WriteLine(" decompressed " + outData.Length + " bytes");
                                      File.WriteAllBytes(@"C:\d\a.byt",outData);*/
                }

                else if (frame.frameType == 0x4a)
                {
                    byte[] btsHeader = new byte[8];
                    int retval = await sr.ReadAsync(btsHeader, 0, btsHeader.Length);

                    if (retval != btsHeader.Length) return;
                    //                  allReceivedBytes.AddRange(btsHeader);

                    Array.Reverse(btsHeader, 0, 4);
                    Array.Reverse(btsHeader, 4, 4);


                    frame.sequenceNumber = BitConverter.ToUInt32(btsHeader, 0);

                    uint payloadLength = (uint)BitConverter.ToInt32(btsHeader, 4);
                    byte[] btsAll = new byte[payloadLength];

                    retval = await sr.ReadAsync(btsAll, 0, (int)payloadLength);
                    //                allReceivedBytes.AddRange(btsAll);

                    frame.strJson = encoder.GetString(btsAll);
                    if(multiThreadMode)
                        tasks.Add(Task.Run(async () => {
                            await owner.signal(frame.strJson);
                        }) );
                    else
                        await owner.signal(frame.strJson);
                    //                    Console.WriteLine("json:" + frame.strJson.Length);
                    //                    File.WriteAllText(@"C:\d\out\aa_"+(orderFiles++)+"_" + DateTime.Now.Second + DateTime.Now.Millisecond + ".json", frame.strJson);

                    /*                    if (onDataFrame != null)
                                        {
                                            onDataFrame(frame);
                                        }*/

                    lastFrameToAck = frame.sequenceNumber;
                    frameReceivedCount++;

                    if (frameReceivedCount >= windowSize)
                    {
                        await sendAck(sw, lastFrameToAck);
                        frameReceivedCount = 0;


                    }
                }
                else
                {
                    throw new NotImplementedException("Frame type not supported " + frame.frameType);
                }


            }
            //            Console.WriteLine("End while " + bContinue + " " + isMemStream + " " + sr.Position + " " + sr.Length);
        }

        public  async Task Handle(TcpClient tcpClient)
        {
            DateTime time = DateTime.Now;
            if (owner.debugMode)
                Console.WriteLine(DTWM() + "[Server] Client has connected");
            clear();
            using (var networkStream = tcpClient.GetStream())
            {
                List<Task> tasks = new List<Task>();
                bool first = true;
                while (first || networkStream.DataAvailable)
                {
                    await parseStream(networkStream, networkStream,tasks);
                    first = false;
                }
                if(multiThreadMode)
                {
                    Task t = Task.WhenAll(tasks.ToArray());
                    try
                    {
                        await t;
                    }
                    catch { }

                    if (t.Status == TaskStatus.RanToCompletion)
                        ;// Console.WriteLine("All Requests attempts succeeded.");
                    else if (t.Status == TaskStatus.Faulted)
                        Console.WriteLine("Requests attempts failed");
                }
                if (owner.debugMode)
                    Console.WriteLine(DTWM() + "end send "+(DateTime.Now-time).TotalMilliseconds+" ms");
            }
        }
    }


}
