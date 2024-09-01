using Markdig;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*using Aspose.Html.Converters;
using Aspose.Html.Saving;
*/
namespace ParserLibrary.PlantUmlGen
{
    public class MarkdownToHtml
    {
       public static void Test()
        {
            /*using var document = Converter.ConvertMarkdown(sourcePath);

            // Convert HTML document to PDF image file format
            Converter.ConvertHTML(document, new PdfSaveOptions(), savePath);*/
            using (StreamReader sr = new StreamReader("C:\\***\\example.md"))
            {
                
                var result = Markdown.ToHtml(sr.ReadToEnd());
                using (StreamWriter sw = new StreamWriter("C:\\***\\example.html"))
                {  sw.Write(result); }
            }

        }
  
    }
}
