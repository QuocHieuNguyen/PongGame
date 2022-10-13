using System;
using System.Collections;
using System.Collections.Generic;
using SocketIO;
using UnityEngine;

[System.Serializable]
public class BallNetwork {
    private SocketIOComponent socketIOComponent;

    public Action<float, float> onUpdatePosition;

    public BallNetwork (SocketIOComponent socketIOComponent) {
        this.socketIOComponent = socketIOComponent;
        socketIOComponent.On("updateBallPosition", UpdatePosition);
    }
    private void UpdatePosition(SocketIOEvent socketIOEvent) {
        Debug.Log ("Get Ball Position Data");
        float x = socketIOEvent.data["position"]["x"].f;
        float y = socketIOEvent.data["position"]["y"].f;
        onUpdatePosition?.Invoke (x, y);
    }
    public void RequestReflectFromWall(){
        socketIOComponent.Emit("reflectFromWall");
    }
}