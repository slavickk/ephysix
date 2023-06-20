using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

namespace ParserLibrary;

public class ReplaySaver
{
    public string path;

    public class SaveItem
    {
        public string fileName; 
        public string value;
    }

    ConcurrentQueue<SaveItem> queue = null;
    Thread t;
    void writeToReplay()
    {
        //            using (StreamWriter sw = new StreamWriter(@"Log.info"))
        {
            for (; ; )
            {
                SaveItem el;
                while (queue.TryDequeue(out el))
                {
//                    var fileName=Path.GetRandomFileName();
                    using(StreamWriter sw = new StreamWriter(Path.Combine(path,el.fileName/*fileName*/)))
                    {
                        sw.Write(el.value);

                    }
                    el = null;
                }

                Thread.Sleep(100);
            }
        }
    }
    public virtual string save(string input, string extension = "")
    {
        if (queue == null)
        {
            queue = new ConcurrentQueue<SaveItem>();
            t = new Thread(writeToReplay);
            t.Start();

        }
        var fileName = Path.GetRandomFileName();
        if(extension!="")
        {
            fileName=path.Replace(Path.GetExtension(fileName), extension);
        }
        SaveItem item = new SaveItem() { fileName = fileName, value = input }; 
        queue.Enqueue(item);
        return item.fileName;
    }

    public void Init()
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }
}