

namespace BookBridge.Application.response
{
    public class Response<T> : IResponse
    {
        public bool Succeeded { get; set; }

        public bool? HasViewPermission { get; set; }

        public T Data { get; set; }

        public ICollection<ResponseError> Errors { get; set; } = new List<ResponseError>();

        public ICollection<ResponseMessage> Messages { get; set; } = new List<ResponseMessage>();

        public Response()
        {
            Errors = new List<ResponseError>();
            Messages = new List<ResponseMessage>();
        }

        public Response(bool hasSuccess, T result)
        {
            Succeeded = hasSuccess;
            Data = result;
        }

        public Response(bool hasSuccess, T result, ResponseError responseError)
        {
            Succeeded = hasSuccess;
            Data = result;
            Errors = responseError == null ? null : new List<ResponseError>
            {
                responseError
            };
        }

        public Response(bool hasSuccess, T result, ResponseMessage responseMessage)
        {
            Succeeded = hasSuccess;
            Data = result;
            Messages = responseMessage == null ? null : new List<ResponseMessage>
            {
                responseMessage
            };
            Errors = new List<ResponseError>();
        }

        public Response(bool hasSuccess, T result, ResponseMessage responseMessage, ResponseError responseError)
        {
            Succeeded = hasSuccess;
            Data = result;
            Messages = responseMessage == null ? null : new List<ResponseMessage>
            {
                responseMessage
            };
            Errors = responseError == null ? null : new List<ResponseError>
            {
                responseError
            };
        }

        public Response(bool hasSuccess, T result, ICollection<ResponseMessage> messages, ICollection<ResponseError> errors)
        {
            Succeeded = hasSuccess;
            Data = result;
            Messages = messages;
            Errors = errors;
        }

        public Response(bool hasSuccess, T result, ICollection<ResponseError> errors)
        {
            Succeeded = hasSuccess;
            Data = result;
            Errors = errors;
        }

        public Response(bool hasSuccess, T result, ICollection<ResponseMessage> messages)
        {
            Succeeded = hasSuccess;
            Data = result;
            Messages = messages;
        }

        public Response(T result)
        {
            Succeeded = true;
            Data = result;
            Errors = new List<ResponseError>();
            Messages = new List<ResponseMessage>();
        }

        public static implicit operator Response<T>(ErrorResponse errorReponse)
        {
            return new Response<T>(false, default(T), errorReponse.Messages, errorReponse.Errors);
        }

        public static Response<T> Ok(T result)
        {
            return new Response<T>(true, result);
        }

        public static Response<T> Error(string message)
        {
            return Error(string.Empty, message);
        }

        public static Response<T> Error(Response<T> response)
        {
            return Error(response.Errors);
        }

        public static Response<T> Error(string key, string value)
        {
            var responseError = new ResponseError(key, value);
            return new Response<T>(false, default(T), responseError);
        }

        public static Response<T> Error(string key, string value, string details)
        {
            var responseError = new ResponseError(key, value, details);
            return new Response<T>(false, default(T), responseError);
        }

        public static Response<T> Error(IList<ResponseError> errors)
        {
            return new Response<T>(false, default(T), errors);
        }
        public static Response<T> Error(ICollection<ResponseError> errors)
        {
            return new Response<T>(false, default(T), errors);
        }

        public void AddErrors(ICollection<ResponseError> errors)
        {
            if (errors == null) return;
            if (Errors == null) Errors = new List<ResponseError>();

            foreach (var item in errors)
            {
                Errors.Add(item);
            }
        }
    }

    public static class OkResponse
    {
        public static Response<T> Create<T>(T data)
        {
            return Response<T>.Ok(data);
        }
    }
}