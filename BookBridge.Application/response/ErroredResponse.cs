
namespace BookBridge.Application.response
{
    public class ErrorResponse : Response<object>
    {
        public ErrorResponse(bool hasSuccess, object result, ResponseError responseError) : base(hasSuccess, result, responseError) { }
        public ErrorResponse(bool hasSuccess, object result, ICollection<ResponseError> errors) : base(hasSuccess, result, errors) { }
        public ErrorResponse(bool hasSuccess, object result, ICollection<ResponseMessage> messages) : base(hasSuccess, result, messages) { }
        public ErrorResponse(bool hasSuccess, object result, ICollection<ResponseMessage> messages, ICollection<ResponseError> errors) : base(hasSuccess, result, messages, errors) { }

        public static ErrorResponse Create(string errorMessage)
        {
            return new ErrorResponse(false, null, new ResponseError(string.Empty, errorMessage));
        }
        public static ErrorResponse Create(Exception e, string errorMessage)
        {
            return new ErrorResponse(false, null, new ResponseError(string.Empty, errorMessage, e.ToString()));
        }
        public static ErrorResponse Create(Exception e, string key, string errorMessage)
        {
            return new ErrorResponse(false, null, new ResponseError(key, errorMessage, e.ToString()));
        }
        public static ErrorResponse Create(string key, string errorMessage, bool? isBusinessError = null)
        {
            return new ErrorResponse(false, null, new ResponseError(key, errorMessage, isBusinessError));
        }
        public static ErrorResponse Create(ICollection<ResponseError> errors)
        {
            return new ErrorResponse(false, null, errors);
        }
        public static ErrorResponse Create<T>(Response<T> response)
        {
            return new ErrorResponse(false, null, response.Messages, response.Errors);
        }
        public static ErrorResponse CreateMessage(string message, MessageType messageType)
        {
            var messages = new List<ResponseMessage>() { new(string.Empty, message, messageType) };
            return new ErrorResponse(false, null, messages);
        }
        public static ErrorResponse CreateMessage(string key, string message, MessageType messageType)
        {
            var messages = new List<ResponseMessage>() { new(key, message, messageType) };
            return new ErrorResponse(false, null, messages);
        }
    }
}
