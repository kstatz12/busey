using System.Text;
using Newtonsoft.Json;

namespace Busey.Command
{
    public static class CommandHelpers
    {
        public static T ToCommand<T>(this byte[] message) where T : ICommand
        {
            var json = Encoding.UTF8.GetString(message);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static byte[] ToMessage(this ICommand command)
        {
            var json = JsonConvert.SerializeObject(command);
            return Encoding.UTF8.GetBytes(json);
        }
    }
}