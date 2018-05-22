using System.Net;
using System.Threading.Tasks;
using Lykke.Service.ClientDictionaries.AzureRepositories.ClientDictionaryBlob;
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
        private const string TooBigPayloadMessage = "Too big value provided";

        private readonly IClientDictionary _clientDictionary;
        private readonly IInputValidator _inputValidator;

        public DictionaryController(
            IClientDictionary clientDictionary,
            IInputValidator inputValidator)
        {
            _clientDictionary = clientDictionary;
            _inputValidator = inputValidator;
        }

        [HttpGet("{clientId}/{key}")]
        [ProducesResponseType(typeof(ResponseModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseModel), (int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ResponseModel), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get(string clientId, string key)
        {
            if (!_inputValidator.IsValidClientId(clientId))
                return BadRequest(ResponseModel.CreateWithError(ErrorType.Validation, InvalidClientIdMessage));

            if (!_inputValidator.IsValidKey(key))
                return BadRequest(ResponseModel.CreateWithError(ErrorType.Validation, InvalidKeyMessage));

            try
            {
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
        public async Task<IActionResult> Post(string clientId, string key, [FromBody] SetRequestModel model)
        {
            if (!_inputValidator.IsValidClientId(clientId))
                return BadRequest(ResponseModel.CreateWithError(ErrorType.Validation, InvalidClientIdMessage));

            if (!_inputValidator.IsValidKey(key))
                return BadRequest(ResponseModel.CreateWithError(ErrorType.Validation, InvalidKeyMessage));

            if (!_inputValidator.IsValidPayload(model.Data))
                return BadRequest(ResponseModel.CreateWithError(ErrorType.Validation, InvalidPayloadMessage));

            if (!_inputValidator.IsValidPayloadSize(model.Data))
                return BadRequest(ResponseModel.CreateWithError(ErrorType.Validation, TooBigPayloadMessage));

            await _clientDictionary.SetAsync(clientId, key, model.Data);

            return Ok(ResponseModel.CreateWithData(null));
        }

        [HttpDelete("{clientId}/{key}")]
        [ProducesResponseType(typeof(ResponseModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseModel), (int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ResponseModel), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete(string clientId, string key)
        {
            if (!_inputValidator.IsValidClientId(clientId))
                return BadRequest(ResponseModel.CreateWithError(ErrorType.Validation, InvalidClientIdMessage));

            if (!_inputValidator.IsValidKey(key))
                return BadRequest(ResponseModel.CreateWithError(ErrorType.Validation, InvalidKeyMessage));
            
            try
            {
                await _clientDictionary.DeleteAsync(clientId, key);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ResponseModel.CreateWithError(ErrorType.NotFound, null));
            }
            
            return Ok(ResponseModel.CreateWithData(null));
        }
    }
}
