using System.Text;
using System.Text.Json;
using Domain;
using RabbitMQ.Client;

namespace Server_GPRC_MQ
{
    public class RabbitMqManagmentServer
    {
        private IModel channel;
        private static RabbitMqManagmentServer Instance = null;
        private static readonly object ObjectLock = new object();

        private RabbitMqManagmentServer()
        {
            CreateConnection();
        }

        private void CreateConnection()
        {

            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            this.channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "direct_logs",
                     type: "direct");

        }

        public static RabbitMqManagmentServer GetInstance()
        {
            if(RabbitMqManagmentServer.Instance == null)
            {
                lock (RabbitMqManagmentServer.ObjectLock)
                {
                    if(RabbitMqManagmentServer.Instance == null)
                    {
                        RabbitMqManagmentServer.Instance = new RabbitMqManagmentServer();
                    }
                }
            }
            return RabbitMqManagmentServer.Instance;
        }

        public void SendMessage(Log log)
        {
            var stringLog = JsonSerializer.Serialize(log);
            var routingKey = log.Level;
            var message = stringLog;
            var body = Encoding.UTF8.GetBytes(message);
            this.channel.BasicPublish(exchange: "direct_logs",
                            routingKey: routingKey,
                            basicProperties: null,
                            body: body);
        }
    }
 }
