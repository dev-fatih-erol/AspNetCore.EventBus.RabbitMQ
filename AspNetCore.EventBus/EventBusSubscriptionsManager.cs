using System;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCore.EventBus
{
    public class EventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {
        private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;

        private readonly Dictionary<string, Type> _eventTypes;

        public event EventHandler<string> OnEventRemoved;

        public EventBusSubscriptionsManager()
        {
            _handlers = new Dictionary<string, List<SubscriptionInfo>>();

            _eventTypes = new Dictionary<string, Type>();
        }

        public bool IsEmpty => !_handlers.Keys.Any();

        public void Clear() => _handlers.Clear();

        public void AddDynamicSubscription<TH>(string eventName) where TH : IDynamicEventHandler
        {
            DoAddSubscription(typeof(TH), eventName, true);
        }

        public void AddSubscription<T, TH>() where T : Event where TH : IEventHandler<T>
        {
            var eventName = GetEventKey<T>();

            DoAddSubscription(typeof(TH), eventName, false);

            _eventTypes.Add(eventName, typeof(T));
        }

        private void DoAddSubscription(Type handlerType, string eventName, bool isDynamic)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                _handlers.Add(eventName, new List<SubscriptionInfo>());
            }

            if (_handlers[eventName].Any(s => s.HandlerType == handlerType))
            {
                throw new ArgumentException($"Handler Type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
            }

            _handlers[eventName].Add(isDynamic ? SubscriptionInfo.Dynamic(handlerType) : SubscriptionInfo.Typed(handlerType));
        }

        public void RemoveDynamicSubscription<TH>(string eventName) where TH : IDynamicEventHandler
        {
            var handlerToRemove = FindDynamicSubscriptionToRemove<TH>(eventName);

            DoRemoveHandler(eventName, handlerToRemove);
        }

        public void RemoveSubscription<T, TH>() where TH : IEventHandler<T> where T : Event
        {
            var handlerToRemove = FindSubscriptionToRemove<T, TH>();

            var eventName = GetEventKey<T>();

            DoRemoveHandler(eventName, handlerToRemove);
        }

        private void DoRemoveHandler(string eventName, SubscriptionInfo subsToRemove)
        {
            if (subsToRemove != null)
            {
                _handlers[eventName].Remove(subsToRemove);

                if (!_handlers[eventName].Any())
                {
                    _handlers.Remove(eventName);

                    if (_eventTypes.ContainsKey(eventName))
                    {
                        _eventTypes.Remove(eventName);
                    }

                    RaiseOnEventRemoved(eventName);
                }
            }
        }

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : Event
        {
            var key = GetEventKey<T>();

            return GetHandlersForEvent(key);
        }

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName) => _handlers[eventName];

        private void RaiseOnEventRemoved(string eventName)
        {
            OnEventRemoved?.Invoke(this, eventName);
        }

        private SubscriptionInfo FindDynamicSubscriptionToRemove<TH>(string eventName) where TH : IDynamicEventHandler
        {
            return DoFindSubscriptionToRemove(eventName, typeof(TH));
        }

        private SubscriptionInfo FindSubscriptionToRemove<T, TH>() where T : Event where TH : IEventHandler<T>
        {
            var eventName = GetEventKey<T>();

            return DoFindSubscriptionToRemove(eventName, typeof(TH));
        }

        private SubscriptionInfo DoFindSubscriptionToRemove(string eventName, Type handlerType)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                return null;
            }

            return _handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);
        }

        public bool HasSubscriptionsForEvent<T>() where T : Event
        {
            var key = GetEventKey<T>();

            return HasSubscriptionsForEvent(key);
        }

        public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);

        public Type GetEventTypeByName(string eventName) => _eventTypes[eventName];

        public string GetEventKey<T>()
        {
            return typeof(T).Name;
        }
    }
}