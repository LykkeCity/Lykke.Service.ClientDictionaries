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
        [ProducesResponseType(typeof(RecordPayloadModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get(string clientId, string key)
        {
            try
            {
                if (!CommonValidators.IsValidClientId(clientId))
                    return BadRequest(ErrorResponse.Create(InvalidClientIdMessage));
                
                if (!CommonValidators.IsValidKey(key))
                    return BadRequest(ErrorResponse.Create(InvalidKeyMessage));
                
                var value = await _clientDictionary.GetAsync(clientId, key);
                
                return Ok(new RecordPayloadModel {Payload = value});
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
        
        [HttpPost("{clientId}/{key}")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post(string clientId, string key, [FromBody]RecordPayloadModel payload)
        {
            if (!CommonValidators.IsValidClientId(clientId))
                return BadRequest(ErrorResponse.Create(InvalidClientIdMessage));
                
            if (!CommonValidators.IsValidKey(key))
                return BadRequest(ErrorResponse.Create(InvalidKeyMessage));
                
            if (!CommonValidators.IsValidPayload(payload.Payload))
                return BadRequest(ErrorResponse.Create(InvalidPayloadMessage));
            
            await _clientDictionary.SetAsync(clientId, key, payload.Payload);

            return Ok();
        }
        
        [HttpDelete("{clientId}/{key}")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete(string clientId, string key)
        {
            try
            {
                if (!CommonValidators.IsValidClientId(clientId))
                    return BadRequest(ErrorResponse.Create(InvalidClientIdMessage));
                
                if (!CommonValidators.IsValidKey(key))
                    return BadRequest(ErrorResponse.Create(InvalidKeyMessage));
                
                await _clientDictionary.DeleteAsync(clientId, key);
                
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
