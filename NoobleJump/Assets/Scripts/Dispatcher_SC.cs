using System;
using UnityEngine;
using System.Collections.Generic;

public enum EventId
{
    unknown = 0,

    playerKilled = 1,
}

public class Dispatcher_SC : MonoBehaviour
{
    private const int PRIORITY_COUNT = 5;

    public delegate void EventSubscribeHandler(EventId id, EventInfo info);

    private static Dictionary<int, List<EventSubscribeHandler>> handlers =
        new Dictionary<int, List<EventSubscribeHandler>>();

    public static void Subscribe(EventId id, EventSubscribeHandler handler)
    {
        int intId = (int)id;
        List<EventSubscribeHandler> foundHandler;

        if (!handlers.TryGetValue(intId, out foundHandler))
        {
            foundHandler = new List<EventSubscribeHandler>();
            handlers.Add(intId, foundHandler);
        }
        foundHandler.Add(handler);
    }

    public static void Unsubscribe(EventId id, EventSubscribeHandler handler)
    {
        List<EventSubscribeHandler> subscriptions;
        int intId = (int)id;

        if (!handlers.TryGetValue(intId, out subscriptions))
            return;

        subscriptions.Remove(handler);
    }

    public static void Send(EventId id, EventInfo info)
    {
        List<EventSubscribeHandler> subscriptions;

        if (!handlers.TryGetValue((int)id, out subscriptions))
            return;

        foreach (var subscription in subscriptions)
        {
            subscription.Invoke(id, info);
        }
    }
}

public class EventInfo
{
    public string name;
    public int firstValue;

    public EventInfo(string _name, int _firstValue)
    {
        name = _name;
        firstValue = _firstValue;
    }
}
