using System.Text;
using Newtonsoft.Json;

namespace Busey.Event
{
    public static class EventHelpers
    {
        public static T ToEvent<T>(this byte[] message) where T : IEvent
        {
            var json = Encoding.UTF8.GetString(message);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static byte[] ToMessage(this IEvent @event)
        {
            var json = JsonConvert.SerializeObject(@event);
            return Encoding.UTF8.GetBytes(json);
        }
    }
}

