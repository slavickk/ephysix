using System.Collections.Generic;
using System.Text.Json;
using NUnit.Framework;

namespace UAMP.Tests
{
    [TestFixture(TestOf = typeof(UAMPValue))]
    public class UampValueTests
    {
        [Test]
        [TestCaseSource("JsonTypeSource")]
        public void JsonConverterParseValue(string json, UAMPType type)
        {
            var converter = new UAMPValueJsonConverter();
            Assert.AreEqual(type, JsonDocument.Parse(json).RootElement.ParseAsUAMP().Type);
        }

        public static IEnumerable<TestCaseData> JsonTypeSource()
        {
            var uscaler = new UAMPScalar("test_scaler");
            var ustruct = new UAMPStruct("test_field1", "test_field2", "test_field3");
            var uarray = new UAMPArray(new UAMPStruct("struct_11", "struct_12", "struct_13"),
                new UAMPStruct("struct_21", "struct_22", "struct_23"),
                new UAMPStruct("struct_31", "struct_32", "struct_33"));
            var uamp = new UAMPMessage {Value = {{"scaler", uscaler}, {"ustruct", ustruct}}};
            // {
            //     Value = new List<UAMPMessage>(){{"scaler", uscaler}, {"ustruct", ustruct}, {"uarray", uarray}}
            // };
            foreach (var uampValue in new UAMPValue[] {uscaler, ustruct, uarray, uamp})
            {
                var json = JsonSerializer.Serialize(uampValue);
                yield return new TestCaseData(json, uampValue.Type).SetName(uampValue.Type.ToString());
            }
        }
    }
}