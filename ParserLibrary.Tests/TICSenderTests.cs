using System.IO;
using System.Threading.Tasks;
using CCFAProtocols.TIC;
using NUnit.Framework;
using PluginBase;
using UniElLib;


namespace ParserLibrary.Tests
{
    [TestFixture]
    public class TICSenderTests
    {
        [OneTimeSetUp]
        public void Init()
        {
            var ticReciever = new TICReceiver()
            {
                port = _port,
                ticFrame = _senderTicFrame
            };
            ticReciever.stringReceived = (s, o) => ticReciever.sendResponse(s,new ContextItem() { context = o });
            ticReciever.start();
        }

        private int _port = 5000;
        private int _senderTicFrame = 6;

        [Test]
        public async Task SenderTest()
        {
            TICSender sender = new();
            sender.ticFrame = _senderTicFrame;
            sender.twfaHost = "localhost";
            sender.twfaPort = _port;

            byte[] bytes = File.ReadAllBytes("TestData/test200.tic")[2..];

            string respJson = await sender.send(TICMessage.DeserializeToJSON(bytes),null);

            byte[] resp = TICMessage.SerializeFromJson(respJson);
            Assert.AreEqual(bytes, resp);
        }

        [Test]
        public async Task SenderTest1()
        {
            var ticSender = new TICSender() { ticFrame = 6, twfaHost = "192.168.75.173", twfaPort = 5593 };

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