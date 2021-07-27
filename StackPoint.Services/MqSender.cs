using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using StackPoint.Domain.Services;

namespace StackPoint.Services
{
    public class MqSender : IMqSender
    {
        private readonly string _hostName;

        public MqSender(string hostName)
        {
            _hostName = hostName;
        }

        public bool Send<T>(string queueName, T entity) where T : class
        {
            try
            {
                var factory = new ConnectionFactory {HostName = _hostName};
                using var connection = factory.CreateConnection();

                using var channel = connection.CreateModel();
                channel.QueueDeclare(queueName, false, false, false, null);

                var json = JsonConvert.SerializeObject(entity);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(string.Empty, queueName, null, body);

                Console.WriteLine($"queue: \"{queueName}\"");
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
                return false;
            }

            return true;
        }
    }
}