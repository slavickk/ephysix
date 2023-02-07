using System.IO;
using System.Threading.Tasks;
using DummySystem1Protocols.DummyProtocol1;
using NUnit.Framework;

namespace ParserLibrary.Tests
{
    [TestFixture]
    public class DummyProtocol1SenderTests
    {
        [OneTimeSetUp]
        public void Init()
        {
            var dummyProtocol1Receiver = new DummyProtocol1Receiver()
            {
                port = _port,
                dummyProtocol1Frame = _senderDummyProtocol1Frame
            };
            dummyProtocol1Receiver.stringReceived = (s, o) => dummyProtocol1Receiver.sendResponseInternal(s, o);
            dummyProtocol1Receiver.startInternal();
        }

        private int _port = 5000;
        private int _senderDummyProtocol1Frame = 6;

        [Test]
        public async Task SenderTest()
        {
            DummyProtocol1Sender sender = new();
            sender.dummyProtocol1Frame = _senderDummyProtocol1Frame;
            sender.dummySystem3Host = "localhost";
            sender.dummySystem3Port = _port;

            byte[] bytes = File.ReadAllBytes("TestData/test200.dummy1")[2..];

            string respJson = await sender.send(DummyProtocol1Message.DeserializeToJSON(bytes),null);

            byte[] resp = DummyProtocol1Message.SerializeFromJson(respJson);
            Assert.AreEqual(bytes, resp);
        }

        [Test]
        public async Task SenderTest1()
        {
            var ticSender = new DummyProtocol1Sender() { dummyProtocol1Frame = 6, dummySystem3Host = "192.168.75.173", dummySystem3Port = 5593 };

            var ans = @"{""Fields"": {
                    ""AccountBalanceData"": {
                        ""AvailableBalance"": 0,
      ""BalanceCurrency"": false,
      ""LedgerBalance"": 0
                    },
    ""AcquiringInstitutionIdentification"": ""1"",
    ""AdditionalPOSData"": {
                        ""Clerk"": """",
      ""CVV2"": """",
      ""DraftCapture"": 1,
      ""InvoiceNumber"": """",
      ""PosBatchAndShiftData"": """",
      ""TransactionCategory"": 0
    },
    ""AcquirerFeeAmount"": {
                        ""IsWithdraw"": true,
      ""_isWithdraw"": ""D"",
      ""Amount"": 0
    },
    ""MBR"": 0,
    ""MiscellaneousTransactionAttributes"": {
                        ""Type"": 3,
      ""Value"": {
                            ""ET"": {
                                ""Value"": ""611915781490"",
          ""Type"": 0
                            },
        ""IC"": {
                                ""Value"": ""566"",
          ""Type"": 0
        },
        ""TC"": {
                                ""Value"": ""5"",
          ""Type"": 0
        }
                        }
                    },
    ""MiscellaneousTransactionAttributes2"": {
                        ""Type"": 3,
      ""Value"": {
                            ""UD"": {
                                ""Value"": ""mytextvaluee"",
          ""Type"": 0
                            },
        ""DI"": {
                                ""Value"": ""mytextvalue222"",
          ""Type"": 0
        },
        ""ITA"": {
                                ""Value"": ""qwerty123"",
          ""Type"": 0
        }
                        }
                    },
    ""PAN"": ""4555550000111158"",
    ""POSConditionCode"": 0,
    ""POSEntryMode"": {
                        ""EntryMethod"": 0,
      ""PinMethod"": 0
    },
    ""ProcessingCode"": {
                        ""FromAccountType"": 0,
      ""ToAccountType"": 0,
      ""TransactionCode"": 110
    },
    ""RegionalListingData"": ""123456789"",
    ""SIC"": 6012,
    ""SytemTraceAuditNumber"": 29,
    ""Track2"": ""4555550000111158=2201101143980752"",
    ""TransactionCurrencyCode"": 810,
    ""TransactionRetrievalReferenceNumber"": ""29"",
    ""TransmissionGreenwichTime"": ""1008121000"",
    ""CardAcceptorTerminal"": {
                        ""ID"": ""10631923"",
      ""Info"": {
                            ""Address"": """",
        ""Branch"": ""ADATAN ABEOKUTA"",
        ""City"": ""ABEOCUTA"",
        ""Class"": 1,
        ""CountryCode"": 51,
        ""CountyCode"": 0,
        ""Date"": ""20160428"",
        ""FiName"": ""DBN"",
        ""Owner"": ""DIAMOND BANK000000000000000000"",
        ""PSName"": ""MSCC"",
        ""Region"": """",
        ""RetailerName"": """",
        ""StateCode"": 0,
        ""TimeOffset"": 0,
        ""ZipCode"": 0
      }
                    }
                },
  ""Header"": {
                    ""ProtocolVersion"": 4,
    ""RejectStatus"": 0
  },
  ""MessageType"": {
                    ""IsReject"": false,
    ""TypeIdentifier"": 200
  }
            }";

            var ans1 = await ticSender.send(ans, null);
            Assert.NotNull(ans1);
        }
    }
}