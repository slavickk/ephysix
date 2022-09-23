using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using DummySystem1Protocols.DummyProtocol1;
using Serilog;

namespace TCPServer.DummyProtocol1Frames
{
    public abstract class DummyProtocol1Frame
    {
        public abstract Task<long> DeserializeLength(Stream reader, CancellationToken cancellationToken);
        public abstract Task SerializeLength(Stream writer, long length, CancellationToken cancellationToken);

        public async Task<string> DeserializeToJson(NetworkStream reader, CancellationToken cancellationToken = default)
        {
            var length = await DeserializeLength(reader, cancellationToken);
            Log.Debug("Recieve {length}", length);
            var bytes = new byte[length];
            var readBytes = 0;
            do
            {
                readBytes += await reader.ReadAsync(bytes, readBytes, bytes.Length - readBytes, cancellationToken);
            } while (readBytes < length);

            return DummyProtocol1Message.DeserializeToJSON(bytes);
        }

        public async Task SerializeFromJson(NetworkStream writer, string dummyProtocol1Message,
            CancellationToken cancellationToken = default)
        {
            var bytes = DummyProtocol1Message.SerializeFromJson(dummyProtocol1Message);
            await SerializeLength(writer, bytes.LongLength, cancellationToken);
            await writer.WriteAsync(bytes);
        }

        public static DummyProtocol1Frame GetFrame(int framenum)
        {
            switch (framenum)
            {
                case 5: return new Frame5();
                case 6: return new Frame6();
                case 12: return new Frame12();
                default: throw new ArgumentException("Unknown frametype", "framenum");
            }
        }
    }
}