using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private MoveRacket moveRacket;
    private void OnEnable()
    {
        GameManager.instance.Subscribe(EventID.MOVE_RACKET, (obj)=>{
            moveRacket.Move(obj);
        });
    }
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            GameManager.instance.Invoke(EventID.MOVE_RACKET, moveRacket.GetSpeed());
        }
        else if (Input.GetKey(KeyCode.S))
        {
            GameManager.instance.Invoke(EventID.MOVE_RACKET, -moveRacket.GetSpeed());
        }else{
            moveRacket.Move(0f);
        }

    }
    
    private void OnDisable()
    {
        GameManager.instance.Unsubscribe(EventID.MOVE_RACKET, (obj)=>{
            moveRacket.Move(obj);
        });
    }
}
