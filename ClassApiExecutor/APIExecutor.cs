using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Json.Serialization;

namespace CamundaInterface
{
        public interface _ApiExecutor
        {
            public class ItemCommand
            {
                public string environment;
                public override string ToString()
                {
                    return Name;
                }
                public string Name;
                public class Parameter
                {
                    public string name;
                    public string fullPath;
                    public bool isDemand = false;
                    public List<string> alternatives = new List<string>();
                    
                }
                public string outputPath;

                public List<Parameter> parameters = new List<Parameter>();
                public class OutputItems
                {
                    public string path;
                    public string Name
                    {
                        get
                        {
                            int index = path.LastIndexOf('/');
                            if (index < 0)
                                return path;
                            return path.Substring( index+1);
                        }
                    }
                    public OutputItems parent = null;
                    public List<OutputItems> children = new List<OutputItems>();
                    public override string ToString()
                    {
                        return path;
                    }

                }
                public List<OutputItems> outputItems = new List<OutputItems>();
            }

            Task beginSessionAsync();
            Task<_ApiFilter> ExecAsync(ExecContextItem[] commands);
            public class ErrorItem
            {
                public string content;
                public string error;
            }
            ErrorItem getError();
            Task endSessionAsync();
            List<ItemCommand> getDefine();


        }
        public interface _ApiFilter
        {
            string[] filter(string path);

            (string name,string value)[] filterWithNames(string path);

        }

    public class ExecContextItem
    {
        public class ItemParam
        {
            public string Key { get; set; }
            public string FullAddr { get; set; }
            public object? Value { get; set; }
            public string? Variable { get; set; }

            public ItemParam()
            {

            }
            public ItemParam(string key, object value)
            {
                Key = key;
                Value = value;
            }

        }
        public ExecContextItem()
        {

        }
        public ExecContextItem(_ApiExecutor.ItemCommand command)
        {
            CommandItem = command;
        }
        [JsonIgnore]
        public _ApiExecutor.ItemCommand CommandItem;
        public string Command { get; set; }

        public List<ItemParam> Params { get; set; }
    }



}
