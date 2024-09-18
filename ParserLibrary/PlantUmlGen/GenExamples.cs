using Markdig;
using CodeHelper.Core.Extensions;

using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.OpenApi.Expressions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Markdig.Prism;

namespace ParserLibrary.PlantUmlGen
{
    public class GenExample
    {
        public string fileName { get; set; }
        public string ID { get; set; }
        public string Caption {  get; set; }
        public List<ExampleItem> steps { get; set; } = new List<ExampleItem>();
        public static async Task generateExample(string jsonFileName)
        {
            var rootDir = Pipeline.configuration["DATA_ROOT_DIR"];
            using (StreamReader sr = new StreamReader(Path.Combine(Path.Combine(rootDir, "Config"), jsonFileName)))

            {
                string json = sr.ReadToEnd();
                var cls1 = JsonSerializer.Deserialize<GenExample>(json);
                cls1.fileName = jsonFileName;
                var res11 = await cls1.Draw();
            }
        }
        private static readonly MarkdownPipeline MarkdownPipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .UsePrism()
            .Build();
        public async Task<string> Draw()
        {

            var retvalue = "# " + Caption + "\n";
            foreach (var step in steps)
            {
                step.owner = this;
                if (step.canDraw)
                {
                    var v1 = await step.getBody();
                    retvalue += "\n### " + step.Caption + "\n" + v1;
                }
            }
            //using CodeHelper.Core.Extensions;

            //            string result = retvalue.MarkDownToHtml();
            var aa = "@startsalt\r\n{+\r\nskinparam handwritten true\r\n\r\n  Услуга:<color:Blue>Билайн БК без комиссии д/б\r\n Оплата с карты | ^Карта 22202***01^\r\n\r\n\r\n Номер телефона (без +7):|\"<color:#9a9a9a>9206526152\"\r\n Сумма, руб.:|\"<color:#9a9a9a>1000  \"\r\nСумма к оплате: 1000.00руб.\r\nКомиссия: 0.00руб.\r\n| [  Начать оплату   ]\r\n}\r\n@endsalt";
            var result = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n\t    <meta charset=\"UTF-8\" />\r\n\t        <link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/prism/1.29.0/themes/prism-tomorrow.min.css\" integrity=\"sha512-vswe+cgvic/XBoF1OcM/TeJ2FW0OofqAVdCZiEYkd6dwGXthvkSFWOoGGJgS2CW70VK5dQM5Oh+7ne47s74VTg==\" crossorigin=\"anonymous\" referrerpolicy=\"no-referrer\" />\r\n    \r\n        <script src=\"https://cdnjs.cloudflare.com/ajax/libs/prism/1.29.0/prism.min.js\" integrity=\"sha512-7Z9J3l1+EYfeaPKcGXu3MS/7T+w19WtKQY/n+xzmw4hZhJ9tyYmcUS+4QqAlzhicE5LAfMQSF3iFTK9bQdTxXg==\" crossorigin=\"anonymous\" referrerpolicy=\"no-referrer\"></script>\r\n        <script src=\"https://cdnjs.cloudflare.com/ajax/libs/prism/1.29.0/plugins/autoloader/prism-autoloader.min.js\" integrity=\"sha512-SkmBfuA2hqjzEVpmnMt/LINrjop3GKWqsuLSSB3e7iBmYK7JuWw4ldmmxwD9mdm2IRTTi0OxSAfEGvgEi0i2Kw==\" crossorigin=\"anonymous\" referrerpolicy=\"no-referrer\"></script>\r\n</head>\r\n<body>" + Markdown.ToHtml(retvalue,MarkdownPipeline)+ "\t<script src=\"/themes/prism.js\"></script>\r\n</body>\r\n</html>";
           // var result = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n\t    <meta charset=\"UTF-8\" />\r\n\t        <link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/prism/1.29.0/themes/prism-tomorrow.min.css\" integrity=\"sha512-vswe+cgvic/XBoF1OcM/TeJ2FW0OofqAVdCZiEYkd6dwGXthvkSFWOoGGJgS2CW70VK5dQM5Oh+7ne47s74VTg==\" crossorigin=\"anonymous\" referrerpolicy=\"no-referrer\" />\r\n    \r\n        <script src=\"https://cdnjs.cloudflare.com/ajax/libs/prism/1.29.0/prism.min.js\" integrity=\"sha512-7Z9J3l1+EYfeaPKcGXu3MS/7T+w19WtKQY/n+xzmw4hZhJ9tyYmcUS+4QqAlzhicE5LAfMQSF3iFTK9bQdTxXg==\" crossorigin=\"anonymous\" referrerpolicy=\"no-referrer\"></script>\r\n        <script src=\"https://cdnjs.cloudflare.com/ajax/libs/prism/1.29.0/plugins/autoloader/prism-autoloader.min.js\" integrity=\"sha512-SkmBfuA2hqjzEVpmnMt/LINrjop3GKWqsuLSSB3e7iBmYK7JuWw4ldmmxwD9mdm2IRTTi0OxSAfEGvgEi0i2Kw==\" crossorigin=\"anonymous\" referrerpolicy=\"no-referrer\"></script>\r\n<link href=\"/themes/prism.css\" rel=\"stylesheet\" />\r\n</head>\r\n<body>" + Markdown.ToHtml(retvalue, MarkdownPipeline) + "\t<script src=\"/themes/prism.js\"></script>\r\n</body>\r\n</html>";
            using (StreamWriter sw = new StreamWriter(Path.Combine(Path.Combine(Pipeline.configuration["DATA_ROOT_DIR"], "wwwroot/Files"), fileName.Replace(".json", ".html"))))
            {
                sw.Write(result);
            }

            // retvalue += string.Join("\n", steps.Select(ii => "### " + ii.Caption + "\n" + await ii.getBody()));
            return retvalue;
        }

