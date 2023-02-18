using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;


namespace WinFormsETLPackagedCreator
{
    public class GraphvizTest
    {

        public static Bitmap test(string body= "digraph{a -> b; b -> c; c -> a;}")
        {
            var getStartProcessQuery = new GetStartProcessQuery() ;
            var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
          
            var registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);

            // GraphGeneration can be injected via the IGraphGeneration interface

            var wrapper = new GraphGeneration(getStartProcessQuery,
                                              getProcessStartInfoQuery,
                                              registerLayoutPluginCommand);
            wrapper.GraphvizPath = @"C:\Program Files\Graphviz\bin\";

            byte[] output = wrapper.GenerateGraph(body, Enums.GraphReturnType.Png);

            Bitmap bmp;
            using (MemoryStream mStream = new MemoryStream())
            {
              //  byte[] pData = blob;
                mStream.Write(output, 0, Convert.ToInt32(output.Length));
                bmp = new Bitmap(mStream, false);
            }
           /* using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawImage(new Bitmap((@"C:\Users\Mena\Desktop\1.png"), new Point(182, 213));
            }*/
     //       pictureBox2.Image = bmp;
            return bmp;
        }

    }
}
