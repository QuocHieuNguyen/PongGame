using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private float speed = 30;
    [SerializeField] private Rigidbody2D rigidbody2D;

    void Start() {
        // Initial Velocity
        rigidbody2D.velocity = Vector2.right * speed;
    }
}
