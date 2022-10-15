using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class DisconnectionHandler : Singleton<DisconnectionHandler>
{
    private SocketIOComponent socketReference;

    public SocketIOComponent SocketReference
    {
        get
        {
            return socketReference = (socketReference == null) ? FindObjectOfType<SocketIOComponent>() : socketReference;
        }
    }

    private void Awake()
    {
        SocketReference.On("disconnect", HandleDisconnection);
    }

    void HandleDisconnection(SocketIOEvent socketIOEvent)
    {
        SceneManagementSystem.Instance.UnlockAllCurrentlyLoadedScenes();
        SceneManagementSystem.Instance.LoadLevel(SceneList.LOGIN, (value)=> {

        });
    }
}
