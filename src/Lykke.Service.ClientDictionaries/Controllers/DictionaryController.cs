using System.Net;
using System.Threading.Tasks;
using Lykke.Service.ClientDictionaries.Core.Exceptions;
using Lykke.Service.ClientDictionaries.Core.Services;
using Lykke.Service.ClientDictionaries.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.ClientDictionaries.Controllers
{
    [Route("api/[controller]")]
    public class DictionaryController : Controller
    {
        private const string InvalidClientIdMessage = "Invalid client Id value provided";
        private const string InvalidKeyMessage = "Invalid key value provided";
        private const string InvalidPayloadMessage = "Invalid value provided";
        
        private readonly IClientDictionary _clientDictionary;
        
        public DictionaryController(IClientDictionary clientDictionary)
        {
            _clientDictionary = clientDictionary;
        }
        
        [HttpGet("{clientId}/{key}")]
        [ProducesResponseType(typeof(ResponseModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseModel), (int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ResponseModel), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get(string clientId, string key)
        {
            try
            {
                if (!CommonValidators.IsValidClientId(clientId))
                    return BadRequest(ResponseModel.CreateWithError(ErrorType.Validation, InvalidClientIdMessage));
                
                if (!CommonValidators.IsValidKey(key))
                    return BadRequest(ResponseModel.CreateWithError(ErrorType.Validation, InvalidKeyMessage));
                
                var value = await _clientDictionary.GetAsync(clientId, key);
                
                return Ok(ResponseModel.CreateWithData(value));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ResponseModel.CreateWithError(ErrorType.NotFound, null));
            }
        }
        
        [HttpPost("{clientId}/{key}")]
        [ProducesResponseType(typeof(ResponseModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseModel), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post(string clientId, string key, [FromBody]SetRequestModel model)
        {
            if (!CommonValidators.IsValidClientId(clientId))
                return BadRequest(ResponseModel.CreateWithError(ErrorType.Validation, InvalidClientIdMessage));
                
            if (!CommonValidators.IsValidKey(key))
                return BadRequest(ResponseModel.CreateWithError(ErrorType.Validation, InvalidKeyMessage));
                
            if (!CommonValidators.IsValidPayload(model.Data))
                return BadRequest(ResponseModel.CreateWithError(ErrorType.Validation, InvalidPayloadMessage));
            
            await _clientDictionary.SetAsync(clientId, key, model.Data);

            return Ok(ResponseModel.CreateWithData(null));
        }
        
        [HttpDelete("{clientId}/{key}")]
        [ProducesResponseType(typeof(ResponseModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseModel), (int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ResponseModel), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete(string clientId, string key)
        {
            try
            {
                if (!CommonValidators.IsValidClientId(clientId))
                    return BadRequest(ResponseModel.CreateWithError(ErrorType.Validation, InvalidClientIdMessage));
                
                if (!CommonValidators.IsValidKey(key))
                    return BadRequest(ResponseModel.CreateWithError(ErrorType.Validation, InvalidKeyMessage));
                
                await _clientDictionary.DeleteAsync(clientId, key);
                
                return Ok(ResponseModel.CreateWithData(null));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ResponseModel.CreateWithError(ErrorType.NotFound, null));
            }
        }
    }
}
