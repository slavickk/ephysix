using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParserLibrary;

namespace ParserLibrary.Tests
{
    [TestFixture]
    public class TestPostgresTest
    {
        [Test]
        public async Task  Begin()
        {
            await TestPostgres.test();
        }

    }
}
