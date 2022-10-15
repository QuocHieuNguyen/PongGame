using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SocketIO;

[System.Serializable]
public class PaddleNetwork
{
    private SocketIOComponent socketIOComponent;
    public Action<float, float> onUpdatePosition;
    private Vector2 currPos;
    private bool hasAuthority = false;

    public PaddleNetwork(SocketIOComponent socketIOComponent, bool hasAuthority){
        this.socketIOComponent = socketIOComponent;
        this.hasAuthority = hasAuthority;
        ValidateSendData();
    }

    public bool GetHasAuthority()
    {
        return hasAuthority;
    }
    public void ValidateSendData()
    {
        if (hasAuthority)
        {
            socketIOComponent.On("updatePosition", UpdatePosition);
        }
        else
        {
            socketIOComponent.On("updatePositionState", UpdatePosition);
        }
    }
    public void SendData(float x, float y){
        Vector2 position = new Vector2(x, y);
        if (position != currPos)
        {
            socketIOComponent.Emit("updatePosition", new JSONObject(JsonUtility.ToJson(position)));
            currPos = position;
        }

        
    }
    private void UpdatePosition(SocketIOEvent socketIOEvent){
        Debug.Log("Get Data");
        float x = socketIOEvent.data["x"].f;
        float y = socketIOEvent.data["y"].f;
        onUpdatePosition?.Invoke(x, y);
    }

    public void UnsubscribeEvent()
    {
        socketIOComponent.Off("updatePosition", UpdatePosition);
        socketIOComponent.Off("updatePositionState", UpdatePosition);
    }
}