        [JsonDerivedType(typeof(PlantUmlItem), 1)]
        [JsonDerivedType(typeof(ScreenExampleFromJson), 2)]
        [JsonDerivedType(typeof(JsonExample), 3)]
        [JsonDerivedType(typeof(ScreenTemplate), 4)]
        
        public abstract class ExampleItem
        {
            public GenExample owner;
            public string ID { get; set; }

            public abstract bool canDraw { get; }
            public string Caption { get; set; }

            public virtual async Task<string> getBody()
            {
                return "";
            }
        }

        public class PlantUmlItem : ExampleItem
        {
            public override bool canDraw => true;
            public string PlantText { get; set; }
            static HttpClient  httpClient = new HttpClient(new HttpClientHandler { AllowAutoRedirect = true });
            string getFileName()
            {
                return  "Pict"+owner.ID+"-"+this.ID + ".svg";
            }
            string getFullPath()
            {
                return Path.Combine(Path.Combine(Pipeline.configuration["DATA_ROOT_DIR"], "wwwroot/files"),getFileName());
            }
            string getRelativePath()
            {
                return "/Files/"+ getFileName();
                //return Path.Combine( "wwwroot/files", getFileName());
            }

            public override async Task<string> getBody() 
            {
                var body = await PlantUmlGen.PlantUMLUrl.GetHTMLContent(httpClient, PlantText);
                if (!string.IsNullOrEmpty(body))
                {
                    using(StreamWriter sw = new StreamWriter(getFullPath()))
                    {
                        sw.Write(body);
                        return $"<img src=\"{getRelativePath()}\"/>"+"\n";
                    }
                   
                }
                else
                    return "\n\n *** error on generate PlantUMLFile \n\n'''\n" + PlantText + "\n'''\n";
            }
        }
        public class ScreenExampleFromJson : PlantUmlItem
        {

            public string IDJsonItem { get; set; }
            public string IDTemplateItem { get; set; }
            public override bool canDraw => true;

            public override async Task<string> getBody() {
                string json=(owner.steps.First(ii => ii.ID == IDJsonItem) as JsonExample).json.GetRawText();
                string template = (owner.steps.First(ii => ii.ID == IDTemplateItem) as ScreenTemplate).Template;
                this.PlantText=ShablonizeHelper.Shablonize(json, template);
                return await base.getBody();
            }
        }

        public class JsonExample : ExampleItem
        {
            public JsonElement json { get; set; }
            public override async Task<string> getBody() => "```json\n"+json.GetRawText()+ "\n```\n";

    
            public override bool canDraw => true;
        }
        public class ScreenTemplate : ExampleItem
        {
            public string Template { get; set; }
            public override async Task<string> getBody() => "";

