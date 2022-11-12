using System;
using System.Collections;
using System.Collections.Generic;
using SocketIO;
using UnityEngine;

[System.Serializable]
public class BallNetwork {
    private SocketIOComponent socketIOComponent;
    private ClientPrediction _clientPrediction;
    public Action<float, float> onUpdatePosition;

    public BallNetwork (SocketIOComponent socketIOComponent, Transform transform) {
        this.socketIOComponent = socketIOComponent;
        socketIOComponent.On("updateBallPosition", UpdatePosition);
        //_clientPrediction = new ClientPrediction(socketIOComponent,transform);
        
    }
    private void UpdatePosition(SocketIOEvent socketIOEvent) {
        float x = socketIOEvent.data["position"]["x"].f;
        float y = socketIOEvent.data["position"]["y"].f;
        onUpdatePosition?.Invoke (x, y);
    }

    public void Update()
    {
        _clientPrediction?.Update();
    }
    public void RequestReflectFromWall(){
        socketIOComponent.Emit("reflectFromWall");
    }
    public void RequestReflectFromPaddle(){
        socketIOComponent.Emit("reflectFromPaddle");
    }

    public void UnsubscribeEvent()
    {
        socketIOComponent.Off("updateBallPosition");
    }
}