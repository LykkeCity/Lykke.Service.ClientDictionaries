using System.Threading.Tasks;
using Lykke.Service.ClientDictionaries.Client.Models;

namespace Lykke.Service.ClientDictionaries.Client
{
    public interface IClientDictionariesClient
    {
        Task<DictResponseModel> GetAsync(string clientId, string key);
        Task<DictResponseModel> SetAsync(string clientId, string key, string value);
        Task<DictResponseModel> DeleteAsync(string clientId, string key);
    }
}
