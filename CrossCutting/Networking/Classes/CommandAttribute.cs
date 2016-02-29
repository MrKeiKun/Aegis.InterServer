using System;

namespace Aegis.CrossCutting.Network.Classes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute
    {
        public CommandAttribute(PACKET_COMMAND command)
        {
            Initialize(command, null);
        }

        public CommandAttribute(PACKET_COMMAND command, int size)
        {
            Initialize(command, size);
        }

        private void Initialize(PACKET_COMMAND command, int? size)
        {
            Command = command;
            Size = size;
        }

        public PACKET_COMMAND Command { get; private set; }
        public int? Size { get; private set; }
    }
}