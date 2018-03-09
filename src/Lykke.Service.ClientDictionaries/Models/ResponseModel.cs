using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lykke.Service.ClientDictionaries.Models
{
    public class ResponseModel
    {
        public string Data { set; get; }
        public ErrorModel Error { set; get; }

        public static ResponseModel CreateWithData(string data)
        {
            return new ResponseModel
            {
                Data = data
            };
        }

        public static ResponseModel CreateWithError(ErrorType type, string message)
        {
            return new ResponseModel
            {
                Error = new ErrorModel
                {
                    Type = type,
                    Message = message
                }
            };
        }
    }

    public class ErrorModel
    {
        public string Message { set; get; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ErrorType Type { set; get; }
    }

    public enum ErrorType
    {
        NotFound, Validation
    }
}
