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
            var serialize = messages.Serialize();
            Assert.AreEqual(uampmessage, serialize);
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