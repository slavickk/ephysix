using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ParserLibrary;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Json;
using Logger = ParserLibrary.Logger;

namespace TestJsonRazbor
{
    public partial class FormTestPipeline : Form
    {
        public FormTestPipeline()
        {
            InitializeComponent();
        }
        int lastIndex = -1;
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                int index = listView1.SelectedIndices[0];
                if (index >= 0)
                {
                    var step = pip.steps[index];
                    if (step.receiver == null)
                    {
                        checkBoxMockReceiver.Enabled = checkBoxMockReceiver.Checked = false;
                    }
                    else
                    {
                        checkBoxMockReceiver.Enabled = true;
                        checkBoxMockReceiver.Checked = step.receiver.MocMode;
                    }
                    if (step.sender == null)
                    {
                        checkBoxSender.Enabled = checkBoxSender.Checked = false;
                    }
                    else
                    {
                        checkBoxSender.Enabled = true;
                        checkBoxSender.Checked = step.sender.MocMode;
                    }
                }
            }
        }
        public Pipeline pip;

        class CustomDateFormatter : IFormatProvider
        {
            readonly IFormatProvider basedOn;
            readonly string shortDatePattern;
            public CustomDateFormatter(string shortDatePattern, IFormatProvider basedOn)
            {
                this.shortDatePattern = shortDatePattern;
                this.basedOn = basedOn;
            }
            public object GetFormat(Type formatType)
            {
                if (formatType == typeof(DateTimeFormatInfo))
                {
                    var basedOnFormatInfo = (DateTimeFormatInfo)basedOn.GetFormat(formatType);
                    var dateFormatInfo = (DateTimeFormatInfo)basedOnFormatInfo.Clone();
                    dateFormatInfo.ShortDatePattern = this.shortDatePattern;
                    return dateFormatInfo;
                }
                return this.basedOn.GetFormat(formatType);
            }
        }


        private void FormTestPipeline_Load(object sender, EventArgs e)
        {
            foreach (Step step in pip.steps)
            {
                listView1.Items.Add(new ListViewItem(new string[] { step.IDStep, (step.receiver == null) ? "" : step.receiver.MocMode.ToString(), (step.sender == null) ? "" : step.sender.MocMode.ToString() }));
            }
            listView1.SelectedIndices.Add(0);


            var formatter = new CustomDateFormatter("dd-MMM-yyyy", new CultureInfo("en-AU"));
            

            Log.Logger= new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(formatProvider: new CultureInfo("en-AU")) // Console 1
                .WriteTo.Console(formatProvider: formatter)                // Console 2
                    .WriteTo.Sink(TbsLoggerSink.LoggerSink, Serilog.Events.LogEventLevel.Debug)
                    .CreateLogger();
            TbsLoggerSink.LoggerSink.NewLogHandler += LogHandler;

        }

        private void LogHandler(object sender, EventArgs e)
        {
            this.Invoke(LogHandler1, new object[] { e as LogEventArgs });
        }


        private void LogHandler1(LogEventArgs e)
        {

            var log = ((LogEventArgs)e).Log;
            listBox1.Items.Insert(0, $"{log.Timestamp.DateTime.ToString("HH:mm:ss")}:{log.RenderMessage()} {((log.Exception != null)?$"Exc:{log.Exception}":"")} \r\n{this.logTextBox.Text}");
            //this.logTextBox.Text = $"{log.Timestamp.DateTime.ToString("HH:mm:ss")}: {log.RenderMessage()}\r\n{this.logTextBox.Text}";
        }
        private void checkBoxMockReceiver_CheckedChanged(object sender, EventArgs e)
        {
            int index = listView1.SelectedIndices[0];
            if (index >= 0 && pip.steps[index].receiver != null)
            {
                pip.steps[index].receiver.MocMode = checkBoxMockReceiver.Checked;
                listView1.Items[index].SubItems[1].Text = pip.steps[index].receiver.MocMode.ToString();
            }
        }

        private void checkBoxSender_CheckedChanged(object sender, EventArgs e)
        {
            int index = listView1.SelectedIndices[0];
            if (index >= 0 && pip.steps[index].sender!= null)
            {
                pip.steps[index].sender.MocMode = checkBoxSender.Checked;
                listView1.Items[index].SubItems[2].Text = pip.steps[index].sender.MocMode.ToString();
            }

        }
        CancellationTokenSource Canceller = new CancellationTokenSource() ;
        Task taskA = null;

        double lastPerfValue=0;
        int lastPerfCount = 0;
        DateTime lastPerfTime=DateTime.Now;

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            if(taskA!= null)
            {
                Canceller.Cancel();
            }
            while (taskA != null) Thread.Sleep(100);
            lastPerfTime = DateTime.Now;
            lastPerfCount = HTTPReceiver.KestrelServer.metricCountExecuted.getCount();
            lastPerfValue = HTTPReceiver.KestrelServer.metricTimeExecuted.sum;
            /*  cancellationToken.IsCancellationRequested = true;
              if(cancellationToken.CanBeCanceled)
              {
                  cancellationToken.ThrowIfCancellationRequested();
              }*/
            logTextBox.Text = "";
            pip.debugMode = checkBoxDebug.Checked;
            taskA = Task.Run(async () =>
            {
                try
                {
                        // specify this thread's Abort() as the cancel delegate
                        using (Canceller.Token.Register(Thread.CurrentThread.Abort))
                        {
                            await pip.run();
                        }
//                    await pip.run();
                }
                catch (ThreadAbortException)
                {
                    Logger.log("execution cancelled on request", LogEventLevel.Error, "any");
                    taskA = null;
                    //return false;
                }
                catch (Exception e77)
                {
                    Logger.log("execution terminated on error {err}",LogEventLevel.Error,"any",e77.ToString());
                   // MessageBox.Show(e77.ToString());
                }

                if(pip.lastExecutedEl!= null)
                {
                    var ex=pip.lastExecutedEl;
                    while (ex.ancestor != null)
                        ex = ex.ancestor;

                    if (checkBoxSetMoc.Checked)
                        pip.tempMocData = ex.toJSON();
                    else
                        pip.tempMocData = "";
/*                    foreach (var step in pip.steps)
                        if (step.receiver != null)
                            step.receiver.MocBody = ex.toJSON();*/
                }
                Logger.log("Execution ended!");
                taskA = null;
            },Canceller.Token);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int index = listView1.SelectedIndices[0];
            if (index >= 0 && pip.steps[index].receiver != null)
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    pip.steps[index].receiver.MocFile = openFileDialog1.FileName;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int index = listView1.SelectedIndices[0];
            if (index >= 0 && pip.steps[index].sender != null)
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    pip.steps[index].sender.MocFile = openFileDialog1.FileName;
                }
            }

        }
        TimeSpan periodMeasure = new TimeSpan(0, 0, 10);
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now - lastPerfTime >= periodMeasure)
            {
                if (HTTPReceiver.KestrelServer.metricCountExecuted.getCount() - lastPerfCount > 0)
                    labelPerf.Text = $"{(int)(1000.0/((HTTPReceiver.KestrelServer.metricTimeExecuted.sum - lastPerfValue) / (HTTPReceiver.KestrelServer.metricCountExecuted.getCount() - lastPerfCount)))} in sec";
                lastPerfTime = DateTime.Now;
                lastPerfCount = HTTPReceiver.KestrelServer.metricCountExecuted.getCount();
                lastPerfValue = HTTPReceiver.KestrelServer.metricTimeExecuted.sum;
            }
            listBox2.Items.Clear();
            foreach(var metric in Metrics.metric.allMetrics)
            listBox2.Items.Add(metric);
            labelRexRequest.Text = $"Opened rex:{StreamSender.countOpenRexRequest}";

            labelCount.Text = $"Executed:{HTTPReceiver.KestrelServer.CountExecuted}";
            labelOpened.Text = $"Open:{HTTPReceiver.KestrelServer.CountOpened}";
        }
    }
}