            public override bool canDraw => false;

        }
    }
    public static class ShablonizeHelper
    {
        public static JToken filterForPath(this JToken jDoc, string path1)
        {
            if (jDoc.GetType() == typeof(JArray))
            {
                var paths = path1.Split("/");
                foreach (JToken currJdoc in jDoc )
                {
                    foreach (var path in paths)
                    {
                        var currJdoc1 = currJdoc[path];
                        if (currJdoc1 != null)
                            return currJdoc1;
                        if (currJdoc == null)
                            return null;
                    }
                    return currJdoc;

                }
                
            }
            else
            {
                JToken currJdoc = jDoc;
                var paths = path1.Split("/");
                foreach (var path in paths)
                {
                    currJdoc = currJdoc[path];
                    if (currJdoc == null)
                        return null;
                }
                return currJdoc;
            }
            return null;
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
                var ffind = token.filterForPath(path);
                return ffind?.ToString().ToUpper() == value.ToUpper();
            }
        }
        public static bool ifCond(this JToken token, IEnumerable<Cond> conditions)
        {
            foreach (var cond in conditions)
                if (!cond.isSignalled(token))
                    return false;
            return true;
        }


        public static IEnumerable<JToken> whileCond(this JToken tokens, IEnumerable<Cond> conditions)
        {
            foreach (var token in tokens)
                if (token.ifCond(conditions))
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
        public static string Shablonize(string json, string shablon)
        {
            JToken currToken = JObject.Parse(json);
            var startToken = currToken;
            string retValue = "";
            int indexStart = 0;
            List<LangItem> stack = new List<LangItem>();
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
                if (stack.Count == 0)
                    retValue += shablon.Substring(indexStart).Substring(0, index);
                else
                {
                    if (stack.Count > 0 && stack.Last().enabled)
                        stack.Last().buffer += shablon.Substring(indexStart).Substring(0, index);
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
                    if (stack.Last().currentIndex >= stack.Last().tokens.Count - 1 || (!stack.Last().enabled))
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
                    stack.Add(new LangItem() { text_token = text_token, indexStart = indexStart });
                    if (stack.Count < 2 || stack[stack.Count - 2].enabled)
                    {
                        switch (text_token)
                        {
                            case "while":
                                {
                                    var cond = tokens.Skip(1).Select(ii => ii.Split("=")).Select(i1 => new Cond(i1[0], i1[1]));
                                    if(currToken== null)
                                    {
                                        retValue += string.Join("", stack.Select(ii => ii.buffer)) + "***** null token ";
                                        return retValue;
                                    }
                                    stack.Last().tokens = currToken.whileCond(cond).ToList();
                                    break;
                                }
                            case "if":
                                {
                                    if (stack.Count == 1)
                                    {
                                        int yy = 0;
                                    }
                                    var cond = tokens.Skip(1).Select(ii => ii.Split("=")).Select(i1 => new Cond(i1[0], i1[1]));
                                    var signalled = currToken.ifCond(cond);
                                    if (signalled)
                                        stack.Last().tokens = new List<JToken> { currToken };
                                    else
                                        stack.Last().tokens = new List<JToken> { };
                                    break;
                                }
                            case "path":
                               // "@startsalt\r\n{+\r\nskinparam handwritten true\r\n\r\n \r\n Услуга:<color:Blue>ТСЖ Снайперов, 3\r\n==|==\r\n Идентификатор платежного документа|\"10\"\r\n Контактный телефон|\"10\"\r\n Номер лицевого счета|\"10\"\r\n Период платежа|\"10\"\r\n Адрес|\"Адрес\"\r\n Сумма|\"1000.00\"\r\n--|--\r\nСумма к оплате: 1027.50руб.\r\nКомиссия: 27.50руб.\r\n| [  Начать оплату   ]\r\n}\r\n@endsalt\r\n"
                                stack.Last().tokens = new List<JToken>() { currToken.filterForPath(tokens[1]) };
                                break;
                            case "val":
                                stack.RemoveAt(stack.Count - 1);
                                var val = currToken.filterForPath(tokens[1]);
                                string valString = "";
                                if (val == null)
                                {

                                }
                                else
                                {
                                    if (val.GetType() == typeof(JValue))
                                        valString = val.ToString();
                                    else
                                        valString = val.First().ToString();
                                }
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
                    if (stack.Count > 1 && !stack[stack.Count - 2].enabled)
                        stack.Last().enabled = false;
                    //stack.RemoveAt(stack.Count - 1);

                }
            }
            return retValue;
        }


    }


}
