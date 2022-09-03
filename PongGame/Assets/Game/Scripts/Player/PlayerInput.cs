using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Socket.Quobject.SocketIoClientDotNet.Client;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private MoveRacket moveRacket;
    private QSocket socket;

    private void Start()
    {
        socket = IO.Socket ("http://localhost:3000");
        socket.On(QSocket.EVENT_CONNECT, () => {
            Debug.Log ("Connected");
            //socket.Emit("chat message", "test");
            socket.On(EventID.MOVE_RACKET, (data) => {
                Debug.Log(data);
                float value = float.Parse(data.ToString());
                moveRacket.Move(value);
            });
        });

    }
    private void OnEnable()
    {
        /*GameManager.instance.Subscribe(EventID.MOVE_RACKET, (obj)=>{
            moveRacket.Move(obj);
        });*/
    }
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            //GameManager.instance.Invoke(EventID.MOVE_RACKET, moveRacket.GetSpeed());
            EmitEvent();
        }
        /*else if (Input.GetKey(KeyCode.S))
        {
            //GameManager.instance.Invoke(EventID.MOVE_RACKET, -moveRacket.GetSpeed());
        }else{
            moveRacket.Move(0f);
        }*/

    }
    
    private void OnDisable()
    {
        /*GameManager.instance.Unsubscribe(EventID.MOVE_RACKET, (obj)=>{
            moveRacket.Move(obj);
        });*/
    }

    public void EmitEvent()
    {
        socket.Emit(EventID.MOVE_RACKET, "test");
    }
}
