/******************************************************************
 * File: TransformerTests.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARDummyProtocol1ULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Diagnostics.Tracing.Parsers;
using NUnit.Framework;
using UniElLib;

namespace ParserLibrary.Tests
{
    public class TransformerTests
    {
        [Test]
        public void TestPreo()
        {
            var p1 = AbstrParser.getApropriateParser("",
                @"eyJ2ZXJzaW9uIjoiMS4wIiwiYWNjZXB0TGFuZ3VhZ2UiOiIiLCJhY2NlcHRIZWFkZXJzIjoidGV4dC9odG1sLGFwcGxpY2F0aW9uL3hodG1sK3htbCxhcHBsaWNhdGlvbi94bWw7cT0wLjksaW1hZ2UvYXZpZixpbWFnZS93ZWJwLGltYWdlL2FwbmcsKi8qO3E9MC44LGFwcGxpY2F0aW9uL3NpZ25lZC1leGNoYW5nZTt2PWIzO3E9MC43IiwidXNlckFnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgSGVhZGxlc3NDaHJvbWUvMTIwLjAuNjA5OS4yMDAgU2FmYXJpLzUzNy4zNiIsImJyb3dzZXJJcCI6IjEwLjguMTkuMjQxIiwidGltZVpvbmUiOiItMzAwIiwiY29sb3JEZXB0aCI6IjI0Iiwic2NyZWVuSGVpZ2h0IjoiMjQiLCJzY3JlZW5XaWR0aCI6IjgwMCIsImphdmFzY3JpcHRFbmFibGVkIjoidHJ1ZSIsImphdmFFbmFibGVkIjoiZmFsc2UifQ==",
                new AbstrParser.UniEl(), new List<AbstrParser.UniEl>(), false);
            Assert.IsTrue(p1);

            var p2 = AbstrParser.getApropriateParser("",
                @"eyJCcmFuZCI6InhpYW9taSIsIkhhc05mY0hjZSI6ZmFsc2UsIkhhc0ZwU2Nhbm5lciI6dHJ1ZSwiSWQiOiIwNjZhY2MwMDE4ZDA1NWRiIiwiTWFudWZhY3RvcmVyIjoiWGlhb21pIiwiTW9kZWwiOiJSZWRtaSBOb3RlIDciLCJPc1ZlcnNpb24iOjI4LCJQcm9kdWN0IjoiQW5kcm9pZCIsIlJhbU1iIjoyNTYsIlNjcmVlbkRwaSI6IlhYSERQSSIsIlNjcmVlbkhlaWdodFB4IjoyMTMxLCJTY3JlZW5XaWR0aFB4IjoxMDgwLCJTY3JlZW5TaXplSW5jaGVzIjoyLjYzNzgwMDd9",
                new AbstrParser.UniEl(), new List<AbstrParser.UniEl>(), false);
            Assert.IsTrue(p2);

            var p3 = AbstrParser.getApropriateParser("",
                @"eyJCcmFuZCI6InhpYW9taSIsIkhhc05mY0hjZSI6ZmFsc2UsIkhhc0ZwU2Nhbm5lciI6dHJ1ZSwiSWQiOiIwNjZhY2MwMDE4ZDA1NWRiIiwiTWFudWZhY3RvcmVyIjoiWGlhb21pIiwiTW9kZWwiOiJSZWRtaSBOb3RlIDciLCJPc1ZlcnNpb24iOjI4LCJQcm9kdWN0IjoiQW5kcm9pZCIsIlJhbU1iIjoyNTYsIlNjcmVlbkRwaSI6IlhYSERQSSIsIlNjcmVlbkhlaWdodFB4IjoyMTMxLCJTY3JlZW5XaWR0aFB4IjoxMDgwLCJTY3JlZW5TaXplSW5jaGVzIjoyLjYzNzgwMDd9",
                new AbstrParser.UniEl(), new List<AbstrParser.UniEl>(), false);
            Assert.IsTrue(p3);
        }

        string[] ConvObject(string[] params1)
        {
            return new string[] { (Convert.ToDouble(params1[0]) / 1000).ToString() };
        }
        [Test]
        public void test5()
        {
            var res=Transliteration.Front("У попа была собака");
        }
        [Test]
        public void TestTransformer()
        {
            var tr1 = new RegexpTransformer();
            var arr = tr1.transform(new string[] { "22200*****037=2512" });
            
            Assert.NotNull(arr);
            // TODO: confirm the expected value, I just used the actual value as expected when writing the test
            Assert.That(arr, Is.EqualTo(new[] { "=2512", "25", "12" }));
            
            var tr =new CSScriptTransformer("$\"20{@Input1}-{@Input2}-01T00:00:00\"");
            var out1=tr.transform(arr);
            
            Assert.NotNull(out1);
            // TODO: confirm the expected value, I just used the actual value as expected when writing the test
            Assert.That(out1.Length, Is.EqualTo(1));
            Assert.That(out1[0], Is.EqualTo("2025-12-01T00:00:00"));
        }
    }
}
