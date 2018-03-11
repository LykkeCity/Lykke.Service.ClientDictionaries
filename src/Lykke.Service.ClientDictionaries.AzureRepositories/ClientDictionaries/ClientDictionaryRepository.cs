using System.Threading.Tasks;
using AzureStorage;
using JetBrains.Annotations;
using Lykke.Service.ClientDictionaries.Core.Exceptions;
using Lykke.Service.ClientDictionaries.Core.Services;

namespace Lykke.Service.ClientDictionaries.AzureRepositories.ClientDictionaries
{
    [UsedImplicitly]
    public class ClientDictionaryRepository : IClientDictionary
    {
        public const string TableName = "ClientDictionaries";
        private readonly INoSQLTableStorage<ClientDictionaryEntity> _tableStorage;
        
        public ClientDictionaryRepository(INoSQLTableStorage<ClientDictionaryEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }
        
        public Task SetAsync(string clientId, string key, string value)
        {
            return _tableStorage.InsertOrMergeAsync(ClientDictionaryEntity.Create(clientId, key, value));
        }

        public async Task<string> GetAsync(string clientId, string key)
        {
            var entity = await _tableStorage.GetDataAsync(clientId, key);

            if (entity == null)
                throw new KeyNotFoundException();

            return entity.Payload;
        }

        public async Task DeleteAsync(string clientId, string key)
        {
            var entity = await _tableStorage.GetDataAsync(clientId, key);

            if (entity == null)
                throw new KeyNotFoundException();

            await _tableStorage.DeleteAsync(clientId, key);
        }
    }
}
