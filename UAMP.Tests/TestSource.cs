using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace UAMP.Tests
{
    internal static class TestSource
    {
        public static IEnumerable<TestCaseData> FileSource()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var encoding = Encoding.GetEncoding(866);
            foreach (var filename in new[]
            {
                "UAMP1.bin", "UAMP2.bin", "SUBUAMP.bin", "TIC124.bin", "recHis.bin"
            }) // "UAMP3.bin", - not correct uamp

            {
                byte[] bytearray;
                string uampmessage;
                using (var file = File.Open($"TestData/{filename}", FileMode.Open))
                {
                    bytearray = new byte[file.Length];
                    var read = file.Read(bytearray);
                    Assert.AreEqual(bytearray.Length, read);
                    uampmessage = encoding.GetString(bytearray);
                }

                yield return new TestCaseData(uampmessage).SetName(filename[..^4]);
            }
        }
    }
}