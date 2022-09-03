using System.Collections;
using System.Collections.Generic;

using UnityEngine;
public class NetworkClient : MonoBehaviour {


    void Start () {
        Debug.Log ("start");
        /*socket = IO.Socket ("http://localhost:3000");

        socket.On (QSocket.EVENT_CONNECT, () => {
            Debug.Log ("Connected");
            socket.Emit ("chat message", "test");
        });

        socket.On ("chat message", data => {
            Debug.Log ("data : " + data);
        });*/
    }
    private void OnDestroy () {
        /*socket.Disconnect ();*/
    }
    // Update is called once per frame
    void Update () {

    }
}