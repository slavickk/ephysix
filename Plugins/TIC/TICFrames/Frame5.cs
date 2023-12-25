using System.Diagnostics;
using Serilog;

namespace ParserLibrary.TIC.TICFrames
{
    public class Frame5 : TICFrame
    {
        public override int FrameNum => 5;

        protected override ActivitySource _activitySource { get; init; } = new(nameof(Frame6));

        public override async Task<long> DeserializeLength(Stream reader, CancellationToken cancellationToken)
        {
            using var activity = _activitySource.StartActivity();
            var bytes = new byte[2];
            await reader.ReadAsync(bytes);
            Log.Debug("Length bytes [{0} {1}]", bytes[0], bytes[1]);
            return BitConverter.ToUInt16(bytes);
        }

        public override async Task SerializeLength(Stream writer, long length, CancellationToken cancellationToken)
        {
            using var activity = _activitySource.StartActivity();
            var _length = (ushort)length;
            var bytes = BitConverter.GetBytes(_length);
            Log.Debug("Length bytes [{0} {1}]", bytes[0], bytes[1]);
            await writer.WriteAsync(bytes);
        }
    }
}