/******************************************************************
 * File: TICFrame.cs
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
using System.Net.Sockets;
using System.Text;
using CCFAProtocols.TIC;
using Serilog;
using TIC.TICFrames;

namespace ParserLibrary.TIC.TICFrames
{
    public abstract class TICFrame
    {
        public static Metrics.MetricCount msg_size =
            (Metrics.MetricCount)Metrics.metric.getMetricCount("tic.frame.msg_size", "Message size in MB");

        public abstract int FrameNum { get; }

        protected abstract ActivitySource _activitySource { get; init; }

        public abstract Task<long> DeserializeLength(Stream reader, CancellationToken cancellationToken);
        public abstract Task SerializeLength(Stream writer, long length, CancellationToken cancellationToken);

        private async Task _SerializeLength(Stream writer, long? length, CancellationToken cancellationToken)
        {
            if (length is null)
            {
                Log.Debug("Return null length");
                await writer.WriteAsync(new byte[] { 0, 0 }, cancellationToken: cancellationToken);
            }
            else
            {
                await SerializeLength(writer, length.Value, cancellationToken);
            }
        }

        public async Task<string?> DeserializeToJson(NetworkStream reader,
            CancellationToken cancellationToken = default)
        {
            var bytes = await _DeserializeLength(reader, cancellationToken);
            if (bytes is null)
            {
                return null;
            }

            return TICMessage.DeserializeToJSON(bytes);
        }

        public async Task<TICMessage?> Deserialize(NetworkStream reader,
            CancellationToken cancellationToken = default)
        {
            var bytes = await _DeserializeLength(reader, cancellationToken);
            if (bytes is null)
            {
                return null;
            }

            return TICMessage.Deserialize(bytes);
        }

        private async Task<byte[]?> _DeserializeLength(NetworkStream reader, CancellationToken cancellationToken)
        {
            var length = await DeserializeLength(reader, cancellationToken);
            if (length == 0)
            {
                return null;
            }

            Log.Debug("Recieve {length}", length);
            var bytes = new byte[length];
            var readBytes = 0;
            do
            {
                readBytes += await reader.ReadAsync(bytes, readBytes, bytes.Length - readBytes, cancellationToken);
            } while (readBytes < length);

            Log.Debug("Bytes Read {bytes}", readBytes);

            msg_size.Add(readBytes);

            Log.Debug("Recieve bytes {bytes}", PrintByteArray(bytes));
            return bytes;
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


        public async Task SerializeFromJson(NetworkStream writer, string? ticMessage,
            CancellationToken cancellationToken = default)
        {
            byte[]? bytes = null;
            if (ticMessage is not null)
            {
                bytes = TICMessage.SerializeFromJson(ticMessage);
                Log.Debug("Serialized: {bytes}", PrintByteArray(bytes));
            }

            await _SerializeLength(writer, bytes?.LongLength, cancellationToken);
            await writer.WriteAsync(bytes);
        }

        public async Task Serialize(NetworkStream writer, TICMessage? ticMessage,
            CancellationToken cancellationToken = default)
        {
            byte[]? bytes = null;
            if (ticMessage is not null)
            {
                bytes = ticMessage.Serialize();
                Log.Debug("Serialized: {bytes}", PrintByteArray(bytes));
            }

            await _SerializeLength(writer, bytes?.LongLength, cancellationToken);
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