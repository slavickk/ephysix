using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLibrary
{
    public class Metrics
    {
        List<Metric> allMetrics = new List<Metric>();

        object syncro = new object();
        public Metric getMetric(string Name,bool noAverage,bool isSuccess,string Comment="")
        {
            lock (syncro)
            {
                var metric = allMetrics.FirstOrDefault(oo => oo.Name == Name);
                if (metric == null)
                    allMetrics.Add(new Metric() { Name = Name, Comment = Comment, noAverage=noAverage, isSuccess=isSuccess });
                return metric;
            }
        }

        public string getPrometeusMetric()
        {
            string retValue = "";
            string lastName = "";            
            foreach(var metric in allMetrics.OrderBy(ii=>ii.Name))
            {
                if(metric.Comment != "" && lastName != metric.Name)
                {
                    retValue += "#  HELP " + metric.Name + ":" + metric.Comment + ".\r\n";
                    lastName = metric.Name;

                }
                retValue += (metric.Name + "{type=\"Count\",result=\""+(metric.isSuccess?"Success":"Error")+"\"} " + metric.getCount()+"\r\n");
                if(!metric.noAverage)
                    retValue += (metric.Name + "{type=\"Avg\",result=\"" + (metric.isSuccess ? "Success" : "Error") + "\"} " + metric.getAverage() + "\r\n");
                
            }
            return retValue;
        }


        public class Metric
        {
            public bool noAverage;
            public bool isSuccess;
            public string Name;
            public string Comment="";
            int count = 0;
            double sum = 0;
            public void Add(DateTime time)
            {
                count++;
                sum += (DateTime.Now - time).TotalMilliseconds;
            }
            public void Add(DateTime time, double value)
            {
                count++;
                sum += value;
            }
            public int getCount()
            {
                return count;

            }
            public double getAverage()
            {
                if (count == 0)
                    return 0;
                return sum/count;

            }

            

            /*public class Item
            {
                public string Name
                {
                    get
                    {
                        if (interval.Hours <= 1)
                            return "LastHour";
                        else
                        if (interval.Days <= 1)
                            return "LastDay";
                        else
                            return "AllTime";
                    }
                }
                public DateTime time= new DateTime(1970,1,1);
                public TimeSpan interval;
                public int count = 0;
                public double sum = 0;
                public void Add(DateTime time,double value)
                {
                    if(time>)
                }
        }*/
    }

    }
}
