using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Npgsql;
using NUnit.Framework;
using ParserLibrary.PlantUmlGen;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;
using DotLiquid;
using System.Text.Encodings.Web;
using System.IO;


namespace ParserLibrary.Tests
{
    public static class PlantUMLTests
    {
        public static JToken filterForPath(this JToken jDoc, string path1)
        {
            JToken currJdoc = jDoc;
            var paths = path1.Split("/");
            foreach (var path in paths)
            {
                currJdoc = currJdoc[path];
                if(currJdoc == null)
                    return null;
            }
            return currJdoc;
        }
        public class Cond
        {
            string path;
            string value;
            public Cond(string path, string value)
            {
                this.path = path;
                this.value = value;
            }
            public bool isSignalled(JToken token)
            {
                var ffind=token.filterForPath(path);
                return ffind?.ToString().ToUpper() == value.ToUpper();
            }
        }
        public static  bool ifCond(this JToken token, IEnumerable<Cond> conditions)
        {
            foreach (var cond in conditions)
                if (!cond.isSignalled(token))
                    return false;
            return true;
        }


        public static IEnumerable<JToken> whileCond(this JToken tokens,IEnumerable<Cond> conditions)
        {
            foreach(var token in tokens)
            if(token.ifCond(conditions))
                    yield return token;
        }
        public class LangItem
        {
            public bool enabled = true;
            public int indexStart;
            public string text_token;
            public List<JToken> tokens;
            public int currentIndex = -1;
            public string buffer;
            public bool multiply = false;
        }
        public static string Shablonize(string json,string shablon)
        {
            JToken currToken= JObject.Parse(json);
            var startToken = currToken;
            string retValue = "";
            int indexStart = 0;
            List<LangItem> stack= new List<LangItem>();
            while (0 == 0)
            {
                int index = shablon.Substring(indexStart).IndexOf("@@");
                if (index == -1)
                {
                    if (stack.Count == 0)
                        retValue += shablon.Substring(indexStart);
                    else
                        retValue += string.Join("", stack.Select(ii => ii.buffer)) + "***** expected " + string.Join(";", stack.Select(ii => "end" + ii.text_token));
                    return retValue;
                }
                if(stack.Count == 0)
                retValue += shablon.Substring(indexStart).Substring(0, index);
                else
                {
                    if(stack.Count >0 && stack.Last().enabled)
                        stack.Last().buffer+=shablon.Substring(indexStart).Substring(0, index);
                }
                int index1 = shablon.Substring(indexStart).Substring(index + 2).IndexOf("@@");
                if (index1 == -1)
                {
                    retValue += string.Join("", stack.Select(ii => ii.buffer)) + "**** expected @@";
                    return retValue;
                }
                var text_token = shablon.Substring(indexStart).Substring(index + 2, index1);
                indexStart += index + 4 + index1;
                //shablon = shablon.Substring(index + 4 + index1);
                if (text_token.StartsWith("end "))
                {
                    var wait_token = text_token.Split(" ")[1];
                    if (wait_token != stack.Last().text_token)
                    {
                        retValue += string.Join("", stack.Select(ii => ii.buffer)) + "**** expected @@end " + stack.Last().text_token + "@@";
                        return retValue;
                    }
                    //          var nextToken = stack.Last().tokens.GetEnumerator().First().MoveNext();
                    if (stack.Last().currentIndex >= stack.Last().tokens.Count - 1 ||(!stack.Last().enabled))
                    {
                        if (stack.Count == 1)
                            retValue += stack.Last().buffer;
                        else
                            stack[stack.Count - 2].buffer += stack.Last().buffer;
                        stack.Last().buffer = "";

                        stack.RemoveAt(stack.Count - 1);
                        if (stack.Count > 0)
                        {
                            if (stack.Last().enabled)
                                currToken = stack.Last().tokens[stack.Last().currentIndex];
                        }
                        else
                            currToken = startToken;
                    }
                    else
                    {
                        stack.Last().currentIndex++;
                        currToken = stack.Last().tokens[stack.Last().currentIndex];
                        indexStart = stack.Last().indexStart;
                    }

                }
                else
                {
                    var tokens = text_token.Split(" ");
                    text_token = tokens[0];
                    stack.Add(new LangItem() { text_token = text_token,indexStart=indexStart });
                    if (stack.Count < 2 || stack[stack.Count - 2].enabled)
                    {
                        switch (text_token)
                        {
                            case "while":
                                {
                                    var cond = tokens.Skip(1).Select(ii => ii.Split("=")).Select(i1 => new Cond(i1[0], i1[1]));
                                    stack.Last().tokens = currToken.whileCond(cond).ToList();
                                    break;
                                }
                            case "if":
                                {
                                    if(stack.Count ==1)
                                    {
                                        int yy = 0;
                                    }
                                    var cond = tokens.Skip(1).Select(ii => ii.Split("=")).Select(i1 => new Cond(i1[0], i1[1]));
                                    var signalled = currToken.ifCond(cond);
                                    if(signalled)
                                    stack.Last().tokens = new List<JToken> { currToken };
                                    else
                                        stack.Last().tokens = new List<JToken> {  };
                                    break;
                                }
                            case "path":
                                stack.Last().tokens = new List<JToken>() { currToken.filterForPath(tokens[1]) };
                                break;
                            case "val":
                                stack.RemoveAt(stack.Count - 1);
                                var val = currToken.filterForPath(tokens[1]);
                                string valString = "";
                                if (val.GetType() == typeof(JValue))
                                    valString = val.ToString();
                                else
                                    valString = val.First().ToString();
                                if (stack.Count > 0)
                                    stack.Last().buffer += valString;
                                else
                                    retValue += valString;
                                break;
                            default:
                                retValue += string.Join("", stack.Select(ii => ii.buffer)) + "**** unknown token @@" + text_token + "@@";
                                return retValue;
                        }
                    }
                    if (stack.Count > 0)
                    {
                        if (stack.Last().tokens.Count > 0 && (stack.Count < 2 || stack[stack.Count - 2].enabled))
                        {
                            if (stack.Last().currentIndex < 0)
                            {
                                stack.Last().currentIndex = 0;
                                currToken = stack.Last().tokens[stack.Last().currentIndex];
                            }
                        }
                        else
                            stack.Last().enabled = false;
                    }
                    if(stack.Count >1 && !stack[stack.Count-2].enabled)
                        stack.Last().enabled = false;
                    //stack.RemoveAt(stack.Count - 1);

                }
            }
            return retValue;
        }


