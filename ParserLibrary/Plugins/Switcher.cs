using DynamicExpresso;
using NJsonSchema.CodeGeneration.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniElLib;
using YamlDotNet.Serialization;
using static ParserLibrary.PlantUmlGen.ShablonizeHelper;

namespace ParserLibrary.Plugins
{
    public abstract class Switcher
    {
        public string description;
      
        abstract public Step nextStep(Step currentStep,AbstrParser.UniEl el, ContextItem context);
        abstract public void Init(Step currentStep);
       
    }

    public abstract class AlternateSwitcher:Switcher
    {
        public class ChoiseItem
        {
            public object value;
            public string stepName;
        }
        public List<ChoiseItem> alternatives= new List<ChoiseItem>();
        public Step choise(Step[] steps,object value)
        {
            var choise = alternatives.FirstOrDefault(t => t.value == value);
            if(choise == null) 
                choise =alternatives.FirstOrDefault(t => t.stepName == null);
            if (choise != null)
                return steps.First(ii => ii.IDStep == choise.stepName);
            return null;
        }
    }
    public class ExpressionPathsSwitcher: AlternateSwitcher
    {
        public class PathItem
        {
            public string NamePar;
            public string NameTypePar;
            public string path;
        }
        public string expression;
        public List<PathItem> vars = new List<PathItem>();

        public string[] getAvailTypes()
        {
            return new string[] { "string", "int", "double", "bool" };
        }
        Type getType(string val)
        {
            switch (val)
            {
                case "string": return typeof(string);
                case "int": return typeof(int);
                case "double": return typeof(double);
                case "bool": return typeof(bool);

                default: return typeof(string);
            }
        }
        public static object getValue(string val, string Type)
        {
            switch (Type)
            {
                case "string": return val;
                case "int": return Convert.ToInt32(val);
                case "double": return Convert.ToDouble(val);
                case "bool": return Convert.ToBoolean(val);

                default: return val;
            }
        }

        public override Step nextStep(Step currentStep, AbstrParser.UniEl root,ContextItem context)
        {
            object[] values= new object[conds.Length];
            for(int i=0; i <  conds.Length;i++)
            {
                var nod = conds[i].getNodes(root, context).FirstOrDefault();
                if(nod != null)
                {
                    values[i] = getValue(nod?.Value.ToString(), vars[i].NameTypePar);
                }
            }
            var res=parsedExpression.Invoke(values);
            return choise(currentStep.owner.steps, res);
        //    throw new NotImplementedException();
        }
        [YamlIgnore]
        Interpreter interpreter = new Interpreter();//.SetVariable("service", new ServiceExample());

        [YamlIgnore]
        ExtractFromInputValue[] conds;
        [YamlIgnore]
        Lambda parsedExpression;



        public Type getExprType()
        {
            return parsedExpression.ReturnType;
        }
        public override void Init(Step currentStep)
        {
            Interpreter interpreter = new Interpreter();//.SetVariable("service", new ServiceExample());
            var parameters = new List<Parameter>();
            List<object> values = new List<object>();
            var conds = new List<ExtractFromInputValue>();
            foreach (var item in vars)
            {
                ExtractFromInputValue cond = new ExtractFromInputValue() { conditionPath = item.path };
                conds.Add(cond);
              /*  var nod = cond.getNodes(list[0], new ContextItem()).FirstOrDefault();
                if (nod != null)*/
                    parameters.Add(new Parameter(item.NamePar, getType(item.NameTypePar)));
     //           values.Add(getValue(nod?.Value.ToString(), item.SubItems[1].Text));
            }
    //        string expression = textBox2.Text;// "(x > 4 && y==2) ? 1 : 2";
            parsedExpression = interpreter.Parse(expression, parameters.ToArray());
            
            //values=new object
        }
        
    }

}
