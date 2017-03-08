using System.Text;
using Busey.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Busey.Bus
{
    public static class BusHelper
    {
        public static void DeclareExchange(IModel channel, ExchangeOptions options)
        {
            channel.ExchangeDeclare(options.Exchange, options.Type.ToString(), options.Durable, options.AutoDelete, options.Arguments);
        }

        public static void DeclareQueue(IModel channel, QueueOptions options)
        {
            channel.QueueDeclare(options.Queue, options.Durable, options.Exclusive, options.AutoDelete,
                options.Arguments);
        }

        public static byte[] CreateMessageBody<T>(T obj)
        {
            var message = JsonConvert.SerializeObject(obj);
            var body = Encoding.UTF8.GetBytes(message);
            return body;
        }
    }
}