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

    public PaddleNetwork(SocketIOComponent socketIOComponent){
        this.socketIOComponent = socketIOComponent;
        socketIOComponent.On("updatePosition", UpdatePosition);
    }
    public void SendData(float x, float y){
        Vector2 position = new Vector2(x, y);
        socketIOComponent.Emit("updatePosition", new JSONObject(JsonUtility.ToJson(position)));
    }
    private void UpdatePosition(SocketIOEvent socketIOEvent){
        Debug.Log("Get Data");
        float x = socketIOEvent.data["x"].f;
        float y = socketIOEvent.data["y"].f;
        onUpdatePosition?.Invoke(x, y);
    }
}
