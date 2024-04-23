using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    public static readonly CannonEvents cannonEvents = new CannonEvents();
    public static readonly CastleEvents castleEvents = new CastleEvents();
    public static readonly GameManagerEvents gameManagerEvents = new GameManagerEvents();


    public class CastleEvents 
    {
        public class CastleDestroyedEvent : UnityEvent<GameObject> { }

        public GenericEvent<CastleDestroyedEvent> OnCastleDestroyed = new GenericEvent<CastleDestroyedEvent>();
        
    }

    public class CannonEvents
    {
        public class CannonMovementEvent : UnityEvent { }

        public class CannonSONormalPlayerEvent : UnityEvent { }
        public class CannonSOSpecialPlayerEvent : UnityEvent { }

        public GenericEvent<CannonMovementEvent> OnMovementCompleted = new GenericEvent<CannonMovementEvent>();

        public GenericEvent<CannonSONormalPlayerEvent> OnNormalCharacterSpawned = new GenericEvent<CannonSONormalPlayerEvent>();
        public GenericEvent<CannonSOSpecialPlayerEvent> OnSpecialCharacterSpawned = new GenericEvent<CannonSOSpecialPlayerEvent>();
    }

    public class GameManagerEvents
    {
        public class LevelInstantiatedEvent : UnityEvent<GameObject> { }
        public class LevelCompletedEvent : UnityEvent { }
        public class GameFailedEvent : UnityEvent { }

        public GenericEvent<LevelInstantiatedEvent> OnLevelInstantiated = new GenericEvent<LevelInstantiatedEvent>();
        public GenericEvent<LevelCompletedEvent> OnLevelCompleted = new GenericEvent<LevelCompletedEvent>();
        public GenericEvent<GameFailedEvent> OnGameFailed = new GenericEvent<GameFailedEvent>();
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
