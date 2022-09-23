using System;
using System.IO;
using System.Linq;
using System.Text;

namespace CCFAProtocols.TIC.Instruments
{
    public static class PLVAR
    {
        private static readonly Encoding encoding = Encoding.GetEncoding(866);

        private static byte[] Deserialize(BinaryReader reader, byte ByteLength)
        {
            var lengthBytes = reader.ReadBytes(ByteLength);

            if (lengthBytes[0] != 0)
            {
                var length = int.Parse(encoding.GetChars(lengthBytes));
                return reader.ReadBytes(length);
            }

            lengthBytes = lengthBytes.Skip(1).ToArray();
            if (BitConverter.IsLittleEndian) Array.Reverse(lengthBytes);

            var Length = lengthBytes.Length switch
            {
                2 => BitConverter.ToUInt16(lengthBytes),
                4 => BitConverter.ToUInt32(lengthBytes),
                8 => BitConverter.ToUInt64(lengthBytes)
            };
            byte[] bytes = new byte[Length];
            for (ulong i = 0; i < Length; i++) bytes[i] = reader.ReadByte();

            return bytes;
        }

        private static void Serialize(BinaryWriter writer, byte Length, byte[] chars)
        {
            var charsLength = chars.Length;
            if (charsLength >= Math.Pow(10, Length))
            {
                var lengthBytes = BitConverter.GetBytes(charsLength);
                if (BitConverter.IsLittleEndian) Array.Reverse(lengthBytes);

                writer.Write((byte) 0);
                writer.Write(lengthBytes);
            }
            else
            {
                writer.Write(charsLength.ToString().PadLeft(Length, '0').ToCharArray());
            }

            writer.Write(chars);
        }

        public static string PLVARReadString(this BinaryReader reader, byte length)
        {
            byte[] value = Deserialize(reader, length);
            return encoding.GetString(value);
        }

        public static void PLVARWriteString(this BinaryWriter writer, byte length, string? obj)
        {
            Serialize(writer, length, encoding.GetBytes(obj));
        }

        public static uint PLVARUINT(this BinaryReader reader, byte length)
        {
            byte[] value = Deserialize(reader, length);
            return uint.Parse(encoding.GetString(value));
        }
    }
}