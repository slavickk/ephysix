using JamaaTech.Smpp.Net.Client;
using NUnit.Framework;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Tasks;


namespace ParserLibrary.Tests
{
    [TestFixture]
    public class RabbitMQReceiverTests
    {
        private const string Hostname = "localhost";
        private const int Port = 5672;
        private const string Username = "guest";
        private const string Password = "guest";
        private const string QueueName = "test_queue";

        private RabbitMQHelper receiver;
        private ConnectionFactory factory;
        private IConnection connection;
        private IModel channel;
        private EventingBasicConsumer consumer;

       // [SetUp]
        public void Setup()
        {
            receiver = new RabbitMQHelper(Hostname, Port, Username, Password, QueueName);
            factory = new ConnectionFactory
            {
                HostName = Hostname,
                Port = Port,
                UserName = Username,
                Password = Password
            };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        [TearDown]
        public void TearDown()
        {
            receiver.StopReceiving();
            channel.Close();
            connection.Close();
        }
        [Test]
        public void testSmpp()
        {
            SmppClient client = new SmppClient();
            System.Threading.Thread.Sleep(3000);
            SmppConnectionProperties properties = client.Properties;
            properties.SystemID = "other";
            properties.Password = "other";
            properties.Port = 7000; //IP port to use
            properties.Host = "10.77.206.210"; //SMSC host name or IP Address
            properties.SystemType = "mysystemtype";
            properties.DefaultServiceType = "mydefaultservicetype";

            //Resume a lost connection after 30 seconds
            client.AutoReconnectDelay = 3000;

            //Send Enquire Link PDU every 15 seconds
            client.KeepAliveInterval = 15000;

            //Start smpp client
            client.Start();
            System.Threading.Thread.Sleep(3000);

            //client.ConnectionStateChanged += client_ConnectionStateChanged;

            TextMessage msg = new TextMessage();

            msg.DestinationAddress = "+79222354098"; //Receipient number
            msg.SourceAddress = "300"; //Originating number
            msg.Text = "Hello, this is my test message!";
            msg.RegisterDeliveryNotification = true; //I want delivery notification for this message

            //SmppClient client = GetSmppClient();
            client.SendMessage(msg);
        }

        [Test]
        public async Task StartReceiving_ReceiveMessage_MessageReceivedSuccessfully()
        {
            // Arrange
            const string expectedMessage = "Test message";
            string receivedMessage = null;
//            receiver.StartReceiving();

            consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                receivedMessage = Encoding.UTF8.GetString(body);
            };
            channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);
            receiver.StartReceiving();
            await Task.Delay(1000); // Wait for the message to be received

            // Act
            receiver.SendMessage(expectedMessage);
            await Task.Delay(1000); // Wait for the message to be received

            // Assert
            Assert.AreEqual(expectedMessage, receivedMessage);
        }
    }

}
