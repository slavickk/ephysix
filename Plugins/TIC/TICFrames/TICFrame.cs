using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
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
        protected abstract ActivitySource _activitySource { get; init; }

        public abstract Task<long> DeserializeLength(Stream reader, CancellationToken cancellationToken);
        public abstract Task SerializeLength(Stream writer, long length, CancellationToken cancellationToken);

        public async Task<string?> DeserializeToJson(NetworkStream reader,
            CancellationToken cancellationToken = default)
        {
            var length = await DeserializeLength(reader, cancellationToken);
            Log.Debug("Recieve {length}", length);
            var bytes = new byte[length];
            var readBytes = 0;
            do
            {
                readBytes += await reader.ReadAsync(bytes, readBytes, bytes.Length - readBytes, cancellationToken);
            } while (readBytes < length);

            Log.Debug("Bytes Read {bytes}", readBytes);

            if (readBytes == 0)
            {
                return null;
            }

            Log.Debug("Recieve bytes {bytes}", PrintByteArray(bytes));
            return TICMessage.DeserializeToJSON(bytes);
        }
        public static string PrintByteArray(byte[] bytes)
        {
            var sb = new StringBuilder("new byte[] { ");
            foreach (var b in bytes)
            {
                sb.Append(b + ", ");
            }
            sb.Append("}");
            return sb.ToString();
        }
        public async Task SerializeFromJson(NetworkStream writer, string ticMessage,
            CancellationToken cancellationToken = default)
        {
            var bytes = TICMessage.SerializeFromJson(ticMessage);
            Log.Debug("Serialized: {bytes}", bytes);
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