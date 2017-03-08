using System.Collections.Generic;

namespace Busey.Configuration
{
    public class QueueOptions
    {
        public string Queue { get; }
        public bool Durable { get; }
        public bool Exclusive { get; }
        public bool AutoDelete { get; }
        public IDictionary<string, object> Arguments { get; }

        public QueueOptions(string queue, bool durable = false, bool exclusive = false, bool autoDelete = false,
            IDictionary<string, object> args = null)
        {
            Queue = queue;
            Durable = durable;
            Exclusive = exclusive;
            AutoDelete = autoDelete;
            Arguments = args ?? new Dictionary<string, object>();
        }
    }
}