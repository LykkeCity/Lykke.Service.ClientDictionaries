using System;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.ClientDictionaries.Core.Services;

namespace Lykke.Service.ClientDictionaries.AzureRepositories.ClientKeysToBlobKeys
{
    public class ClientKeysToBlobKeys : IKeysMapper
    {
        public const string TableName = "ClientDictionariesKeysMappings";
        private readonly INoSQLTableStorage<ClientKeysToBlobKeyEntity> _tableStorage;

        public ClientKeysToBlobKeys(INoSQLTableStorage<ClientKeysToBlobKeyEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }
        
        public async Task<string> GetKey(string clientId, string key)
        {
            var entity = await _tableStorage.GetOrInsertAsync(
                ClientKeysToBlobKeyEntity.GetPartitionKey(clientId),
                ClientKeysToBlobKeyEntity.GetRowKey(key),
                () => ClientKeysToBlobKeyEntity.Create(clientId, key, Guid.NewGuid().ToString()));

            return entity.BlobKey;
        }
    }
}
