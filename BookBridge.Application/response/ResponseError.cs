namespace BookBridge.Application.response
{
    public class ResponseError
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public string Details { get; set; }

        public bool? IsBusinessError { get; set; }

        public ResponseError() { }

        public ResponseError(string key, string value, bool? isBusinessError = null) : this(key, value, null as string, isBusinessError) { }

        public ResponseError(string key, string value, string details, bool? isBusinessError = null)
        {
            Key = key;
            Value = value;
            Details = details;
            IsBusinessError = isBusinessError;
        }
    }
}