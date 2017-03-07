using System.Collections.Generic;

namespace Busey.Configuration
{
    public class ExchangeOptions
    {
        public string Exchange { get; }
        public ExchangeType Type { get; }
        public bool Durable { get; }
        public bool AutoDelete { get; }
        public IDictionary<string, object> Arguments { get; }

        public ExchangeOptions(string exchange, ExchangeType type, bool durable = false, bool autoDelete = false,
            IDictionary<string, object> args = null)
        {
            Exchange = exchange;
            Type = type;
            Durable = durable;
            AutoDelete = autoDelete;
            Arguments = args ?? new Dictionary<string, object>();
        }
    }
}