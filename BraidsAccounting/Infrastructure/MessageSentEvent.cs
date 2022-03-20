using Prism.Events;

namespace BraidsAccounting.Infrastructure
{
    public class MessageSentEvent : PubSubEvent<string>
    {
    }
}
