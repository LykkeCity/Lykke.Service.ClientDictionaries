using System.Threading.Tasks;

namespace Lykke.Service.ClientDictionaries.Core.Services
{
    public interface IClientDictionary
    {
        Task SetAsync(string clientId, string key, string value);
        Task<string> GetAsync(string clientId, string key);
        Task DeleteAsync(string clientId, string key);
    }
}
