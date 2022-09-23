using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using NUnit.Framework;
using UAMP;

namespace UAMP.Tests
{
    [TestFixture]
    public class UAMPTests
    {

        [TestCaseSource("FileTestCases")]
        public void Deserialize(string message,string filename)
        {
            UAMPPackage uampValue = new UAMPPackage(message);
            Assert.AreEqual(message,uampValue.Serialize());
        }
        public static  IEnumerable FileTestCases()
        {
            List<string> paths = new() { "UPDATE.acq","UPDATE.iss" };
            foreach (string path in paths)
            {
                using (StreamReader reader=new StreamReader(Path.Combine("TestData",path)))
                {
                    yield return new TestCaseData(reader.ReadToEnd(),path).SetProperty("source", "file")
                        .SetName($@"File:{path}");
                }
                
                
            }
        }
    }
    
}