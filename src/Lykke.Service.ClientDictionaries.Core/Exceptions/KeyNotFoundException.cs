using System;

namespace Lykke.Service.ClientDictionaries.Core.Exceptions
{
    public class KeyNotFoundException: Exception
    {
        public KeyNotFoundException()
        {
        }

        public KeyNotFoundException(string message)
            : base(message)
        {
        }

        public KeyNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public KeyNotFoundException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            
        }
    }
}
