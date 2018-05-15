using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AzureStorage;
using JetBrains.Annotations;
using Lykke.Service.ClientDictionaries.Core.Exceptions;
using Lykke.Service.ClientDictionaries.Core.Services;

namespace Lykke.Service.ClientDictionaries.AzureRepositories.ClientDictionaryBlob
{
    [UsedImplicitly]
    public class ClientDictionaryBlob : IClientDictionary
    {
        private const string BlobContainer = "client-dictionaries";
        private readonly IBlobStorage _storage;
        private readonly IKeysMapper _keysMapper;
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _locks;

        public ClientDictionaryBlob(IBlobStorage storage, IKeysMapper keysMapper)
        {
            _storage = storage;
            _keysMapper = keysMapper;
            _locks = new ConcurrentDictionary<string, SemaphoreSlim>();
        }
        
        public async Task SetAsync(string clientId, string key, string value)
        {
            await _storage.SaveBlobAsync(
                BlobContainer,
                await MakeBlobKey(clientId, key),
                Encoding.UTF8.GetBytes(value));
        }

        public async Task<string> GetAsync(string clientId, string key)
        {
            var semaphore = new SemaphoreSlim(1, 1);
            
            key = await MakeBlobKey(clientId, key);

            semaphore = _locks.GetOrAdd(key, semaphore);

            await semaphore.WaitAsync();

            try
            {
                var keyExists = await _storage.HasBlobAsync(BlobContainer, key);

                if (keyExists)
                    return await _storage.GetAsTextAsync(
                        BlobContainer,
                        key);
                
                throw new KeyNotFoundException();
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task DeleteAsync(string clientId, string key)
        {
            var semaphore = new SemaphoreSlim(1, 1);
            
            key = await MakeBlobKey(clientId, key);

            semaphore = _locks.GetOrAdd(key, semaphore);

            await semaphore.WaitAsync();

            try
            {
                var keyExists = await _storage.HasBlobAsync(BlobContainer, key);

                if (keyExists)
                    await _storage.DelBlobAsync(
                        BlobContainer,
                        key);
                
                throw new KeyNotFoundException();
            }
            finally
            {
                semaphore.Release();
            }
        }

        private async Task<string> MakeBlobKey(string clientId, string key)
        {
            key = await _keysMapper.GetKey(clientId, key);
            
            return $"{clientId}/{key}";
        }
    }
}
