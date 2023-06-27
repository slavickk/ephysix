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

        [SetUp]
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
