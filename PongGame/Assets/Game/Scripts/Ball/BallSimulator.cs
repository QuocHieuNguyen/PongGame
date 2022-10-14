using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSimulator
{
    private Vector3 velocity;

    public BallSimulator(Vector3 velocity)
    {
        this.velocity = velocity;
    }

    public Vector3 UpdatePosition(Vector3 currentPosition, float deltaTime)
    {
        return currentPosition;
    }

    public void ReflectVelocityVertically()
    {
        velocity = new Vector3(velocity.x, -velocity.y, velocity.z);
    }

    public void ReflectVelocityHorizontally()
    {
        velocity = new Vector3(-velocity.x, velocity.y, velocity.z);
    }
}
