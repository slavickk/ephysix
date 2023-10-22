using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using UniElLib;
using YamlDotNet.Serialization;

namespace ParserLibrary;

public class ReplaySaver
{
    [YamlIgnore]
    public bool enable
    {
        get => string.IsNullOrEmpty(path);
    }
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
    public virtual string save(string input, string context = "")
    {
        if (enable)
        {
            if (queue == null)
            {
                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                } 
                queue = new ConcurrentQueue<SaveItem>();
                t = new Thread(writeToReplay);
                t.Start();

            }
            var fileName = context + Path.GetRandomFileName();
            SaveItem item = new SaveItem() { fileName = fileName, value = input.MaskSensitive() };
            queue.Enqueue(item);
            return item.fileName;
        }
        else
            return "";
    }

    public void Init()
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }
}