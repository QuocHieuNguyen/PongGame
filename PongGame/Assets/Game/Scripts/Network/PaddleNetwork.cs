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
    private bool isControlling = false;

    public PaddleNetwork(SocketIOComponent socketIOComponent, bool isControlling){
        this.socketIOComponent = socketIOComponent;
        this.isControlling = isControlling;
        ValidateSendData();
    }
    
    public void ValidateSendData()
    {
        if (isControlling)
        {
            socketIOComponent.On("updatePosition", UpdatePosition);
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
}