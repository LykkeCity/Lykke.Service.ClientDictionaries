using System.Net;
using System.Threading.Tasks;
using Lykke.Service.ClientDictionaries.AzureRepositories.ClientDictionaries;
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

        private readonly ClientDictionaryRepository _clientDictionaryTable;
        private readonly ClientDictionaryBlob _clientDictionaryBlob;
        private readonly IInputValidator _inputValidator;

        public DictionaryController(
            ClientDictionaryRepository clientDictionaryTable,
            ClientDictionaryBlob clientDictionaryBlob,
            IInputValidator inputValidator)
        {
            _clientDictionaryTable = clientDictionaryTable;
            _clientDictionaryBlob = clientDictionaryBlob;
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
                var value = await _clientDictionaryBlob.GetAsync(clientId, key);

                return Ok(ResponseModel.CreateWithData(value));
            }
            catch (KeyNotFoundException)
            {
                try
                {
                    var value = await _clientDictionaryTable.GetAsync(clientId, key);

                    return Ok(ResponseModel.CreateWithData(value));
                }
                catch (KeyNotFoundException)
                {
                    return NotFound(ResponseModel.CreateWithError(ErrorType.NotFound, null));
                }
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

            await _clientDictionaryBlob.SetAsync(clientId, key, model.Data);

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

            bool deletedFromBlob;
            bool deletedFromTable;
            
            try
            {
                await _clientDictionaryBlob.DeleteAsync(clientId, key);

                deletedFromBlob = true;
            }
            catch (KeyNotFoundException)
            {
                deletedFromBlob = false;
            }
            
            try
            {
                await _clientDictionaryTable.DeleteAsync(clientId, key);

                deletedFromTable = false;
            }
            catch (KeyNotFoundException)
            {
                deletedFromTable = false;
            }
            
            if(!deletedFromBlob && !deletedFromTable)
                return NotFound(ResponseModel.CreateWithError(ErrorType.NotFound, null));
            
            return Ok(ResponseModel.CreateWithData(null));
        }
    }
}
