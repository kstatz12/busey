namespace Busey.Configuration
{
    public class QosSettings
    {
        public ushort PrefetchSize { get; }
        public ushort PrefetchCount { get; }
        public bool Global { get; }

        public QosSettings(ushort prefetchSize = 0, ushort prefetchCount = 1, bool global = false)
        {
            PrefetchSize = prefetchSize;
            PrefetchCount = prefetchCount;
            Global = global;
        }
    }
}