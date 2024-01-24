using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using UniElLib;

namespace ParserLibrary.Tests
{
    public class TransformerTests
    {
        public TransformerTests()
        {
            Pipeline.AddCustomParsers(Assembly.GetAssembly(typeof(ParserLibrary.DummyProtocol1Receiver)));
        }

        [Test]
        public void TestPreo()
        {
            
//            AbstrParser.getApropriateParser("", @"RCC=643\u0010TPH=3400000\u0010CCC=1\u0010CAT=0", new AbstrParser.UniEl(), new List<AbstrParser.UniEl>(), false);
            AbstrParser.getApropriateParser("", @"eyJ2ZXJzaW9uIjoiMS4wIiwiYWNjZXB0TGFuZ3VhZ2UiOiIiLCJhY2NlcHRIZWFkZXJzIjoidGV4dC9odG1sLGFwcGxpY2F0aW9uL3hodG1sK3htbCxhcHBsaWNhdGlvbi94bWw7cT0wLjksaW1hZ2UvYXZpZixpbWFnZS93ZWJwLGltYWdlL2FwbmcsKi8qO3E9MC44LGFwcGxpY2F0aW9uL3NpZ25lZC1leGNoYW5nZTt2PWIzO3E9MC43IiwidXNlckFnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgSGVhZGxlc3NDaHJvbWUvMTIwLjAuNjA5OS4yMDAgU2FmYXJpLzUzNy4zNiIsImJyb3dzZXJJcCI6IjEwLjguMTkuMjQxIiwidGltZVpvbmUiOiItMzAwIiwiY29sb3JEZXB0aCI6IjI0Iiwic2NyZWVuSGVpZ2h0IjoiMjQiLCJzY3JlZW5XaWR0aCI6IjgwMCIsImphdmFzY3JpcHRFbmFibGVkIjoidHJ1ZSIsImphdmFFbmFibGVkIjoiZmFsc2UifQ==", new AbstrParser.UniEl(), new List<AbstrParser.UniEl>(), false);

            AbstrParser.getApropriateParser("", @"eyJCcmFuZCI6InhpYW9taSIsIkhhc05mY0hjZSI6ZmFsc2UsIkhhc0ZwU2Nhbm5lciI6dHJ1ZSwiSWQiOiIwNjZhY2MwMDE4ZDA1NWRiIiwiTWFudWZhY3RvcmVyIjoiWGlhb21pIiwiTW9kZWwiOiJSZWRtaSBOb3RlIDciLCJPc1ZlcnNpb24iOjI4LCJQcm9kdWN0IjoiQW5kcm9pZCIsIlJhbU1iIjoyNTYsIlNjcmVlbkRwaSI6IlhYSERQSSIsIlNjcmVlbkhlaWdodFB4IjoyMTMxLCJTY3JlZW5XaWR0aFB4IjoxMDgwLCJTY3JlZW5TaXplSW5jaGVzIjoyLjYzNzgwMDd9", new AbstrParser.UniEl(), new List<AbstrParser.UniEl>(), false);
            AbstrParser.getApropriateParser("", @"eyJCcmFuZCI6InhpYW9taSIsIkhhc05mY0hjZSI6ZmFsc2UsIkhhc0ZwU2Nhbm5lciI6dHJ1ZSwiSWQiOiIwNjZhY2MwMDE4ZDA1NWRiIiwiTWFudWZhY3RvcmVyIjoiWGlhb21pIiwiTW9kZWwiOiJSZWRtaSBOb3RlIDciLCJPc1ZlcnNpb24iOjI4LCJQcm9kdWN0IjoiQW5kcm9pZCIsIlJhbU1iIjoyNTYsIlNjcmVlbkRwaSI6IlhYSERQSSIsIlNjcmVlbkhlaWdodFB4IjoyMTMxLCJTY3JlZW5XaWR0aFB4IjoxMDgwLCJTY3JlZW5TaXplSW5jaGVzIjoyLjYzNzgwMDd9", new AbstrParser.UniEl(), new List<AbstrParser.UniEl>(), false);
            // AbstrParser.getApropriateParser("", @"eyJ2ZXJzaW9uIjoiMS4wIiwiYWNjZXB0TGFuZ3VhZ2UiOiIiLCJhY2NlcHRIZWFkZXJzIjoidGV4dC9odG1sLGFwcGxpY2F0aW9uL3hodG1sK3htbCxhcHBsaWNhdGlvbi94bWw7cT0wLjksaW1hZ2UvYXZpZixpbWFnZS93ZWJwLGltYWdlL2FwbmcsKi8qO3E9MC44LGFwcGxpY2F0aW9uL3NpZ25lZC1leGNoYW5nZTt2PWIzO3E9MC43IiwidXNlckFnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgSGVhZGxlc3NDaHJvbWUvMTIwLjAuNjA5OS4xMzAgU2FmYXJpLzUzNy4zNiIsImJyb3dzZXJJcCI6IjEwLjguMTkuMjQxIiwidGltZVpvbmUiOiItMzAwIiwiY29sb3JEZXB0aCI6IjI0Iiwic2NyZWVuSGVpZ2h0IjoiMjQiLCJzY3JlZW5XaWR0aCI6IjgwMCIsImphdmFzY3JpcHRFbmFibGVkIjoidHJ1ZSIsImphdmFFbmFibGVkIjoiZmFsc2Ui", new AbstrParser.UniEl(), new List<AbstrParser.UniEl>(), false);
            //AbstrParser.getApropriateParser("", @"eyJ2ZXJzaW9uIjoiMS4wIiwiYWNjZXB0TGFuZ3VhZ2UiOiIiLCJhY2NlcHRIZWFkZXJzIjoidGV4dC9odG1sLGFwcGxpY2F0aW9uL3hodG1sK3htbCxhcHBsaWNhdGlvbi94bWw7cT0wLjksaW1hZ2UvYXZpZixpbWFnZS93ZWJwLGltYWdlL2FwbmcsKi8qO3E9MC44LGFwcGxpY2F0aW9uL3NpZ25lZC1leGNoYW5nZTt2PWIzO3E9MC43IiwidXNlckFnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgSGVhZGxlc3NDaHJvbWUvMTIwLjAuNjA5OS4xMzAgU2FmYXJpLzUzNy4zNiIsImJyb3dzZXJJcCI6IjEwLjguMTkuMjQxIiwidGltZVpvbmUiOiItMzAwIiwiY29sb3JEZXB0aCI6IjI0Iiwic2NyZWVuSGVpZ2h0IjoiMjQiLCJzY3JlZW5XaWR0aCI6IjgwMCIsImphdmFzY3JpcHRFbmFibGVkIjoidHJ1ZSIsImphdmFFbmFibGVkIjoiZmFsc2UifQ", new AbstrParser.UniEl(), new List<AbstrParser.UniEl>(), false);
            //            "eyJ2ZXJzaW9uIjoiMS4wIiwiYWNjZXB0TGFuZ3VhZ2UiOiIiLCJhY2NlcHRIZWFkZXJzIjoidGV4dC9odG1sLGFwcGxpY2F0aW9uL3hodG1sK3htbCxhcHBsaWNhdGlvbi94bWw7cT0wLjksaW1hZ2UvYXZpZixpbWFnZS93ZWJwLGltYWdlL2FwbmcsKi8qO3E9MC44LGFwcGxpY2F0aW9uL3NpZ25lZC1leGNoYW5nZTt2PWIzO3E9MC43IiwidXNlckFnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgSGVhZGxlc3NDaHJvbWUvMTIwLjAuNjA5OS4xMzAgU2FmYXJpLzUzNy4zNiIsImJyb3dzZXJJcCI6IjEwLjguMTkuMjQxIiwidGltZVpvbmUiOiItMzAwIiwiY29sb3JEZXB0aCI6IjI0Iiwic2NyZWVuSGVpZ2h0IjoiMjQiLCJzY3JlZW5XaWR0aCI6IjgwMCIsImphdmFzY3JpcHRFbmFibGVkIjoidHJ1ZSIsImphdmFFbmFibGVkIjoiZmFsc2UifQ"

        }
        string[] ConvObject(string[] params1)
        {
            return new string[] { (Convert.ToDouble(params1[0]) / 1000).ToString() };
        }
        [Test]
        public void TestTransformer()
        {
            var tr1 = new RegexpTransformer();
            var arr = tr1.transform(new string[] { "22200*****037=2512" });
//            var tr = new CSScriptTransformer("(@Input1+@Input0)/10", true);
            var tr =new CSScriptTransformer("$\"20{@Input1}-{@Input2}-01T00:00:00\"");
            var out1=tr.transform(arr);
        }
    }
}
