using System;
using System.Collections.Generic;
using Busey.Command;
using Busey.Configuration;
using Busey.Event;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Busey.Bus
{
    public class RabbitMqBus : IBus
    {
        private IConnection _connection;
        private IModel _channel;

        private readonly List<Tuple<string, Func<IModel, EventingBasicConsumer>>> _handlers;
        private readonly ConnectionFactory _factory;

        public RabbitMqBus(RabbitHost host)
        {
            var rabbitHost = host;
            _factory = new ConnectionFactory()
            {
                HostName = rabbitHost.Uri,
                UserName = rabbitHost.Username,
                Password = rabbitHost.Password
            };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _handlers = new List<Tuple<string, Func<IModel, EventingBasicConsumer>>>();
        }

        public void Start()
        {
            var handlers = _handlers;

            //call basicconsume on all the consumers that were registered in previous steps, if any
            foreach (var handler in handlers)
            {
                if (_connection.IsOpen)
                {
                    _channel.BasicConsume(handler.Item1, true, handler.Item2(_channel));
                }
            }
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        public void Send(ICommand command)
        {
            var queueName = command.GetType().ToQueue();
            using (_connection = _factory.CreateConnection())
            using (_channel = _connection.CreateModel())
            {
                InitQueue(queueName);

                var body = command.ToMessage();
                var properties = _channel.CreateBasicProperties();
                properties.Persistent = true;
                _channel.BasicPublish("",
                     queueName,
                     false,
                     properties,
                     body);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        public void Publish(IEvent @event)
        {
            var queueName = @event.GetType().ToQueue();
            using (_connection = _factory.CreateConnection())
            using (_channel = _connection.CreateModel())
            {
                InitQueue(queueName);

                var body = @event.ToMessage();
                var properties = _channel.CreateBasicProperties();
                properties.Persistent = true;
                _channel.BasicPublish("",
                     queueName,
                     false,
                     properties,
                     body);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handle"></param>
        /// <param name="prefetchCount"></param>
        public void RegisterCommandHandler<T>(Action<T> handle, ushort prefetchCount = 1) where T : ICommand
        {
            var queueName = typeof(T).ToQueue();
            InitQueue(queueName);
            InitQos(prefetchCount);
            Func<IModel, EventingBasicConsumer> commandConsumer = (channel) =>
            {
                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (sender, args) =>
                {
                    var body = args.Body.ToCommand<T>();
                    handle(body);
                    _channel.BasicAck(args.DeliveryTag, false);
                };
                return consumer;
            };
            _handlers.Add(new Tuple<string,Func<IModel, EventingBasicConsumer>>(queueName,commandConsumer));
        }

        public void RegisterEventHandler<T>(Action<T> handle, ushort prefetchCount = 1) where T : IEvent
        {
            var queueName = typeof(T).ToQueue();
            InitQueue(queueName);
            InitQos(prefetchCount);
            Func<IModel, EventingBasicConsumer> eventConsumer = (channel) =>
            {
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, args) =>
                {
                    var body = args.Body.ToEvent<T>();
                    handle(body);
                    channel.BasicAck(args.DeliveryTag, false);
                };
                return consumer;
            };
            _handlers.Add(new Tuple<string, Func<IModel, EventingBasicConsumer>>(queueName, eventConsumer));
        }

        private void InitQueue(string queueName)
        {
            var queueOptions = new QueueOptions(queueName, true);
            BusHelper.DeclareQueue(_channel, queueOptions);

        }

        private void InitQos(ushort prefetch)
        {
            var qosSettings = new QosSettings(prefetchCount: prefetch);
            _channel.BasicQos(qosSettings.PrefetchSize, qosSettings.PrefetchCount, qosSettings.Global);
        }
    }
}