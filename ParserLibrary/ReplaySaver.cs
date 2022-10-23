using System.Collections.Concurrent;
using System.IO;
using System.Threading;

namespace ParserLibrary;

public class ReplaySaver
{
    public string path;
    ConcurrentQueue<string> queue = null;
    Thread t;
    void writeToReplay()
    {
        //            using (StreamWriter sw = new StreamWriter(@"Log.info"))
        {
            for (; ; )
            {
                string el;
                while (queue.TryDequeue(out el))
                {
                    var fileName=Path.GetRandomFileName();
                    using(StreamWriter sw = new StreamWriter(Path.Combine(path,fileName)))
                    {
                        sw.Write(el);

                    }
                    el = null;
                }

                Thread.Sleep(100);
            }
        }
    }
    public virtual void save(string input)
    {
        if (queue == null)
        {
            queue = new ConcurrentQueue<string>();
            t = new Thread(writeToReplay);
            t.Start();
        }
        queue.Enqueue(input);
    }

}