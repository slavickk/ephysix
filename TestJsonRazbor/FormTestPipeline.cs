using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ParserLibrary;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Json;

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
            this.logTextBox.Text = $"{log.Timestamp.DateTime.ToString("HH:mm:ss")}: {log.RenderMessage()}\r\n{this.logTextBox.Text}";
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

        private void button1_Click(object sender, EventArgs e)
        {
            logTextBox.Text = "";
            Task taskA = Task.Run(async () =>
            {
                try
                {
                    await pip.run();
                }
                catch(Exception e77)
                {
                    MessageBox.Show(e77.ToString());
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
                MessageBox.Show("Execution ended!");
            });
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
    }
}
