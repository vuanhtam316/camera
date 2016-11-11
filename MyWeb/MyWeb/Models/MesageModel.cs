namespace MyWeb.Models
{
    public class MesageModel
    {
        public enum MessageType : short
        {
            Success,
            Error,
            Info,
            Warning
        }

        public class MessagesModel
        {
            public MessageType MessageType { get; set; }
            public string MessageContent { get; set; }
            public string LinkText { get; set; }
            public string LinkUrl { get; set; }
        }
    }
}