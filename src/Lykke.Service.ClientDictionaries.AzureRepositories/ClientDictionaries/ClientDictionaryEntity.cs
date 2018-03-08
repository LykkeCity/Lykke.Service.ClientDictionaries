using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.ClientDictionaries.AzureRepositories.ClientDictionaries
{
    public class ClientDictionaryEntity : TableEntity
    {
        public static ClientDictionaryEntity Create(string clientId, string key, string value)
        {
            return new ClientDictionaryEntity
            {
                PartitionKey = clientId,
                RowKey = key,
                Payload = value
            };
        }
        
        public string Payload { set; get; }
    }
}
