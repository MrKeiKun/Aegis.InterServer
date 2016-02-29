using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Aegis.CrossCutting.Network.Packets;

namespace Aegis.CrossCutting.Network.Classes
{
    public static class Command
    {
        public static readonly Dictionary<Type, CommandAttribute> ProvidedMethods = new Dictionary<Type, CommandAttribute>();

        static Command()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof (PacketBase))))
            {
                var attributes = type.GetCustomAttributes(typeof (CommandAttribute), true);
                if (attributes.Length == 0)
                    continue;

                ProvidedMethods.Add(type, (CommandAttribute) attributes[0]);
            }
        }

        public static Type GetType(PACKET_COMMAND command)
        {
            return (from pair in ProvidedMethods let methodInfo = pair.Value where methodInfo.Command == command select pair.Key).FirstOrDefault();
        }

        public static bool HasMethod(PACKET_COMMAND command)
        {
            return ProvidedMethods.Count(x => x.Value.Command == command) > 0;
        }

        public static int? GetPacketInfo(PACKET_COMMAND command)
        {
            var packetInfo = ProvidedMethods.FirstOrDefault(x => x.Value.Command == command);
            return packetInfo.Value?.Size;
        }
    }
}