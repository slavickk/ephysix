using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Dynamic;
using net.adamec.lib.common.dmn.engine.engine.decisions;
using net.adamec.lib.common.dmn.engine.engine.definition;
using net.adamec.lib.common.dmn.engine.engine.execution.context;
using net.adamec.lib.common.dmn.engine.parser;


namespace ParserLibrary
{
    public class DMNExecutorSender : Sender
    {
       static  List<net.adamec.lib.common.dmn.engine.parser.dto.DmnModel.Script> listScripts;
        //string[] allVariables = new string[] { };
        public class SignalledRule
        {
            public string RuleID;
            public int Severity;
            public string Result;
            public bool retValue=true;
        }
        public class RecordField
        {
            public string Key;
            public string Value;
        }
        public class ItemVar
        {
            public string Name { get; set; }
            public object Value { get; set; }
        }

        public class ItemInputVar
        {
            public string Name;
            public object Value;
        }
        List<ItemInputVar> dict = new List< ItemInputVar>();


        void getList(string Name)
        {

        }
        protected virtual List<ItemInputVar> PrepareData(AbstrParser.UniEl root)
        {
            dict.Clear();
            if (dict.Count == 0)
            {
                dict.Add(new ItemInputVar() { Name = "SignalledRules", Value = new List<SignalledRule>() });
                dict.Add(new ItemInputVar() { Name = "InputRecord", Value = new List<RecordField>()  });
            }
            foreach (var item in root.childs)
            {
                var it = dict.FirstOrDefault(ii => ii.Name == item.Name switch
                {
                    "InputParameters" => "SignalledRules",
                    "InputRecordFields" => "InputRecord",
                    _ => "none"
                });
                if (it != null)
                {
                    if (it.Name == "SignalledRules")
                        (it.Value as List<SignalledRule>).Add(new SignalledRule() { retValue = true, RuleID = item.childs.First(ii => ii.Name == "RuleID").Value.ToString(), Severity = Convert.ToInt32(item.childs.First(ii => ii.Name == "Severity").Value),  Result = item.childs.First(ii => ii.Name == "Result").Value.ToString() });
                    if (it.Name == "InputRecord")
                        (it.Value as List<RecordField>).Add(new RecordField() { Key = item.childs.First(ii => ii.Name == "Key").Value.ToString(), Value = item.childs.First(ii => ii.Name == "Value").Value.ToString() });


                }
            }

                return dict;
/*            return new List<ItemInputVar>() { new ItemInputVar() { Name = "SignalledRules", Value = new SignalledRule[] { new SignalledRule() { RuleID = "REX_TRAN_001", Severity = 1 }
, new SignalledRule() { RuleID = "REX_TRAN_002", Severity = 1 }
, new SignalledRule() { RuleID = "REX_TRAN_003", Severity = 1 } } }
            , new ItemInputVar() { Name = "InputRecord", Value = new RecordField[] { new RecordField() { Key = "TypeRecord", Value = "authTran" }, new RecordField() { Key = "PAN", Value = "454********623" }, new RecordField() { Key = "PhoneNumber", Value = "+7922****45" }, new RecordField() { Key = "Amount", Value = "900" } } }

            };*/
        }


        public void setXML(string xml)
        {
            XML = xml;
        }
        public string XML;
        async Task<string> execDmn(AbstrParser.UniEl root)
        {
/*            using(StreamReader sr= new StreamReader(@"C:\Camunda\DMNSummator.xml"))
            {
                XML= sr.ReadToEnd();    
            }*/
/*            object[] inputObjects = new object[]
    {
    new SignalledRule[] {new SignalledRule() { RuleID = "REX_TRAN_001",  Severity=1 }
,new SignalledRule() { RuleID = "REX_TRAN_002",  Severity=1 }
,new SignalledRule() { RuleID = "REX_TRAN_003",  Severity=1 } },
new RecordField[] { new RecordField() { Key = "TypeRecord", Value = "authTran" },new RecordField() { Key = "PAN", Value = "454********623" },  new RecordField() { Key = "PhoneNumber", Value = "+7922****45" } , new RecordField() { Key =  "Amount", Value="900"} }
    };*/
/*            object[] arr1 = new object[3];
            for (int i = 0; i < 3; i++)
            {
                dynamic employee = new ExpandoObject();
                ((IDictionary<String, Object>)employee).Add("RuleID", $"REX_TRAN_{i}");
                ((IDictionary<String, Object>)employee).Add("Severity", 1);
                arr1[i] = employee;
            }
            object[] arr2 = new object[2];
            {
                dynamic employee = new ExpandoObject();
                ((IDictionary<String, Object>)employee).Add("Key", "PAN");
                ((IDictionary<String, Object>)employee).Add("Value", "454323");
                arr2[0] = employee;
            }
            {
                dynamic employee = new ExpandoObject();
                ((IDictionary<String, Object>)employee).Add("Key", "Amount");
                ((IDictionary<String, Object>)employee).Add("Value", 200);
                arr2[1] = employee;
            }
            object[] inputObjects = new object[] { arr1, arr2 };
*/
            try
            {
                if(ctx== null)
                    ctx = getDMN(XML);
                List<IDmnDecision> executedDecision = new List<IDmnDecision>();

                foreach(var item in PrepareData(root))
                    ctx.WithInputParameter(item.Name, item.Value);

                //        ctx.ExecuteDecision("Scoring");
                ctx.ExecDecisions();
            }
            catch (Exception ex)
            {
                Logger.log("Error DMN:{exc}", Serilog.Events.LogEventLevel.Error, ex);
/*                exc = ex;
                if (ctx != null)
                    dmn_variables = ctx.Variables.ToArray();
                StateHasChanged();*/
                //                await JS.InvokeVoidAsync("alert", ex.Message);

                //                await JS.InvokeVoidAsync("getXML");

                return "";
            }

            var dmn_variables = ctx.Variables.Where(ii=>ii.Value.Type != null).Select(ii=>new ItemVar() { Name = ii.Value.Name, Value = ii.Value.Value }).ToArray();

            return JsonSerializer.Serialize<ItemVar[]>(dmn_variables);

//            StateHasChanged();
//            return true;
        }
        DmnExecutionContext ctx;

        private static DmnExecutionContext getDMN(string XML)
        {
            DmnExecutionContext ctx;
            var def = DmnParser.ParseString(XML, DmnParser.DmnVersionEnum.V1_3);
            //       allVariables = await formListVariables();
            listScripts = def.extensionElements.Script;
            var definition = DmnDefinitionFactory.CreateDmnDefinition(def);

            ctx = DmnExecutionContextFactory.CreateExecutionContext(definition);
            return ctx;
        }

        public override string getTemplate(string key)
        {
            return "{\"InputParameters\":[{\"RuleID\":\"\",\"Severity\":\"\",\"Score\":\"\"},{\"RuleID\":\"\",\"Severity\":\"\",\"Score\":\"\"}],\"InputRecordFields\":[{\"Key\": \"\",\"Value\": \"\"},{\"Key\": \"\",\"Value\": \"\"}]}";
            //            return base.getTemplate(key);
        }
        public override string getExample()
        {
            return "";
            //            return "{\"Define\":[]}";
        }

        public override TypeContent typeContent => TypeContent.internal_list;
        DateTime timeFinish;
        public async override Task<string> sendInternal(AbstrParser.UniEl root)
        {

            return await execDmn(root);
//            return ans.ToString();

        }


    }
}
