using Busey.Bus;
using Busey.Command;
using Busey.Event;

namespace Busey.Configuration
{
    public class BusBootstrapper
    {
        private IBus _bus;
        private RabbitHost _hostSettings;

        public BusBootstrapper Init(RabbitHost host)
        {
            _hostSettings = host;
            _bus = new RabbitMqBus(_hostSettings);
            _bus.Start();
            return this;
        }

        public BusBootstrapper WithCommandHandler<T>(ICommandHandler<T> handler) where T : ICommand
        {
            _bus.RegisterCommandHandler<T>(handler.Handle);
            return this;
        }

        public BusBootstrapper WithEventHandler<T>(IEventHandler<T> handler) where T : IEvent
        {
            _bus.RegisterEventHandler<T>(handler.Handle);
            return this;
        }

        public BusBootstrapper Stop()
        {
            _bus.Dispose();
            return this;
        }

        public IBus GetBus()
        {
            return _bus;
        }
    }
}