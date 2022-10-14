using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallLogic
{

    public WallLogic()
    {
        
    }
    public void Collide(IBall ball, IBallCollision ballCollision){
        ball.OnBallCollisionEnter(ballCollision);
    }
}
