using System.Linq;
using System.Text.Json;
using NUnit.Framework;

namespace UAMP.Tests
{
    [TestFixture]
    internal class UAMPStructTests : UAMPBaseTypeTests
    {
        [Test]
        public override void Parse()
        {
            var serialized = $"scaler1{(char) Symbols.FS}scaler2{(char) Symbols.FS}scaler3";
            var s = new UAMPStruct(serialized);
            Assert.AreEqual(UAMPType.Struct, s.Type);
            Assert.IsNotEmpty(s.Value);
            Assert.AreEqual(new UAMPScalar[] {"scaler1", "scaler2", "scaler3"}, s.Value);
        }

        [Test]
        public override void NI()
        {
            var serialized = $"scaler1{(char) Symbols.FS}{(char) Symbols.NI}{(char) Symbols.FS}scaler3";
            var s = new UAMPStruct(serialized);
            Assert.AreEqual(UAMPType.Struct, s.Type);
            Assert.IsNotEmpty(s.Value);
            Assert.IsNull(s[1]);
            // In JSon
            var json = JsonSerializer.Serialize(s);
            var document = JsonDocument.Parse(json).RootElement;
            var uampScalars = document.GetProperty("Value").EnumerateArray()
                .Select(element => (UAMPScalar) element.ParseAsUAMP())
                .ToArray();
            Assert.AreEqual(3, uampScalars.Length, "Length not equal");
            // Assert.AreEqual(typeof(UAMPScalar), uampScalars[1].GetType(), "Different types");
            Assert.IsNull(uampScalars[1]);
            // In serialized
            Assert.AreEqual((char) Symbols.NI, s.Serialize()[8]);
        }

        [Test]
        public override void Json()
        {
            var s = new UAMPStruct("scaler1", null, "scaler3");
            var json = JsonSerializer.Serialize(s);
            var document = JsonDocument.Parse(json).RootElement;
            document.GetProperty("Type");
            document.GetProperty("Value");
            Assert.AreEqual(UAMPType.Struct, (UAMPType) document.GetProperty("Type").GetByte());
            Assert.AreEqual(JsonValueKind.Array, document.GetProperty("Value").ValueKind);
            var uampScalars = document.GetProperty("Value").EnumerateArray()
                .Select(element => (UAMPScalar) element.ParseAsUAMP())
                .ToArray();
            Assert.IsTrue(s.Value.SequenceEqual(uampScalars));
        }

        [Test]
        public override void BuildObject()
        {
            var s = new UAMPStruct("scaler1", "scaler2", "scaler3");
            Assert.AreEqual(UAMPType.Struct, s.Type);
            Assert.IsNotEmpty(s.Value);
            Assert.AreEqual(new UAMPScalar[] {"scaler1", "scaler2", "scaler3"}, s.Value);
            var serialized = $"scaler1{(char) Symbols.FS}scaler2{(char) Symbols.FS}scaler3";
            Assert.AreEqual(serialized, s.Serialize());
        }
    }
}