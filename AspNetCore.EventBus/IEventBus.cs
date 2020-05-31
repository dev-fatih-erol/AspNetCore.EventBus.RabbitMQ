namespace AspNetCore.EventBus
{
    public interface IEventBus
    {
        void Publish(Event @event);

        void Subscribe<T, TH>() where T : Event where TH : IEventHandler<T>;

        void Unsubscribe<T, TH>() where TH : IEventHandler<T> where T : Event;

        void SubscribeDynamic<TH>(string eventName) where TH : IDynamicEventHandler;

        void UnsubscribeDynamic<TH>(string eventName) where TH : IDynamicEventHandler;
    }
}