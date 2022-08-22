using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRacket : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private float speed = 2f;

    private void OnEnable()
    {
        EventManager.StartListening(EventID.MOVE_RACKET, Move);
    }
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            EventManager.TriggerEvent(EventID.MOVE_RACKET, speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            EventManager.TriggerEvent(EventID.MOVE_RACKET, -speed);
        }

    }
    void Move(object obj)
    {
        Debug.Log("Move " + obj);
        float v = (float)obj;
        rigidbody2D.AddForce(new Vector2(0, v));
        //rigidbody2D.velocity = new Vector2(0, (float)obj);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventID.MOVE_RACKET, Move);
    }
}