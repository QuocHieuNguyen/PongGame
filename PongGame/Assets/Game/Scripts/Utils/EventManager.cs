using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

// [System.Serializable]
// public class TypedEvent : Action<object> { }

public class EventManager : MonoBehaviour
{

    // ...
    private Dictionary<string, Action<object>> typedEventDictionary;

    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType (typeof (EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError ("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init (); 
                }
            }

            return eventManager;
        }
    }

    void Init()
    {
        if (typedEventDictionary == null)
        {
            // ...
            typedEventDictionary = new Dictionary<string, Action<object>>();
        }
    }

    // ...
    public static void StartListening(string eventName, Action<object> listener)
    {
        Action<object> thisEvent = null;
        if (instance.typedEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listener;
        }
        else
        {
            thisEvent += listener;
            instance.typedEventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, Action<object> listener)
    {
        if (eventManager == null) return;
        Action<object> thisEvent = null;
        if (instance.typedEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
        }
    }

    public static void TriggerEvent(string eventName, object data)
    {
        Action<object> thisEvent = null;
        if (instance.typedEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(data);
        }
    }
}