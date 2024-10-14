using ParserLibrary;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using UniElLib;
using YamlDotNet.Serialization;

namespace WinFormsSettingSender
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                textBoxHeaderName.Text = listView1.SelectedItems[0].SubItems[0].Text;
                textBoxHeaderValue.Text = listView1.SelectedItems[0].SubItems[1].Text;
            }
        }



        private void buttonNewHeader_Click(object sender, EventArgs e)
        {
            textBoxHeaderName.Text = textBoxHeaderValue.Text = "";
            listView1.SelectedIndices.Clear();
        }
        public class SenderItem
        {
            public string body { get; set; }
            public HTTPSender sender { get; set; }
            public string answer { get; set; }
            public string SwaggerUrl { get; set; }
            public string SwaggerJsonUrl { get; set; }
            public override string ToString()
            {
                return sender.description;
            }
        }

        List<SenderItem> allSenders = new List<SenderItem>();
        private void buttonSaveHeader_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxHeaderName.Text) && !string.IsNullOrEmpty(textBoxHeaderName.Text))
            {
                if (listView1.SelectedItems.Count == 0)
                    listView1.Items.Add(new ListViewItem(new string[] { textBoxHeaderName.Text, textBoxHeaderValue.Text }));
                else
                {
                    listView1.SelectedItems[0].SubItems[0].Text = textBoxHeaderName.Text;
                    listView1.SelectedItems[0].SubItems[1].Text = textBoxHeaderValue.Text;
                }
            }

        }

        private void buttonDelHeader_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
                listView1.Items.Remove(listView1.SelectedItems[0]);
        }

        private async void button7_Click(object sender, EventArgs e)
        {
            var sender1 = createSender();
            /*foreach (ListViewItem item in listView1.Items)
            {

                sender1.sender.headers.Add(item.SubItems[0].Text, item.SubItems[1].Text);
            }*/
            try
            {
                var context = new UniElLib.ContextItem() { context = new HTTPReceiver.SyncroItem() };
                List<AbstrParser.UniEl> list = new List<AbstrParser.UniEl>();
                list.Clear();
                AbstrParser.UniEl rootEl = AbstrParser.CreateNode(null, list, "Item");
                var line = textBoxBody.Text;
                if (line != "")
                {
                    foreach (var pars in AbstrParser.availParser)
                        if (pars.canRazbor("", line, rootEl, list))
                            break;
                }


                textBoxAnswer.Text = await sender1.sender.sendInternal(rootEl, context);//.internSend(textBoxBody.Text, context);
                if ((context.context as HTTPReceiver.SyncroItem).isError)
                    MessageBox.Show($"Return {(context.context as HTTPReceiver.SyncroItem).HTTPStatusCode} {(context.context as HTTPReceiver.SyncroItem).HTTPErrorJsonText}");
                else
                    MessageBox.Show("Success!!");
            }
            catch (Exception e77)
            {
                MessageBox.Show(e77.ToString());
            }
        }

        private SenderItem createSender()
        {
            HTTPSender sender1 = new HTTPSender();
            sender1.method = Enum.GetValues<HTTPSender.Method>()[Enum.GetNames<HTTPSender.Method>().ToList().IndexOf(comboBoxMethod.SelectedItem.ToString())];
            sender1.url = textBoxUrl.Text;
            sender1.timeoutSendInMilliseconds = (double)numericUpDownTimeout.Value;
            sender1.allowAutoRedirect = true;
            sender1.headers = new Dictionary<string, string>();
            sender1.ResponseType = textBoxContentType.Text;
            sender1.description = textBoxDescription.Text;
            sender1.headers.Clear();
            sender1.MocBody=textBoxAnswer.Text;
            sender1.template=textBoxBody.Text;
           
            foreach (ListViewItem item in listView1.Items)
                sender1.headers.Add(item.SubItems[0].Text, item.SubItems[1].Text);
            return new SenderItem() {SwaggerUrl=textBoxSwaggerPath.Text,SwaggerJsonUrl=textBoxSwaggerJsonUrl.Text, sender = sender1, body = textBoxBody.Text, answer = textBoxAnswer.Text };
        }
        void RefreshSenders()
        {
            comboBoxChooseSender.Items.Clear();
            comboBoxChooseSender.Items.AddRange(allSenders.ToArray());
        }

        string fileName = "Data/AllSenders.yml";
        private void buttonSaveSender_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxDescription.Text))
                MessageBox.Show("Fill description!");

            //  createSender();
            if (comboBoxChooseSender.SelectedItem != null)
                allSenders[comboBoxChooseSender.SelectedIndex] = createSender();
            else
                allSenders.Add(createSender());
            SaveAllSenders();
            if (!this.Text.Contains(fileName))
                this.Text += " Saved on " + Path.GetFullPath(fileName);
            RefreshSenders();

        }

        private void SaveAllSenders()
        {
            ISerializer serializer = Pipeline.getSerializer(Assembly.GetAssembly(typeof(Pipeline)));


            using (StreamWriter sw = new StreamWriter(fileName))
            {
                serializer.Serialize(sw, allSenders);
            }


        }

        private void comboBoxChooseSender_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxMethod.SelectedIndex = 0;
            if (File.Exists(fileName))
            {
                var deser = Pipeline.GetDeserializer(Assembly.GetAssembly(typeof(Pipeline)));
                using (StreamReader sr = new StreamReader(fileName))
                {

                    var line = sr.ReadToEnd();
                    allSenders = deser.Deserialize<List<SenderItem>>(line);
                    //allSenders = JsonSerializer.Deserialize<List<SenderItem>>(line);
                    RefreshSenders();
                }
            }
        }

        private void buttonDelSender_Click(object sender, EventArgs e)
        {
            if (comboBoxChooseSender.SelectedIndex >= 0)
            {
                allSenders.RemoveAt(comboBoxChooseSender.SelectedIndex);
                comboBoxChooseSender.SelectedIndex = -1;

                SaveAllSenders();
                RefreshSenders();

            }
        }

        private void buttonNewSender_Click(object sender, EventArgs e)
        {
            comboBoxChooseSender.SelectedIndex = -1;
        }

        private void comboBoxChooseSender_SelectedIndexChanged_1(object sender, EventArgs e)
        {
        }

        private void comboBoxChooseSender_SelectedIndexChanged_2(object sender, EventArgs e)
        {
            textBoxHeaderName.Text = textBoxHeaderValue.Text = "";
            if (comboBoxChooseSender.SelectedIndex < 0)
            {
                numericUpDownTimeout.Value = 5000;
                comboBoxMethod.SelectedIndex = 0;
                textBoxAnswer.Text = textBoxBody.Text = textBoxDescription.Text = textBoxUrl.Text = textBoxSwaggerPath.Text=textBoxSwaggerJsonUrl.Text="";
                listView1.Items.Clear();
            }
            else
            {
                SenderItem sender1 = comboBoxChooseSender.SelectedItem as SenderItem;
                comboBoxMethod.SelectedIndex = Enum.GetNames(typeof(HTTPSender.Method)).ToList().IndexOf(Enum.GetName<HTTPSender.Method>(sender1.sender.method));
                textBoxAnswer.Text = sender1.answer;
                textBoxBody.Text = sender1.body;
                numericUpDownTimeout.Value = (decimal)sender1.sender.timeoutSendInMilliseconds;
                listView1.Items.Clear();
                foreach (var item in sender1.sender.headers)
                {
                    listView1.Items.Add(new ListViewItem(new string[] { item.Key, item.Value }));
                }
                textBoxDescription.Text = sender1.sender.description;
                textBoxContentType.Text = sender1.sender.ResponseType;
                textBoxUrl.Text = sender1.sender.url;
                textBoxSwaggerPath.Text=sender1.SwaggerUrl;
                textBoxSwaggerJsonUrl.Text=sender1.SwaggerJsonUrl;
                // textBoxBody.Text=
            }


        }

        private void textBoxSwaggerPath_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(!string.IsNullOrEmpty(textBoxSwaggerPath.Text))
            {
                var processes = Process.GetProcessesByName("Chrome");
                var path = processes.FirstOrDefault()?.MainModule?.FileName;

                System.Diagnostics.Process.Start(path, textBoxSwaggerPath.Text);

            }
        }
    }
}
