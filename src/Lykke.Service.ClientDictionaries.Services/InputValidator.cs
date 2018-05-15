using System.Text;
using JetBrains.Annotations;
using Lykke.Service.ClientDictionaries.Core.Services;

namespace Lykke.Service.ClientDictionaries.Services
{
    [UsedImplicitly]
    public class InputValidator : IInputValidator
    {
        private readonly int _maxPayloadSizeBytes;

        public InputValidator(int maxPayloadSizeBytes)
        {
            _maxPayloadSizeBytes = maxPayloadSizeBytes;
        }
        
        public bool IsValidClientId(string clientId)
        {
            return !string.IsNullOrWhiteSpace(clientId);
        }

        public bool IsValidKey(string key)
        {
            return !string.IsNullOrWhiteSpace(key);
        }

        public bool IsValidPayload(string payload)
        {
            return payload != null;
        }

        public bool IsValidPayloadSize(string payload)
        {
            return Encoding.UTF8.GetBytes(payload).Length <= _maxPayloadSizeBytes;
        }
    }
}
