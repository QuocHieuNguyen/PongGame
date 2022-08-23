using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRacket : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 2f;


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