using System.Collections;
using System.Collections.Generic;
using SocketIO;
using UnityEngine;

public class SocketIOTest : MonoBehaviour {
    public SocketIOComponent socket;
    // Start is called before the first frame update
    void Start () {
        socket.On ("boop", TestBoop);
        socket.Emit("beep");
    }

    // Update is called once per frame
    void Update () {

    }
    public void TestBoop (SocketIOEvent e) {
        Debug.Log (string.Format ("[name: {0}, data: {1}]", e.name, e.data));
    }
}