using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TCPServer.TICFrames
{
    public class Frame6 : TICFrame
    {
        public override async Task<long> DeserializeLength(Stream reader, CancellationToken cancellationToken)
        {
            var bytes = new byte[2];
            await reader.ReadAsync(bytes, cancellationToken);
            Array.Reverse(bytes);

            return BitConverter.ToUInt16(bytes);
        }


        public override async Task SerializeLength(Stream writer, long length, CancellationToken cancellationToken)
        {
            var _length = (ushort) length;
            var bytes = BitConverter.GetBytes(_length);
            Array.Reverse(bytes);
            await writer.WriteAsync(bytes, cancellationToken);
        }
    }
}