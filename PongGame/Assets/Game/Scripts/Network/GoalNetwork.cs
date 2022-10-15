using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class GoalNetwork 
{
    private SocketIOComponent socketIOComponent;

    public GoalNetwork(SocketIOComponent socketIOComponent)
    {
        this.socketIOComponent = socketIOComponent;
    }

    public void SendData()
    {
        socketIOComponent.Emit("playerIsLose");
    }
    
}
