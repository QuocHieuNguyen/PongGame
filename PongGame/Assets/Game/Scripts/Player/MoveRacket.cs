using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Socket.Quobject.SocketIoClientDotNet.Client;

public class MoveRacket : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 2f;
    private QSocket socket;

    private void Start()
    {
        socket = IO.Socket ("http://localhost:3000");
        socket.On(QSocket.EVENT_CONNECT, () => {
            Debug.Log ("Connected");
            socket.Emit("chat message", "test");
        });
    }

    public void Move(object obj)
    {
        Debug.Log("Move " + obj);
        float v = (float)obj;
        //rb.AddForce(new Vector2(0, v));
        rb.velocity = new Vector2(0, v);
    }
    public float GetSpeed(){
        return speed;
    }

}