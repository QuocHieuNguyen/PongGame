using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalLogic
{
    private readonly int playerScoreId;

    public GoalLogic( int playerScoreId)
    {
        this.playerScoreId = playerScoreId;
    }
    public void Collide(IBall ball, IBallCollision ballCollision){
        Debug.Log("Player lose with score id " + playerScoreId);
    }
}
