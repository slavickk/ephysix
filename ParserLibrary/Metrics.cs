using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YamlDotNet.Core.Tokens;

namespace ParserLibrary
{
    public class MetricsOld
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
                    retValue += "#  HELP " + metric.Name + ":" + metric.Comment + ".\n";
                    lastName = metric.Name;

                }
                retValue += (metric.Name + "{type=\"Count\",result=\""+(metric.isSuccess?"Success":"Error")+"\"} " + metric.getCount()+"\n");
                if(!metric.noAverage)
                    retValue += (metric.Name + "{type=\"Avg\",result=\"" + (metric.isSuccess ? "Success" : "Error") + "\"} " + metric.getAverage() + "\n");
                
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

    public class Metrics
    {

        public static Metrics metric = new Metrics();


        public List<Metric> allMetrics = new List<Metric>();

        object syncro = new object();
        public Metric getMetricCount(string Name, string Comment = "", bool isSuccess = true)
        {
            lock (syncro)
            {
                var metric = allMetrics.FirstOrDefault(oo => oo.Name == Name);
                if (metric == null)
                {
                    metric = new MetricCount(Name,Comment) {  isSuccess = isSuccess };
                    allMetrics.Add(metric);
                }

                return metric;
            }

        }
        public Metric getMetricAbs(string Name, string Comment = "", bool isSuccess = true)
        {
            lock (syncro)
            {
                var metric = allMetrics.FirstOrDefault(oo => oo.Name == Name);
                if (metric == null)
                {
                    metric = new MetricAbs(Name,Comment) {  isSuccess = isSuccess };
                    allMetrics.Add(metric);
                }

                return metric;
            }

        }
        public static double[] getDiapHistogram()
        {
            return new double[] { 1, 3, 5, 10, 20, 30, 50, 100, 200, 500, 700, 1000, 3000, 10000 };
        }
        public Metric getMetricHistogram(string Name, string Comment = "", double[] diaps = null)
        {
            lock (syncro)
            {
                var metric = allMetrics.FirstOrDefault(oo => oo.Name == Name);
                if (metric == null)
                {
                    metric = new MetricHistogram(Name, Comment, diaps);
                    allMetrics.Add(metric);
                }

                return metric;
            }
        }


        public string getPrometeusMetric()
        {
            string retValue = "";
            string lastName = "";
            foreach (var metric in allMetrics.OrderBy(ii => ii.Name))
            {
                retValue += metric.getHeader(ref lastName);
                /*                if (metric.Comment != "" && lastName != metric.Name)
                                {
                                    retValue += "#  HELP " + metric.Name + ":" + metric.Comment + ".\n";
                                    lastName = metric.Name;

                                }*/
                retValue += metric.getBody();

            }
            retValue += "# EOF\n";
            return retValue;
        }

        public abstract class Metric
        {
            public Metric()
            {
                Metrics.metric.allMetrics.Add(this);
            }
            public string Name;
            public string Comment = "";
            public enum Type { count, histogram };
            public abstract Type typeMetric
            {
                get;
            }
            public abstract string getHeader(ref string lastName);
            public abstract string getBody();
            abstract public void Add(double value);
            public static double Add(ref double totalValue, double addend)
            {
                double initialValue, computedValue;
                do
                {
                    // Save the current running total in a local variable.
                    initialValue = totalValue;


                    // Add the new value to the running total.
                    computedValue = initialValue + addend;



                    // CompareExchange compares totalValue to initialValue. If
                    // they are not equal, then another thread has updated the
                    // running total since this loop started. CompareExchange
                    // does not update totalValue. CompareExchange returns the
                    // contents of totalValue, which do not equal initialValue,
                    // so the loop executes again.
                } while (initialValue != Interlocked.CompareExchange(
                    ref totalValue, computedValue, initialValue));
                // If no other thread updated the running total, then 
                // totalValue and initialValue are equal when CompareExchange
                // compares them, and computedValue is stored in totalValue.
                // CompareExchange returns the value that was in totalValue
                // before the update, which is equal to initialValue, so the 
                // loop ends.



                // The function returns computedValue, not totalValue, because
                // totalValue could be changed by another thread between
                // the time the loop ends and the function returns.
                return computedValue;
            }

        }
        public class MetricHistogram : Metric
        {
            double[] diapasones = new double[] { 1, 3, 5, 10, 20, 30, 50, 100, 200, 500, 700, 1000, 3000, 10000 };
            public class Item
            {
                public int count = 0;
                public double sum = 0;
            }
            public Item[] items;

            public MetricHistogram(string name, string description, double[] diapasones1)
            {
                diapasones = diapasones1;
                this.Name = name;
                Comment = description;
                items = Enumerable.Range(0, diapasones.Length + 1).Select(ii => new Item()).ToArray();
            }
            public static int BinarySearch(double[] array, double searchedValue/*,out int cycle*/)
            {
                //                cycle = 0;
                int left = 0;
                int right = array.Length;
                if (searchedValue > array[^1])
                    return array.Length;
                if (searchedValue <= array[0])
                    return 0;

                //пока не сошлись границы массива
                while (left <= right)
                {
                    //                  cycle++;
                    if (right - left == 1)
                        return right;
                    //индекс среднего элемента
                    var middle = (left + right) / 2;

                    if (searchedValue == array[middle])
                    {
                        return middle;
                    }
                    else if (searchedValue < array[middle])
                    {
                        //сужаем рабочую зону массива с правой стороны
                        right = middle;
                    }
                    else
                    {
                        //сужаем рабочую зону массива с левой стороны
                        left = middle;
                    }
                }
                //ничего не нашли
                return -1;
            }
            override public void Add(double value)
            {
                var index = BinarySearch(diapasones, value);
                Interlocked.Increment(ref items[index].count);
                Add(ref items[index].sum, value);

                //                items[index].sum += value;
            }
            /*            int getIndex(double value)
                        {
                            int firstInd = 0;
                            int lastInd = diapasones.Length - 1;
                            if (value > diapasones[^1])
                                return diapasones.Length;
                            int newInd = (lastInd - firstInd) / 2;
                            if()
                        }*/
            public override Type typeMetric => Type.histogram;
            string getItemValue(int index)
            {
                if (index < diapasones.Length)
                    return diapasones[index].ToString().Replace(",", ".");
                return "+Inf";
            }
            public override string getBody()
            {
                string retValue = "";
                int totalCount = 0;
                double totalSum = 0;
                for (int i = 0; i < items.Length; i++)
                {
                    totalCount += items[i].count;
                    totalSum += items[i].sum;
                    retValue += "prometheus_" + Name + "_bucket{ handler = \"/\",le = \"" + getItemValue(i) + "\"} " + totalCount + "\n";
                }
                retValue += "prometheus_" + Name + "_sum{ handler = \"/\"} " + totalSum + "\n";
                retValue += "prometheus_" + Name + "_count{ handler = \"/\"} " + totalCount + "\n";
                return retValue;
                //                throw new NotImplementedException();
            }

            public override string getHeader(ref string lastName)
            {
                string retValue = "";
                if (this.Comment != "" && lastName != this.Name)
                {
                    retValue += "#  HELP " + this.Name + ":" + this.Comment + ".\n";
                    lastName = this.Name;
                    retValue += "# TYPE " + this.Name + " histogram" + "\n";
                }
                return retValue;
            }
        }

        public class MetricCount : Metric
        {
            //            public bool noAverage;
            public bool isSuccess;
            int count = 0;
            double sum = 0;
            public void Increment()
            {
                Interlocked.Increment(ref count);

            }
            public void Decrement()
            {
                Interlocked.Decrement(ref count);

            }
            public MetricCount(string Name,string Comment)
            {
                this.Name = Name;
                this.Comment = Comment;
            }

            public override Type typeMetric => Type.count;
            public void Add(DateTime time)
            {
                Interlocked.Increment(ref count);
                Add(ref sum, (DateTime.Now - time).TotalMilliseconds);

             /*   count++;
                sum += (DateTime.Now - time).TotalMilliseconds;*/
            }
            public void Add(DateTime time, long value)
            {
                Interlocked.Increment(ref count);
                Add(ref sum, value);
                //sum += value;
            }
            override public void Add(double value)
            {
                Interlocked.Increment(ref count);
                Add(ref sum, value);
            }
            public int getCount()
            {
                return count;

            }
            public double getAverage()
            {
                if (count == 0)
                    return 0;
                return sum / count;

            }

            public override string getHeader(ref string lastName)
            {
                string retValue = "";
                if (this.Comment != "" && lastName != this.Name)
                {
                    retValue += "#  HELP " + this.Name + ":" + this.Comment + "\n";
                    lastName = this.Name;
                    retValue += $"# TYPE {Name} counter\n";
                }
                return retValue;
            }

            public override string getBody()
            {
                string retValue = "";
                retValue += ("prometeus_" + Name + "{type=\"Count\",result=\"" +  "Success"  + "\"} " + getCount() + "\n");
                /*                if (!noAverage)
                                    retValue += (Name + "{type=\"Avg\",result=\"" + (isSuccess ? "Success" : "Error") + "\"} " + getAverage() + "\n");*/

                return retValue;
            }



        }

        public class MetricAbs : MetricCount
        {

            public MetricAbs(string Name,string Comment):base(Name,Comment)
            { }
            double val = 0;
            public override void Add(double value)
            {
                Add(ref val, value);
                /*Interlocked.CompareExchange(
                    ref val, value, val);*/
            }

            public override string getBody()
            {
                string retValue = "";
                retValue += ("prometeus_" + Name + "{type=\"Count\",result=\"" + (isSuccess ? "Success" : "Error") + "\"} " + val + "\n");
                /*                if (!noAverage)
                                    retValue += (Name + "{type=\"Avg\",result=\"" + (isSuccess ? "Success" : "Error") + "\"} " + getAverage() + "\n");*/

                return retValue;
            }

        }


    }

}
