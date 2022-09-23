using System.IO;
using System.Linq;
using CCFAProtocols.TIC.Instruments;
using Serilog;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class PrimaryBitMapField
    {
        public const byte FieldNumber = 0;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            char[] bitMapChars = reader.ReadChars(16);
            var bitmap = bitMapChars.SelectMany(c => HexUtils.HexToBoolMap[c]).ToArray();
            Log.Debug("{PrimaryBitMap}", bitmap);
            isomessage.PrimaryBitMap = bitmap;
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            var res = new char[16];

            for (var i = 0; i < isomessage.PrimaryBitMap.LongCount(); i += 4)
                res[i / 4] = HexUtils.BoolsToHexMap[isomessage.PrimaryBitMap[i..(i + 4)]];
            writer.Write(res);
        }
    }
}