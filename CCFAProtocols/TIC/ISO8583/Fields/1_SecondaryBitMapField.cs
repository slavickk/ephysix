using System.IO;
using System.Linq;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class SecondaryBitMapField
    {
        public const byte FieldNumber = 1;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                var bitmap = reader.ReadChars(16).SelectMany(c => HexUtils.HexToBoolMap[c]).ToArray();
                isomessage.SecondaryBitMap = bitmap;
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.SecondaryBitMap is null) return;

            var res = new char[16];

            for (var i = 0; i < isomessage.SecondaryBitMap.LongCount(); i += 4)
                res[i / 4] = HexUtils.BoolsToHexMap[isomessage.SecondaryBitMap[i..(i + 4)]];

            writer.Write(res);
        }
    }
}