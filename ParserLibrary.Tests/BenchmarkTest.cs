using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLibrary.Tests
{
    public class TheEasiestBenchmark
    {
        [Benchmark(Description = "Summ100")]
        public int Test100()
        {
            return Enumerable.Range(1, 100).Sum();
        }

        [Benchmark(Description = "Summ200")]
        public int Test200()
        {
            return Enumerable.Range(1, 200).Sum();
        }
    }

    public class TheMetricBenchmark
    {
        Metrics.MetricCounters[] metrics;
        int kolLabel = 2;
        int kolMetric = 10;
        List<string[]> labels = new List<string[]>() { new string[] { "One", "Too", "Tree" }, new string[] { "One", "Too", "Tree", "Four" }, new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" } };
        Random rnd= new Random();
        public TheMetricBenchmark()
        {
            metrics=new Metrics.MetricCounters[kolMetric];

        }
        [Benchmark(Description = "TestMetric")]
        public int Test100()
        {
            foreach (var met in metrics)
            {
                int index1 = rnd.Next(100);
                met.AddCount(string.Join('/', labels.Select((ii, index) => ii[index1 % ii.Length])));
            }
            return 0;
        }

    }

    public class UnitTest1
    {
        [Test]
        
        public void TestMethod1()
        {
            BenchmarkRunner.Run<TheMetricBenchmark>();
//            BenchmarkRunner.Run<TheEasiestBenchmark>();
        }
    }
}

