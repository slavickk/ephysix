using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using CCFAProtocols.TIC;
using Serilog;
using TIC.TICFrames;

namespace ParserLibrary.TIC.TICFrames
{
    public abstract class TICFrame
    {
        public abstract int FrameNum { get; }

        public abstract Task<long> DeserializeLength(Stream reader, CancellationToken cancellationToken);
        public abstract Task SerializeLength(Stream writer, long length, CancellationToken cancellationToken);

        public async Task<string?> DeserializeToJson(NetworkStream reader, CancellationToken cancellationToken = default)
        {
            var length = await DeserializeLength(reader, cancellationToken);
            Log.Debug("Recieve {length}", length);
            var bytes = new byte[length];
            var readBytes = 0;
            do
            {
                readBytes += await reader.ReadAsync(bytes, readBytes, bytes.Length - readBytes, cancellationToken);
            } while (readBytes < length);

            if (readBytes == 0)
            {
                return null;
            }

            return TICMessage.DeserializeToJSON(bytes);
        }

        public async Task SerializeFromJson(NetworkStream writer, string ticMessage,
            CancellationToken cancellationToken = default)
        {
            var bytes = TICMessage.SerializeFromJson(ticMessage);
            await SerializeLength(writer, bytes.LongLength, cancellationToken);
            await writer.WriteAsync(bytes);
        }

        public static TICFrame GetFrame(int framenum)
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