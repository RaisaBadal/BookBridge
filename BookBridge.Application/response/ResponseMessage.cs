namespace BookBridge.Application.response
{
    public class ResponseMessage
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public MessageType Type { get; set; }

        public ResponseMessage() { }

        public ResponseMessage(string key, string value, MessageType type)
        {
            Key = key;
            Value = value;
            Type = type;
        }
    }

    public enum MessageType
    {
        Info = 1,
        Warning = 2,
        ViewPermission = 3,
        Error = 4,
        Restriction = 5,
        BusinessProcessCanceled = 6
    }
}