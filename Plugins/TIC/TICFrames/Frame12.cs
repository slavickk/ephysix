using System.Diagnostics;
using ParserLibrary.TIC.TICFrames;
using Serilog;

namespace TIC.TICFrames
{
    public class Frame12 : TICFrame
    {
        public override int FrameNum => 12;

        protected override ActivitySource _activitySource { get; init; } = new ActivitySource(nameof(Frame6));

        public override async Task<long> DeserializeLength(Stream reader, CancellationToken cancellationToken)
        {
            using var activity = _activitySource.StartActivity();
            var bytes = new byte[4];
            await reader.ReadAsync(bytes, cancellationToken);
            Array.Reverse(bytes);
            Log.Debug("Length bytes [{0} {1}]", bytes[0], bytes[1]);
            return BitConverter.ToUInt32(bytes);
        }

        public override async Task SerializeLength(Stream writer, long length, CancellationToken cancellationToken)
        {
            using var activity = _activitySource.StartActivity();
            var bytes = BitConverter.GetBytes((uint)length);
            Array.Reverse(bytes);
            Log.Debug("Length bytes [{0} {1}]", bytes[0], bytes[1]);
            await writer.WriteAsync(bytes, cancellationToken);
        }
    }
}