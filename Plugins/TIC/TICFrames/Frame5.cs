namespace ParserLibrary.TIC.TICFrames
{
    public class Frame5 : TICFrame
    {
        public override int FrameNum => 5;

        public override async Task<long> DeserializeLength(Stream reader, CancellationToken cancellationToken)
        {
            var bytes = new byte[2];
            await reader.ReadAsync(bytes);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return BitConverter.ToUInt16(bytes);
        }

        public override async Task SerializeLength(Stream writer, long length, CancellationToken cancellationToken)
        {
            var _length = (ushort)length;
            var bytes = BitConverter.GetBytes(_length);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            await writer.WriteAsync(bytes);
        }
    }
}