using System.Threading.Tasks;
using Lykke.Service.ClientDictionaries.AutorestClient.Models;

namespace Lykke.Service.ClientDictionaries.Client
{
    public interface IClientDictionariesClient
    {
        Task<ResponseModel> GetAsync(string clientId, string key);
        Task<ResponseModel> SetAsync(string clientId, string key, string value);
        Task<ResponseModel> DeleteAsync(string clientId, string key);
    }
}
