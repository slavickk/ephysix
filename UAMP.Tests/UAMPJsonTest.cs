using System.Linq;
using System.Text.Json;
using NUnit.Framework;

namespace UAMP.Tests
{
    [TestFixture]
    public class UAMPJsonTest
    {
        [Test]
        public void SerializeToJson()
        {
            var uamp = new UAMPPackage();
            var message1 = new UAMPMessage();
            UAMPScalar[] s = {"s11", "s12", "s13"};
            message1["array"] = new UAMPArray(new UAMPStruct(s), new UAMPStruct("s21", "s22", "s23"));
            message1["scaler"] = "scaler1";
            message1["struct"] = new UAMPStruct(s);

            var subuamp = new UAMPPackage();
            var subuampmessage = new UAMPMessage();
            subuampmessage["sub_scaler"] = "sub_scaler";
            subuampmessage["sub_array"] = new UAMPArray("as1", "as2", "as3");

            subuamp.Value.Add(subuampmessage);
            message1["subuamp"] = subuamp;
            var message2 = new UAMPMessage();
            message2["array"] = new UAMPArray(new UAMPStruct(s), new UAMPStruct(s), new UAMPStruct(s));

            uamp.Value.Add(message1);
            uamp.Value.Add(message2);
            var serialize = JsonSerializer.Serialize(uamp);
            var desUAMP = JsonSerializer.Deserialize<UAMPPackage>(serialize);
            Assert.AreEqual(uamp.GetType(), desUAMP.GetType());
            Assert.IsFalse(uamp.Value.SequenceEqual(desUAMP.Value));
        }
    }
}