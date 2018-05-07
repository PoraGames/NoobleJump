using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Обработчик всех событий в игре
/// </summary>
public class Dispatcher_SC : MonoBehaviour
{
    /// <summary>
    /// Формат сообщений событий
    /// </summary>
    /// <param name="id"> id события  </param>
    /// <param name="info"> передаваемая информация </param>
    public delegate void EventSubscribeHandler(EventInfo info);

    /// <summary> Все подписчики </summary>
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

    /// <summary> Сообщить о событии </summary>
    public static void Send(EventId id, EventInfo info)
    {
        List<EventSubscribeHandler> subscriptions;

        if (!handlers.TryGetValue((int)id, out subscriptions))
            return;

        foreach (var subscription in subscriptions)
        {
            subscription.Invoke(info);
        }
    }
}
