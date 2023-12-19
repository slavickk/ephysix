using ParserLibrary.TIC.TICFrames;

namespace TIC.TICFrames
{
    public class Frame12 : TICFrame
    {
        public override int FrameNum => 12;

        public override async Task<long> DeserializeLength(Stream reader, CancellationToken cancellationToken)
        {
            var bytes = new byte[4];
            await reader.ReadAsync(bytes, cancellationToken);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return BitConverter.ToUInt32(bytes);
        }

        public override async Task SerializeLength(Stream writer, long length, CancellationToken cancellationToken)
        {
            var bytes = BitConverter.GetBytes((uint)length);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            await writer.WriteAsync(bytes, cancellationToken);
        }
    }
}