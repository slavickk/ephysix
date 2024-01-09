using System;
using NUnit.Framework;

namespace UAMP.Tests
{
    [TestFixture]
    internal class TestUAMP : UAMPBaseTypeTests
    {
        [TestCaseSource(typeof(TestSource), "FileSource", Category = "files")]
        public void ParseFromFiles(string uampmessage)
        {
            var messages = new UAMPMessage(uampmessage);
            Console.WriteLine(messages);
            var serialize = messages.Serialize();
            Assert.AreEqual(uampmessage, serialize);
        }

        [Test]
        public void ParseExample()
        {
            var messages = new UAMPMessage("RCC=643\u0010TPH=3400000\u0010CCC=1\u0010CAT=0");
        }


        public override void Parse()
        {
            throw new NotImplementedException();
        }

        public override void NI()
        {
            throw new NotImplementedException();
        }

        public override void Json()
        {
            throw new NotImplementedException();
        }

        public override void BuildObject()
        {
            throw new NotImplementedException();
        }
    }
}