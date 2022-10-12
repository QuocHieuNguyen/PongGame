using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallLogic
{
    private readonly int playerScoreId;

    public WallLogic( int playerScoreId)
    {
        this.playerScoreId = playerScoreId;
    }
    public void Collide(IBall ball, IBallCollision ballCollision){
        ball.OnBallCollisionEnter(ballCollision);
    }
    public void Hit()
    {
        Debug.Log("Get Point");

    }
}
