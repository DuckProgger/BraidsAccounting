namespace BraidsAccounting.Infrastructure
{
    internal class Message
    {
        public string Text { get; set; } = null!;
        public MessageType Type { get; set; }
    }
    internal enum MessageType { Info, Error, Warning }
}
