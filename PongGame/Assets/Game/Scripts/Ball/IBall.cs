using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBall
{
    public void OnBallCollisionEnter(IBallCollision collider);
}
