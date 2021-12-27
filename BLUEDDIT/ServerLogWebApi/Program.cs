using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServerLogLogic;
using ServerLogLogicInterface;

namespace ServerLogWebApi
{
    public class Program
    {
        private static IServerLogLogic serverLog = new LogLogic();
        public static void Main(string[] args)
        {
            var taskShowMenu = Task.Run(() => MainAnterior());
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void MainAnterior()
        {
            Console.WriteLine("Bienvenido al ServerLog de BLUEDDIT!");
            using var channel = new ConnectionFactory() { HostName = "localhost" }.CreateConnection().CreateModel();

            channel.ExchangeDeclare(exchange: "direct_logs", type: "direct");
            var queueName = channel.QueueDeclare().QueueName;
            Console.WriteLine("Enter log severity levels [info] [warning] [error]:");
            var severities = Console.ReadLine()?.Split(" ");


            if (severities != null)
            {
                foreach (var severity in severities)
                {
                    channel.QueueBind(queue: queueName,
                        exchange: "direct_logs",
                        routingKey: severity);
                }
            }


            Console.WriteLine(" [*] Waiting for messages.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var log = JsonSerializer.Deserialize<Log>(message);
                serverLog.AddLog(log);
                var routingKey = ea.RoutingKey;
                Console.WriteLine(" [x] Received log level [{0}], " +
                    "message [{1}], user: [{2}]",
                    log.Level, log.Message, log.UserName, log.Date);
            };
            channel.BasicConsume(queue: queueName,
                autoAck: true,
                consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
