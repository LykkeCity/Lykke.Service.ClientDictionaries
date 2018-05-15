namespace Lykke.Service.ClientDictionaries.Core.Services
{
    public interface IInputValidator
    {
        bool IsValidClientId(string clientId);
        bool IsValidKey(string key);
        bool IsValidPayload(string payload);
        bool IsValidPayloadSize(string payload);
    }
}
