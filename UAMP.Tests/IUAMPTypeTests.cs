using NUnit.Framework;

namespace UAMP.Tests
{
    internal abstract class UAMPBaseTypeTests
    {
        [Test]
        public abstract void Parse();

        [Test]
        public abstract void NI();

        [Test]
        public abstract void Json();

        [Test]
        public abstract void BuildObject();
    }
}