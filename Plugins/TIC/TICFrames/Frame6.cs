using System.Diagnostics;
using Serilog;

namespace ParserLibrary.TIC.TICFrames
{
    public class Frame6 : TICFrame
    {
        public override int FrameNum => 6;
        protected override ActivitySource _activitySource { get; init; } = new(nameof(Frame6));

        public override async Task<long> DeserializeLength(Stream reader, CancellationToken cancellationToken)
        {
            using var activity = _activitySource.StartActivity();
            var bytes = new byte[2];
            await reader.ReadAsync(bytes, cancellationToken);
            Log.Debug("Frame {FrameNum}: Length bytes (not transform) [{0} {1}]", FrameNum, bytes[0], bytes[1]);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
                Log.Debug("IsLittleEndian: {IsLittleEndian}. Reverse bytes length", BitConverter.IsLittleEndian);
            }

            return BitConverter.ToUInt16(bytes);
        }


        public override async Task SerializeLength(Stream writer, long length, CancellationToken cancellationToken)
        {
            using var activity = _activitySource.StartActivity();
            var _length = (ushort)length;
            var bytes = BitConverter.GetBytes(_length);
            Log.Debug("Frame {FrameNum}: Length bytes (not transform) [{0} {1}]", FrameNum, bytes[0], bytes[1]);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
                Log.Debug("IsLittleEndian: {IsLittleEndian}. Reverse bytes length", BitConverter.IsLittleEndian);
            }

            // Array.Reverse(bytes);
            await writer.WriteAsync(bytes, cancellationToken);
        }
    }
}