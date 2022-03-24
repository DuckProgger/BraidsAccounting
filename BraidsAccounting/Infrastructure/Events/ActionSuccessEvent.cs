using Prism.Events;

namespace BraidsAccounting.Infrastructure.Events
{
    internal class ActionSuccessEvent : PubSubEvent<bool>
    {
    }
}
