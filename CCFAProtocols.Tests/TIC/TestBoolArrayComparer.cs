using CCFAProtocols.TIC.Instruments;
using NUnit.Framework;

namespace CCFAProtocols.Tests.TIC
{
    [TestFixture]
    public class TestBoolArrayComparer
    {
        private readonly BoolArrayEquilityComparer comparer = new();

        [Test]
        public void TestEquals()
        {
            Assert.IsTrue(comparer.Equals(null, null));
            Assert.IsTrue(comparer.Equals(new[] {true, false}, new[] {true, false}));
            Assert.IsFalse(comparer.Equals(new[] {true, false}, new[] {true, true}));
            Assert.IsFalse(comparer.Equals(new[] {true, false}, null));
            Assert.IsFalse(comparer.Equals(null, new[] {true, true}));
        }

        [Test]
        public void TestGetHashCode()
        {
            Assert.AreEqual(comparer.GetHashCode(new[] {true, false}),
                comparer.GetHashCode(new[] {true, false}));
            Assert.AreNotEqual(comparer.GetHashCode(new[] {true, true}),
                comparer.GetHashCode(new[] {true, false}));
        }
    }
}