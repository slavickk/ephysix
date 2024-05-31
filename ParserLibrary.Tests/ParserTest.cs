/******************************************************************
 * File: ParserTest.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

using DotLiquid;
using NUnit.Framework;
using UniElLib;

namespace ParserLibrary.Tests
{
    public  class ParserTest
    {
       public ParserTest() { }
        [Test]
        [Benchmark(Description = "TestMetric")]

        public void test()
        {

            DateTime tim1 = DateTime.Now;
            var bool1=SimpleTest();
            var del1=(DateTime.Now-tim1).TotalMilliseconds;
            tim1 = DateTime.Now;
            SimpleTest();
            var del2 = (DateTime.Now - tim1).TotalMilliseconds;
            tim1 = DateTime.Now;
            SimpleTest();
            var del3 = (DateTime.Now - tim1).TotalMilliseconds;
        }

        public static bool SimpleTest()
        {
            ContextItem context = new ContextItem();
            context.list = new List<AbstrParser.UniEl>();
            var rootElement = AbstrParser.CreateNode(null, context.list, "Root");

            AbstrParser.ParseStringTest(@"{""Fields"":{""AccountBalanceData"":{""AvailableBalance"":0,""BalanceCurrency"":false,""LedgerBalance"":0},""AcquiringInstitutionIdentification"":""00000000001"",""AdditionalPOSData"":{""Clerk"":"""",""CVV2"":"""",""DraftCapture"":1,""InvoiceNumber"":"""",""PosBatchAndShiftData"":"""",""TransactionCategory"":0},""CardIssuerData"":{""AuthFIName"":""DEMO"",""AuthPSName"":""CPLS""},""AcquirerFeeAmount"":{""IsWithdraw"":true,""_isWithdraw"":""D"",""Amount"":0},""MBR"":0,""MiscellaneousTransactionAttributes"":{""Type"":3,""Value"":{""CC"":{""Value"":""0"",""Type"":0},""IB"":{""Value"":""11000"",""Type"":0},""IC"":{""Value"":""4"",""Type"":0},""SPA"":{""Value"":""0"",""Type"":0},""TCE"":{""Value"":""0"",""Type"":0}}},""MiscellaneousTransactionAttributes2"":{""Type"":3,""Value"":{""AED"":{""Value"":[{""Type"":1,""Value"":[{""Value"":""51"",""Type"":0},{""Type"":3,""Value"":{""BrowserInfo"":{""Value"":""eyJ2ZXJzaW9uIjoiMS4wIiwiYWNjZXB0TGFuZ3VhZ2UiOiIiLCJhY2NlcHRIZWFkZXJzIjoidGV4dC9odG1sLGFwcGxpY2F0aW9uL3hodG1sK3htbCxhcHBsaWNhdGlvbi94bWw7cT0wLjksaW1hZ2UvYXZpZixpbWFnZS93ZWJwLGltYWdlL2FwbmcsKi8qO3E9MC44LGFwcGxpY2F0aW9uL3NpZ25lZC1leGNoYW5nZTt2PWIzO3E9MC43IiwidXNlckFnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgSGVhZGxlc3NDaHJvbWUvMTIwLjAuNjA5OS4yMDAgU2FmYXJpLzUzNy4zNiIsImJyb3dzZXJJcCI6IjEwLjguMTkuMjQxIiwidGltZVpvbmUiOiItMzAwIiwiY29sb3JEZXB0aCI6IjI0Iiwic2NyZWVuSGVpZ2h0IjoiMjQiLCJzY3JlZW5XaWR0aCI6IjgwMCIsImphdmFzY3JpcHRFbmFibGVkIjoidHJ1ZSIsImphdmFFbmFibGVkIjoiZmFsc2UifQ=="",""Type"":0},""DsReferenceNumber"":{""Value"":""1000"",""Type"":0},""MessageCategory"":{""Value"":""01"",""Type"":0},""TdsReqAuthIndicator"":{""Value"":""01"",""Type"":0},""TdsServerTranId"":{""Value"":""e015ebfd-8c93-47b1-8aff-53831fc82a6a"",""Type"":0},""Term/acquirerBIN"":{""Value"":""2201380101"",""Type"":0},""Term/acquirerMerchantID"":{""Value"":""POS_1"",""Type"":0},""Term/deviceChannel"":{""Value"":""02"",""Type"":0}}}]},{""Type"":1,""Value"":[{""Value"":""52"",""Type"":0},{""Type"":3,""Value"":{""Extensions"":{""Value"":[{""Type"":1,""Value"":[{""Value"":""extensionField1"",""Type"":0},{""Value"":""ID1"",""Type"":0},{""Value"":""0"",""Type"":0},{""Value"":""{\u0022valueOne\u0022:\u0022value\u0022}"",""Type"":0}]},{""Type"":1,""Value"":[{""Value"":""CpCcfaDeviceGuid"",""Type"":0},{""Value"":""CpCcfaDeviceGuid"",""Type"":0},{""Value"":""0"",""Type"":0},{""Value"":""{ \u0022guid\u0022: \u0022541b19ee-3884-4afa-8406-f8dbbe06de8e\u0022 }"",""Type"":0}]},{""Type"":1,""Value"":[{""Value"":""CCFA_3DS"",""Type"":0},{""Value"":""374890234"",""Type"":0},{""Value"":""0"",""Type"":0},{""Value"":""{\u00223ds\u0022:\u0022challenge\u0022,\u0022declinereason\u0022:\u0022No rules signalled.Default behaviour\u0022}"",""Type"":0}]}],""Type"":2}}}]}],""Type"":2},""EC"":{""Type"":3,""Value"":{""3DS/3DS2ProtovolVer"":{""Value"":""2.2.0"",""Type"":0},""3DS/CallbackUrl"":{""Value"":"""",""Type"":0},""3DS/DSTransId"":{""Value"":""21089df6-d761-48e6-9090-f0c0415d6ec3"",""Type"":0},""3DS/ExpTimeInterval"":{""Value"":""30"",""Type"":0},""3DS/MessCategory"":{""Value"":""01"",""Type"":0},""3DS/PrevACSTranId"":{""Value"":"""",""Type"":0},""3DS/ReqAuthIndicator"":{""Value"":""01"",""Type"":0},""3DS/TdsServerTranId"":{""Value"":""e015ebfd-8c93-47b1-8aff-53831fc82a6a"",""Type"":0},""3DS/acsTranId"":{""Value"":""29cb25da-8a50-4c5f-9f46-effb2053dcd3"",""Type"":0},""Ext/Network"":{""Value"":""44"",""Type"":0},""Ext/NetworkRid"":{""Value"":""ACS"",""Type"":0},""Merchant/BIN"":{""Value"":""2201380101"",""Type"":0},""Merchant/Name"":{""Value"":""compass"",""Type"":0},""Merchant/Rid"":{""Value"":""2201380101"",""Type"":0},""Merchant/URL"":{""Value"":""http://compassplus.ru"",""Type"":0},""3DS/PrepareAuthTranId"":{""Value"":""9909707"",""Type"":0}}},""GT"":{""Value"":""20240109224115"",""Type"":0}}},""NumericMessage"":0,""PAN"":""22200*****038"",""POSConditionCode"":0,""POSEntryMode"":{""EntryMethod"":0,""PinMethod"":1},""ProcessingCode"":{""FromAccountType"":1,""ToAccountType"":0,""TransactionCode"":623},""SecureData3D"":""0000000021089df6d76148e69090f0c0415d6ec3"",""SIC"":3000,""SytemTraceAuditNumber"":278065,""Track2"":""22200*****038=2512"",""TransactionAmount"":2537,""TransactionCurrencyCode"":840,""TransactionRetrievalReferenceNumber"":""000009909707"",""TransmissionGreenwichTime"":""0109174121"",""CardAcceptorTerminal"":{""ID"":""EC2"",""Info"":{""Address"":"""",""Branch"":"""",""City"":"""",""Class"":4,""CountryCode"":643,""CountyCode"":0,""Date"":""00000000"",""FiName"":"""",""Owner"":""POS_1"",""PSName"":""CPLS"",""Region"":"""",""RetailerName"":""compass_mrc_1"",""StateCode"":0,""TimeOffset"":0,""ZipCode"":0}},""LocalTransactionTime"":""22:41:15"",""LocalTransactionDate"":""2024-01-09T00:00:00""},""Header"":{""ProtocolVersion"":19,""RejectStatus"":0},""MessageType"":{""IsReject"":false,""TypeIdentifier"":103}}", context.list, rootElement, true);

            var find = "Root/Fields/MiscellaneousTransactionAttributes2/Value/AED/Value/Value/Value/BrowserInfo/Value/timeZone";
            var tokens = find.Split("/");

            var el = rootElement.getAllDescentants(tokens, 0, context).First();
            // el.Value = "500";
            var el1 = rootElement.getAllDescentants(tokens.Take(tokens.Length - 1).ToArray(), 0, context).First();
            string val = el1.Value.ToString();
            var el3 = el1.copy(AbstrParser.CreateNode(null, new List<AbstrParser.UniEl>(), "Root1"));

            return el3.Value.ToString() == val;
        }
        [Test]

        public void TestMethod1()
        {
            BenchmarkRunner.Run<ParserTest>();
            //            BenchmarkRunner.Run<TheEasiestBenchmark>();
        }
    }
}
