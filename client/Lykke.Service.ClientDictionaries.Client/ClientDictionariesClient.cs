using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.ClientDictionaries.AutorestClient;
using Lykke.Service.ClientDictionaries.AutorestClient.Models;
using Lykke.Service.ClientDictionaries.Client.Models;
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
        
        private DictResponseModel PrepareResponse(object serviceResponse)
        {
            var error = serviceResponse as ErrorResponse;
            var result = serviceResponse as RecordPayloadModel;

            if (error != null)
            {
                return new DictResponseModel
                {
                    Error = new ErrorModel
                    {
                        Message = error.ErrorMessage
                    }
                };
            }

            if (result != null)
            {
                return new DictResponseModel
                {
                    Value = result.Payload
                };
            }

            return new DictResponseModel();
        }

        public async Task<DictResponseModel> GetAsync(string clientId, string key)
        {
            var response = await _apiClient.ApiDictionaryByClientIdByKeyGetWithHttpMessagesAsync(clientId, key);

            return PrepareResponse(response.Body);
        }

        public async Task<DictResponseModel> SetAsync(string clientId, string key, string value)
        {
            var response = await _apiClient.ApiDictionaryByClientIdByKeyPostWithHttpMessagesAsync(clientId, key, new RecordPayloadModel {Payload = value});

            return PrepareResponse(response.Body);
        }

        public async Task<DictResponseModel> DeleteAsync(string clientId, string key)
        {
            var response = await _apiClient.ApiDictionaryByClientIdByKeyDeleteWithHttpMessagesAsync(clientId, key);

            return PrepareResponse(response.Body);
        }
    }
}
