using System;

namespace Aegis.CrossCutting.Network.Classes
{
    public class BufferException : Exception
    {
        public string Pos { get; set; }
        public byte[] Packet { get; set; }

        public BufferException(string pos, string message, byte[] packet) : base(message)
        {
            Pos = pos;
        }
    }
}