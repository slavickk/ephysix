using System;
using System.IO;
using System.Linq;

namespace CCFAProtocols.TIC.Instruments
{
    internal static class BinaryWriterExtensions
    {
        public static void Write(this BinaryWriter writer, int length, object obj)
        {
            string s = obj switch
            {
                string sobj => sobj.PadRight(length),
                ushort or uint or ulong or uint or byte => obj.ToString().PadLeft(length, '0'),
                long lobj => lobj.ToString(lobj < 0 ? $"D{length - 1}" : $"D{length}"),
                short sobj => sobj.ToString(sobj < 0 ? $"D{length - 1}" : $"D{length}"),
                null => "null",
                _ => throw new ArgumentException($"Type is not valid:{obj.GetType()}", nameof(obj))
            };
            writer.Write(s.ToArray());
        }
    }
}