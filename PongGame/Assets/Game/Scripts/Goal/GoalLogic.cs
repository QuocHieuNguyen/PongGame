using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GoalLogic
{
    private readonly int playerScoreId;

    private GoalNetwork goalNetwork;

    public GoalLogic( int playerScoreId, GoalNetwork goalNetwork)
    {
        this.playerScoreId = playerScoreId;
        this.goalNetwork = goalNetwork;
    }
    public void Collide(IBall ball, IBallCollision ballCollision){
        Debug.Log("Player lose with score id " + playerScoreId);
        goalNetwork.SendData();
    }

    public void OnDisable()
    {
        
    }
}
