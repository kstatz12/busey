using System;
using System.Collections.Generic;
using Busey.Command;
using Busey.Event;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Busey.Bus
{
    public interface IBus : IDisposable
    {
        void Start();
        void Send(ICommand command);
        void Publish(IEvent @event);
        void RegisterCommandHandler<T>(Action<T> handler, ushort prefetchCount = 1) where T : ICommand;
        void RegisterEventHandler<T>(Action<T> handler, ushort prefetchCount = 1) where T : IEvent; 
    }
}