using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TCPServer.TICFrames
{
    public class Frame5 : TICFrame
    {
        public override async Task<long> DeserializeLength(Stream reader, CancellationToken cancellationToken)
        {
            var bytes = new byte[2];
            await reader.ReadAsync(bytes);
            return BitConverter.ToUInt16(bytes);
        }

        public override async Task SerializeLength(Stream writer, long length, CancellationToken cancellationToken)
        {
            var _length = (ushort) length;
            var bytes = BitConverter.GetBytes(_length);
            await writer.WriteAsync(bytes);
        }
    }
}