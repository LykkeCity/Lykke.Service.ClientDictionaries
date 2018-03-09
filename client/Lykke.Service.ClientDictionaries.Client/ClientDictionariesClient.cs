using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.ClientDictionaries.AutorestClient;
using Lykke.Service.ClientDictionaries.AutorestClient.Models;
using Microsoft.Rest;

namespace Lykke.Service.ClientDictionaries.Client
{
    public class ClientDictionariesClient : IClientDictionariesClient, IDisposable
    {
        private readonly ILog _log;
        private ClientDictionariesAPI _apiClient;

        public ClientDictionariesClient(string serviceUrl, ILog log)
        {
            _log = log;
            _apiClient = new ClientDictionariesAPI(new Uri(serviceUrl));
        }

        public void Dispose()
        {
            if (_apiClient == null)
                return;
            _apiClient.Dispose();
            _apiClient = null;
        }
        
        private ResponseModel PrepareResponse(object serviceResponse)
        {
            if(!(serviceResponse is ResponseModel responseModel))
                throw new ArgumentException("Service response of unknown type");
            return responseModel;
        }

        public async Task<ResponseModel> GetAsync(string clientId, string key)
        {
            var response = await _apiClient.ApiDictionaryByClientIdByKeyGetWithHttpMessagesAsync(clientId, key);

            return PrepareResponse(response.Body);
        }

        public async Task<ResponseModel> SetAsync(string clientId, string key, string value)
        {
            var response = await _apiClient.ApiDictionaryByClientIdByKeyPostWithHttpMessagesAsync(clientId, key, new SetRequestModel {Data = value});

            return PrepareResponse(response.Body);
        }

        public async Task<ResponseModel> DeleteAsync(string clientId, string key)
        {
            var response = await _apiClient.ApiDictionaryByClientIdByKeyDeleteWithHttpMessagesAsync(clientId, key);

            return PrepareResponse(response.Body);
        }
    }
}
