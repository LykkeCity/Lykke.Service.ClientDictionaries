using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.ClientDictionaries.AzureRepositories.ClientKeysToBlobKeys
{
    public class ClientKeysToBlobKeyEntity : TableEntity
    {
        public static ClientKeysToBlobKeyEntity Create(string clientId, string key, string blobKey)
        {
            return new ClientKeysToBlobKeyEntity
            {
                PartitionKey = GetPartitionKey(clientId),
                RowKey = GetRowKey(key),
                BlobKey = blobKey
            };
        }

        public static string GetPartitionKey(string clientId)
        {
            return clientId;
        }

        public static string GetRowKey(string key)
        {
            return key;
        }
        
        public string BlobKey { set; get; }
    }
}
