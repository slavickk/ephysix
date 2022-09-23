using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC
{
    /// <summary>
    /// TIC HEADER
    /// </summary>
    /// <remarks>"A4M"|ProtocolVersion(N2)|RejectStatus(N3)</remarks>
    public class TICHeader
    {
        // public string Indicator;
        public byte ProtocolVersion;
        public ushort RejectStatus;

        public static TICHeader Deserialize(BinaryReader reader)
        {
            return new TICHeader()
            {
                ProtocolVersion = reader.ReadByte(2),
                RejectStatus = reader.ReadUshort(3)
            };
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(2, ProtocolVersion);
            writer.Write(3, RejectStatus);
        }

        public override string ToString()
        {
            return $"HEADER:\n\t ProtocolVersion {ProtocolVersion}, RejectStatus {RejectStatus}";
        }
    }
}