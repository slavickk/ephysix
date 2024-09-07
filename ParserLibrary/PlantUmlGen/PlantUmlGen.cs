using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ParserLibrary.PlantUmlGen
{
    /// <summary>
    /// Create a Links to a PlantUML from plain PlantUML content.
    /// See http://plantuml.com/plantuml for examples.
    /// </summary>
    public class PlantUMLUrl
    {


        static string ReplaceContext(string context, OpenApiDef def)
        {
            var path = def.paths.First(ii => ii.path.Substring(ii.path.LastIndexOf("/") + 1) == context);
            string ex = "";
            if(!string.IsNullOrEmpty(path.exampleDoc))
                ex= "      [[/Files/" + path.exampleDoc.Replace(".json", ".html") + " Пример использования]] ";
            return $"\\n \\n<i> [[/swagger/index.html#/INFO/get{context} {path.path}]]     [[/Files/{context}.html Диаграмма переходов]]  {ex}</i>";
        }
        static string FindAndReplaceSubstrings(string input, OpenApiDef def)
        {
            // Regular expression to find substrings starting with "{#" and ending with "#}"
            string pattern = @"{\#.*?\#}";

            // Find matches in the input string
            MatchCollection matches = Regex.Matches(input, pattern);

            // Extracting all occurrences into a list
         //   List<string> occurrences = new List<string>();
            foreach (Match match in matches)
            {
                input=input.Replace(match.Value, ReplaceContext(match.Value.Substring(2,match.Value.Length-4),def));
            }

            return input;
        }
        public static async Task GenerateHtml(string inputDir, string outputDir, OpenApiDef def)
        {
            var httpClient = new HttpClient(new HttpClientHandler { AllowAutoRedirect = true });

            foreach (var file in Directory.GetFiles(inputDir, "*.puml"))
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    var content = sr.ReadToEnd();
                    content = FindAndReplaceSubstrings(content, def);

                    var html = await GetHTMLContent(httpClient, content);
                    if (!string.IsNullOrEmpty(html))
                    {
                        using (StreamWriter sw = new StreamWriter(Path.Combine(outputDir, Path.GetFileNameWithoutExtension(file) + ".html")))
                        {
                            sw.WriteLine(html);
                        }
                    }
                }

            }
        }

        public static async Task<string> GetHTMLContent(HttpClient httpClient, string content)
        {
            var url = SVG(content);
            /*        HttpClientHandler handler = new HttpClientHandler();
                    handler.AllowAutoRedirect = true;*/
            // var link = "https://grnh.se/8dc368b82us?s=LinkedIn&source=LinkedIn";
            var responseMessage = await httpClient.GetAsync(url);
            var ans = await httpClient.GetAsync(responseMessage.Headers.Location);
            if (ans.IsSuccessStatusCode)
            {
                var html = await ans.Content.ReadAsStringAsync();
                return html;
            }
            return string.Empty;
        }
    
        private string _baseUrl = "http://plantuml.com/plantuml";
        private string _umlContent = "@startuml\nlicense\n@enduml";
        private string _kind = "uml";

        /// <summary>
        /// Convenient method to create a self contained PlantUML link http://plantuml.com/plantuml/svg/{content}
        /// If you want to use another base url than http://plantuml.com/plantuml please use the public ctor.
        /// </summary>
        /// <param name="content">PlantUML text</param>
        /// <returns>self contained PlantUML link http://plantuml.com/plantuml/svg/{content}</returns>
        public static string SVG(string content)
        {
            return Create()
                .WithUmlContent(content)
                .SvgStyle()
                .ToString();
        }

        /// <summary>
        /// Convenient method to create a self contained PlantUML link http://plantuml.com/plantuml/png/{content}
        /// If you want to use another base url than http://plantuml.com/plantuml please use the public ctor.
        /// </summary>
        /// <param name="content">PlantUML text</param>
        /// <returns>self contained PlantUML link http://plantuml.com/plantuml/png/{content}</returns>
        public static string PNG(string content)
        {
            return Create()
                .WithUmlContent(content)
                .PngStyle()
                .ToString();
        }

        /// <summary>
        /// Convenient method to create a self contained PlantUML link http://plantuml.com/plantuml/uml/{content}
        /// If you want to use another base url than http://plantuml.com/plantuml please use the public ctor.
        /// </summary>
        /// <param name="content">PlantUML text</param>
        /// <returns>self contained PlantUML link http://plantuml.com/plantuml/uml/{content}</returns>
        public static string UML(string content)
        {
            return Create()
                .WithUmlContent(content)
                .UmlStyle()
                .ToString();
        }

        /// <summary>
        /// Convenient method to create a self contained PlantUML link http://plantuml.com/plantuml/txt/{content}
        /// If you want to use another base url than http://plantuml.com/plantuml please use the public ctor.
        /// </summary>
        /// <param name="content">PlantUML text</param>
        /// <returns>self contained PlantUML link http://plantuml.com/plantuml/txt/{content}</returns>
        public static string ASCII(string content)
        {
            return Create()
                .WithUmlContent(content)
                .AsciiStyle()
                .ToString();
        }

        public static PlantUMLUrl Create()
        {
            return new PlantUMLUrl();
        }

        public PlantUMLUrl WithBaseUrl(string baseUrl)
        {
            _baseUrl = baseUrl;
            return this;
        }

        public PlantUMLUrl WithUmlContent(string umlContent)
        {
            _umlContent = umlContent;
            return this;
        }

        public PlantUMLUrl PngStyle()
        {
            _kind = "png";
            return this;
        }

        public PlantUMLUrl SvgStyle()
        {
            _kind = "svg";
            return this;
        }

        public PlantUMLUrl UmlStyle()
        {
            _kind = "uml";
            return this;
        }

        public PlantUMLUrl AsciiStyle()
        {
            _kind = "txt";
            return this;
        }

        public override string ToString()
        {
            var content = _umlContent;
            content = Uri.UnescapeDataString(content);
            var bytes = Zip(content);
            var base64 = EncodeAsBase64(bytes);
            return $"{_baseUrl}/{_kind}/{base64}";
        }

        #region Zipping

        private static byte[] Zip(string str)
        {
            using (var output = new MemoryStream())
            {
                using (var gzip = new DeflateStream(output, CompressionMode.Compress))
                {
                    var bytes = Encoding.UTF8.GetBytes(str);
                    gzip.Write(bytes, 0, bytes.Length);
                }
                return output.ToArray();
            }
        }

        #endregion

        #region PlantUML Base64 Encoding

        private static string EncodeAsBase64(byte[] data)
        {
            var r = "";
            for (var i = 0; i < data.Length; i += 3)
            {
                if (i + 2 == data.Length)
                {
                    r += AppendThreeBytes(data[i], data[i + 1], 0);
                }
                else if (i + 1 == data.Length)
                {
                    r += AppendThreeBytes(data[i], 0, 0);
                }
                else
                {
                    r += AppendThreeBytes(data[i], data[i + 1],
                        data[i + 2]);
                }
            }

            return r;
        }

        private static string AppendThreeBytes(int b1, int b2, int b3)
        {
            var c1 = b1 >> 2;
            var c2 = ((b1 & 0x3) << 4) | (b2 >> 4);
            var c3 = ((b2 & 0xF) << 2) | (b3 >> 6);
            var c4 = b3 & 0x3F;
            var r = "";
            r += EncodeSixBit(c1 & 0x3F);
            r += EncodeSixBit(c2 & 0x3F);
            r += EncodeSixBit(c3 & 0x3F);
            r += EncodeSixBit(c4 & 0x3F);
            return r;
        }

        private static char EncodeSixBit(int b)
        {
            if (b < 10)
            {
                return (char)(48 + b);
            }

            b -= 10;
            if (b < 26)
            {
                return (char)(65 + b);
            }

            b -= 26;
            if (b < 26)
            {
                return (char)(97 + b);
            }

            b -= 26;
            if (b == 0) return '-';
            return b == 1 ? '_' : '?';
        }

        #endregion
    }
}
