using System.Collections;
using System.Collections.Generic;
using SocketIO;
using UnityEngine;

public class GoalMonoBehavior : MonoBehaviour, IBallCollision
{

    private GoalLogic goalLogic;
    private GoalNetwork goalNetwork;

    public void Init(int playerScoreId){
        goalNetwork = new GoalNetwork(FindObjectOfType<SocketIOComponent>());
        goalLogic = new GoalLogic(playerScoreId, goalNetwork);
    }
    private void OnCollisionEnter(Collision other) {
        IBall ball = other.gameObject.GetComponent<IBall>();
        if (ball != null){
            Collide(ball);
        }

    }
    public void Collide(IBall ball){
        Debug.Log("Collide with ball");
        
        goalLogic.Collide(ball, this);
    }
}
