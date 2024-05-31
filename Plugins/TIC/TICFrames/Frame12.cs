/******************************************************************
 * File: Frame12.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

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
            Log.Debug(
                "Deserialize! Frame {FrameNum}: Length bytes (not transform) [{0} {1} {2} {3}]. IsLittleEndian: {IsLittleEndian}.",
                FrameNum, bytes[0], bytes[1], bytes[2], bytes[3], BitConverter.IsLittleEndian);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return BitConverter.ToUInt32(bytes);
        }

        public override async Task SerializeLength(Stream writer, long length, CancellationToken cancellationToken)
        {
            using var activity = _activitySource.StartActivity();
            var bytes = BitConverter.GetBytes((uint)length);
            Log.Debug(
                "Serialize! Frame {FrameNum}: Length bytes (not transform) [{0} {1} {2} {3}]. IsLittleEndian: {IsLittleEndian}.",
                FrameNum, bytes[0], bytes[1], bytes[2], bytes[3], BitConverter.IsLittleEndian);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            await writer.WriteAsync(bytes, cancellationToken);
        }
    }
}