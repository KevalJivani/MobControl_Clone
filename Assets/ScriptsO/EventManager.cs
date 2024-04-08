using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    public static readonly CannonEvents cannonEvents = new CannonEvents();
    public static readonly CastleEvents castleEvents = new CastleEvents();


    public class CastleEvents 
    {
        public class CastleDestroyedEvent : UnityEvent<Component, GameObject> { }

        public GenericEvent<CastleDestroyedEvent> OnCastleDestroyed = new GenericEvent<CastleDestroyedEvent>();
    }

    public class CannonEvents
    {
        public class CannonMovementEvent : UnityEvent<Component> { }

        public GenericEvent<CannonMovementEvent> OnMovementCompleted = new GenericEvent<CannonMovementEvent>();
    }


    public class GenericEvent<T> where T : class, new()
    {
        private ConcurrentDictionary<string, T> map;

        public GenericEvent()
        {
            map = new ConcurrentDictionary<string, T>();
        }

        public T Get(string channel = "")
        {
            return map.GetOrAdd(channel, new T());
        }
    }
}
