using ETL_DB_Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public class CustomVariableCoverter : JsonConverter
    {
        private readonly Type[] _types;

        public CustomVariableCoverter(params Type[] types)
        {
            _types = types;
        }

        public static string ConvertFromNodes(List<GenerateStatement.ItemVar> variables)
        {
            var json=JsonConvert.SerializeObject(variables, Formatting.Indented, new CustomVariableCoverter(typeof(GenerateStatement.ItemVar)));

            Console.WriteLine(json);
            return json;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken t = JToken.FromObject(value);

            if (t.Type != JTokenType.Object)
            {
                t.WriteTo(writer);
            }
            else
            {
                var val1 = value as GenerateStatement.ItemVar;
                JObject o = (JObject)t;
                IList<string> propertyNames = o.Properties().Select(p => p.Name).ToList();

                o.AddFirst(new JProperty(val1.Name, new JArray(propertyNames)));

                o.WriteTo(writer);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return _types.Any(t => t == objectType);
        }
    }

    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IList<string> Roles { get; set; }
    }
    internal class CustomVariableSerializer
    {
    }
}
