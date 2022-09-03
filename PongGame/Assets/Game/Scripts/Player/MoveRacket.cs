using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveRacket : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 2f;
    public float currentValue;


    public void Move(object obj)
    {
        
        float v = (float)obj;
        Debug.Log("Move " + v);
        currentValue = v;
        //rb.AddForce(new Vector2(0, v));
        //rb.velocity = new Vector2(0, v*GetSpeed());
        transform.position = new Vector2(currentValue, currentValue);
    }
    public float GetSpeed(){
        return speed;
    }

}