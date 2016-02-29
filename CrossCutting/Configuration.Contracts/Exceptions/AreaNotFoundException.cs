using System;
using System.Runtime.Serialization;

namespace Aegis.CrossCutting.Configuration.Contracts.Exceptions
{
    [Serializable]
    public class AreaNotFoundException : ConfigurationException
    {
        public AreaNotFoundException()
        {
        }

        public AreaNotFoundException(string message) : base(message)
        {
        }

        public AreaNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected AreaNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}