using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class EventsListeners : MonoBehaviour
{
    private static List<GameObject> _listeners;
    private static bool _isInstanced;

    void Awake()
    {
        if (_isInstanced)
        {
            Debug.LogError(
                "Attempt to create multiple instances of EventListeners. All, except one, instances will be destoryed");
            Destroy(this);
        }
        _isInstanced = true;
        _listeners = new List<GameObject>(32);
    }

    /// <summary>
    /// Add listener to events list
    /// </summary>
    /// <param name="listener">new listener</param>
    public static void AddListener(GameObject listener)
    {
        if (listener == null) throw new ArgumentNullException(nameof(listener));
        if (_listeners.Contains(listener)) throw new ArgumentException("this listener already in list");
        _listeners.Add(listener);
    }

    public static bool RemoveListener(GameObject listener)
    {
        if(listener == null) throw new ArgumentNullException(nameof(listener));
        return _listeners.Remove(listener);
    }

    public static void Execute<T>(BaseEventData eventData, ExecuteEvents.EventFunction<T> function)
        where T : IEventSystemHandler
    {
        foreach (var listener in _listeners)
        {
            ExecuteEvents.Execute(listener, eventData, function);
        }
    }
}