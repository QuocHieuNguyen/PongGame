using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private MoveRacket moveRacket;
    Vector3 oldPosition;
    Vector3 currentPosition;
    private void OnEnable()
    {
        /*GameManager.instance.Subscribe(EventID.MOVE_RACKET, (obj)=>{
            moveRacket.Move(obj);
        });*/
        NetworkManager.instance.localPlayer = this;
    }
    void FixedUpdate()
    {
        /*if (Input.GetKey(KeyCode.W))
        {
            GameManager.instance.Invoke(EventID.MOVE_RACKET, moveRacket.GetSpeed());
        }
        else if (Input.GetKey(KeyCode.S))
        {
            GameManager.instance.Invoke(EventID.MOVE_RACKET, -moveRacket.GetSpeed());
        }else{
            moveRacket.Move(0f);
        }*/

        var y = Input.GetAxis ("Vertical") * Time.deltaTime * 3.0f;
        //transform.Translate (0, y,0);
        Vector3 newPos = transform.position + new Vector3(0, y);
        currentPosition = transform.position;

        if (currentPosition != newPos) {
            NetworkManager.instance.CommandMove(newPos);
            currentPosition = newPos;
            //oldPosition = currentPosition;
        }
        
    }
    
    private void OnDisable()
    {
        /*GameManager.instance.Unsubscribe(EventID.MOVE_RACKET, (obj)=>{
            moveRacket.Move(obj);
        });*/
    }
}
