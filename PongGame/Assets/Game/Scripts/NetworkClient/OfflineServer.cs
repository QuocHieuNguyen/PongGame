using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineServer : GameCore.NetworkClient
{
    public override void Invoke(string key, object value)
    {
        EventManager.TriggerEvent(key, value);
    }
    public override void Listen(string key, Action<object> value)
    {
        EventManager.StartListening(key, value);
    }
    public override void StopListen(string key, Action<object> value)
    {
        EventManager.StopListening(key, value);
    }
}