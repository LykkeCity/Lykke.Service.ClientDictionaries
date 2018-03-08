namespace Lykke.Service.ClientDictionaries
{
    public static class CommonValidators
    {
        public static bool IsValidClientId(string clientId)
        {
            return !string.IsNullOrWhiteSpace(clientId);
        }

        public static bool IsValidKey(string key)
        {
            return !string.IsNullOrWhiteSpace(key);
        }

        public static bool IsValidPayload(string payload)
        {
            return !string.IsNullOrWhiteSpace(payload);
        }
    }
}
