using System;
using System.Runtime.Serialization;

namespace Aegis.CrossCutting.Configuration.Contracts.Exceptions
{
    [Serializable]
    public class KeyNotFoundException : ConfigurationException
    {
        public KeyNotFoundException()
        {
        }

        public KeyNotFoundException(string message) : base(message)
        {
        }

        public KeyNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected KeyNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}