        [Test]
        public static void Test3()
        {
            string json = @"
      {
  ""ABSMessage"": """",
  ""Error"": ""0"",
  ""Name"": ""Билайн БК без комиссии д/б"",
  ""FullName"": """",
  ""Fields"": {
    ""Field"": [
      {
        ""FieldTable"": {
          ""Col"": {
            ""Row"": {
              ""N"": """",
              ""ID"": """"
            },
            ""ID"": """",
            ""Name"": """",
            ""Visible"": """"
          }
        },
        ""ID"": """",
        ""Name"": """",
        ""Type"": """",
        ""Visible"": """"
      }
    ]
  },
  ""Input"": [
    {
      ""Col"": """",
      ""ID"": ""transactionId"",
      ""Order"": 1,
      ""Type"": ""Input"",
      ""DataType"": ""%String"",
      ""Print"": true,
      ""ReadOnly"": true,
      ""Visible"": false,
      ""Req"": false
    },
    {
      ""Col"": """",
      ""ID"": ""a3_NUMBER_1_2"",
      ""Order"": 2,
      ""Name"": ""Номер телефона (без +7):"",
      ""Type"": ""Input"",
      ""DataType"": ""%String"",
      ""Hint"": ""Пример: 9051111111"",
      ""Mask"": """",
      ""RegExp"": ""^\\d{10}$"",
      ""RightNum"": 0,
      ""Note"": ""Номер телефона должен содержать 10 цифр."",
      ""Print"": true,
      ""ReadOnly"": false,
      ""Visible"": true,
      ""Req"": false
    },
    {
      ""Col"": """",
      ""ID"": ""a3_AMOUNT_2_2"",
      ""Order"": 2,
      ""Name"": ""Сумма, руб.:"",
      ""Type"": ""Input"",
      ""DataType"": ""%Numeric"",
      ""Hint"": ""Пример: 1234.12"",
      ""Mask"": """",
      ""RegExp"": ""^\\d{10}$"",
      ""RightNum"": 0,
      ""Note"": ""Номер телефона должен содержать 10 цифр."",
      ""Print"": true,
      ""ReadOnly"": false,
      ""Visible"": true,
      ""Req"": true
    }
  ]
  ,
  ""Step"": 2,
  ""FinalStep"": """",
  ""ID"": ""AWGW579843"",
  ""Type"": ""AWASTEP"",
  ""AnsDateTime"": ""2024-08-13T20:31:22""
}
";
            string shablon = @"
@startsalt
{+
skinparam handwritten true

  Услуга:<color:Blue>@@val Name@@
@@path Input@@
@@while Visible=true@@
 @@val Name@@|@@if Type=Input@@""@@end if@@ @@if ReadOnly=True@@<color:#9a9a9a>@@end if@@--insert value--@@if Type=Input@@""@@end if@@
@@end while@@
@@end path@@
@@if FinalStep=true@@
Сумма к оплате: 1000.00руб.
Комиссия: 0.00руб.
| [  Начать оплату   ]
@@end if@@
}
@endsalt
";

            var ret=Shablonize(json, shablon);

            // Parse the JSON string into a JObject
            var jsonObject = JObject.Parse(json);
            foreach (var ffound in jsonObject.filterForPath("Input").whileCond(new List<Cond> { new Cond("Visible", "True") }))
            {
            }
//                Select all Inputs where Visible is true
            var visibleInputs = jsonObject["Input"]
                .Where(input => (bool)input["Visible"]);

            // Output the results
            foreach (var input in visibleInputs)
            {
                Console.WriteLine($"ID: {input["ID"]}, Name: {input["Name"]}");
            }
        }
    

    [Test]
        public static async Task testExamplesUml()
        {
            Environment.SetEnvironmentVariable("DATA_ROOT_DIR", "C:\\Users\\jurag\\source\\repos\\ephysix\\ParserLibrary\\Plugins\\SwaggerUIData\\");
            using (StreamReader sr = new StreamReader("C:\\Users\\jurag\\source\\repos\\ephysix\\ParserLibrary\\Plugins\\SwaggerUIData\\Config\\ExampleStep.json"))
            {
                string json = sr.ReadToEnd();
                var cls1 = JsonSerializer.Deserialize<GenExample>(json);
                var res11 = await cls1.Draw();
            }

                GenExample gen = new GenExample()
                {
                    Caption = "Example",
                    steps = new List<GenExample.ExampleItem>()
            {
                new GenExample.JsonExample()
                {
                     Caption="Шаг1",
                      ID="Id1",
                       json=JsonDocument.Parse("{\"body\":\"123\"}").RootElement

                },
                new GenExample.PlantUmlItem()
                {
                    Caption="Шаг2",
                     ID="ID2",
                      PlantText="@startsalt\r\n{+\r\nskinparam handwritten true\r\n\r\n  Услуга:<color:Blue>Билайн БК без комиссии д/б\r\n\r\n Номер телефона (без +7):|\"<color:#9a9a9a>9206526152\"\r\n Сумма, руб.:|\"<color:#9a9a9a>1000  \"\r\nСумма к оплате: 1000.00руб.\r\nКомиссия: 0.00руб.\r\n| [  Начать оплату   ]\r\n}\r\n@endsalt"

                }
            }
                };
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
,
                IgnoreNullValues = true
            };

            var res =JsonSerializer.Serialize(gen,options);
            var cls=JsonSerializer.Deserialize<GenExample>(res);
            //MarkdownToHtml.Test();
            string retValue = "";
          /*  retValue += (new GenExamples.JsonGen("Запрос", "{\r\n  \"ADDITIONAL\": [\r\n    {\r\n      \"FIELDNAME\": \"transactionId\",\r\n      \"FIELDID\": 1,\r\n      \"FIELDVALUE\": \"39984826\"\r\n    }\r\n  ],\r\n  \"PUREF\": \"PDOG||1228\",\r\n  \"PROVIDER\": \"\",\r\n  \"Step\": \"1\",\r\n  \"ID\": \"AWGW579843\",\r\n  \"customerId\": \"A123408767\",\r\n  \"A\": \"200\",\r\n  \"CUR\": \"RUB\"\r\n}") { }).GeneratePlant();
            retValue += (new GenExamples.JsonGen("Ответ", "{\r\n  \"STEPResult\": {\r\n    \"AWHead\": {\r\n      \"ABSMessage\": \"\",\r\n      \"Error\": \"0\"\r\n    },\r\n    \"Inputs\": {\r\n      \"Input\": [\r\n        {\r\n          \"Col\": \"\",\r\n          \"ID\": \"transactionId\",\r\n          \"Order\": 1,\r\n          \"Name\": \"Номер телефона (без +7):\",\r\n          \"Type\": \"Input\",\r\n          \"DataType\": \"%String\",\r\n          \"Hint\": \"Пример: 9051111111\",\r\n          \"Mask\": \"\",\r\n          \"RegExp\": \"^\\\\d{10}$\",\r\n          \"Min\": \"10\",\r\n          \"Max\": \"15000\",\r\n          \"Sum\": true,\r\n          \"RightNum\": 0,\r\n          \"Note\": \"Номер телефона должен содержать 10 цифр.\",\r\n          \"Print\": true,\r\n          \"ReadOnly\": true,\r\n          \"Visible\": false,\r\n          \"Req\": false,\r\n          \"OnChange\": \"\",\r\n          \"Template\": false\r\n        },\r\n        {\r\n          \"Col\": \"\",\r\n          \"ID\": \"Summa\",\r\n          \"Order\": 1,\r\n          \"Name\": \"Сумма, руб.:\",\r\n          \"Type\": \"Input\",\r\n          \"DataType\": \"%String\",\r\n          \"Hint\": \"Пример: 9051111111\",\r\n          \"Mask\": \"\",\r\n          \"RegExp\": \"^\\\\d{10}$\",\r\n          \"Min\": \"10\",\r\n          \"Max\": \"15000\",\r\n          \"Sum\": true,\r\n          \"RightNum\": 0,\r\n          \"Note\": \"Номер телефона должен содержать 10 цифр.\",\r\n          \"Print\": true,\r\n          \"ReadOnly\": true,\r\n          \"Visible\": false,\r\n          \"Req\": false,\r\n          \"OnChange\": \"\",\r\n          \"Template\": false\r\n        }\r\n\r\n      ]\r\n    },\r\n    \"Sums\": {\r\n      \"SumSTrs\": \"200\",\r\n      \"Fee\": \"0\",\r\n      \"AWGWFee\": \"0\",\r\n      \"AcquireFee\": \"0\",\r\n      \"PaymentMethod\": \"\",\r\n      \"PaymentCurrency\": \"\"\r\n    },\r\n    \"AWAnswer\": {\r\n      \"Vendor\": \"A3||4285\",\r\n      \"Status\": \"\",\r\n      \"Step\": 2,\r\n      \"PrevStep\": \"\",\r\n      \"FinalStep\": \"\",\r\n      \"NextVendor\": \"\",\r\n      \"InvoiceID\": \"\"\r\n    },\r\n    \"Actions\": \"\",\r\n    \"ID\": \"AWGW579843\",\r\n    \"Type\": \"AWASTEP\",\r\n    \"AnsDateTime\": \"2024-08-13T20:31:22\"\r\n  },\r\n  \"Step\": \"\"\r\n}\r\n") { }).GeneratePlant();
        */
            }
        [Test]
        public static void testUml()
        {
            NpgsqlConnectionStringBuilder conn = new NpgsqlConnectionStringBuilder();
//            conn.ConnectionString = "User ID=fp;Password=rav12\"34;Host=192.168.75.220;Port=5432;Database=fpdb;SearchPath=md;";
            conn.ConnectionString = "Host=192.168.75.220;Port=5432;Database=fpdb;SearchPath=md;";
            conn.Username = "User ID";
            conn.Password = "1234;1234";
            
            var dict = new List<PlantUMLItem>() {
                new PlantUMLItem()
                {
                    Name="Step1",
                     links=new List<PlantUMLItem.Link>()
                     {
                         new PlantUMLItem.Link()
                         {
                             NameRq="Link1", children=new PlantUMLItem()
                             {
                                 Name="Step2", color="#00FF00",
                                 links= new List<PlantUMLItem.Link>()
                                 {
                                     new PlantUMLItem.Link()
                                     {
                                         isError=true,NameRp="Return error",
                                          NameRq="Link155"
                                          , children=new PlantUMLItem() {
                                              Name="Item999"
                                          }
                                     }
                                 }
                             }
                         }
                         ,
                                                 new PlantUMLItem.Link()
                         {
                              NameRq="Link2", children=new PlantUMLItem()
                             {
                                 Name="Step3"
                             }
                         }

                     }
                }
            };
            var jsonBody=JsonSerializer.Serialize(dict);
            var st=PlantUMLItem.getUML("Tect" ,dict);

        }

    }
}
