using System.Threading.Tasks;

namespace Lykke.Service.ClientDictionaries.Core.Services
{
    public interface IKeysMapper
    {
        Task<string> GetKey(string clientId, string key);
    }
}
