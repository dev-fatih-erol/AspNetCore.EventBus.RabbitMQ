using System;
using System.Collections.Generic;

namespace AspNetCore.EventBus
{
    public interface IEventBusSubscriptionsManager
    {
        bool IsEmpty { get; }

        event EventHandler<string> OnEventRemoved;

        void AddDynamicSubscription<TH>(string eventName) where TH : IDynamicEventHandler;

        void AddSubscription<T, TH>() where T : Event where TH : IEventHandler<T>;

        void RemoveSubscription<T, TH>() where TH : IEventHandler<T> where T : Event;

        void RemoveDynamicSubscription<TH>(string eventName) where TH : IDynamicEventHandler;

        bool HasSubscriptionsForEvent<T>() where T : Event;

        bool HasSubscriptionsForEvent(string eventName);

        Type GetEventTypeByName(string eventName);

        void Clear();

        IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : Event;

        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);

        string GetEventKey<T>();
    }
}