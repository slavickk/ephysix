using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TCPServer.TICFrames
{
    public class Frame12 : TICFrame
    {
        public override async Task<long> DeserializeLength(Stream reader, CancellationToken cancellationToken)
        {
            var bytes = new byte[4];
            await reader.ReadAsync(bytes, cancellationToken);
            Array.Reverse(bytes);
            return BitConverter.ToUInt32(bytes);
        }

        public override async Task SerializeLength(Stream writer, long length, CancellationToken cancellationToken)
        {
            var bytes = BitConverter.GetBytes((uint) length);
            Array.Reverse(bytes);
            await writer.WriteAsync(bytes, cancellationToken);
        }
    }
}