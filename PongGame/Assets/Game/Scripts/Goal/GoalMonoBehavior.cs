using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalMonoBehavior : MonoBehaviour, IBallCollision
{

    private GoalLogic goalLogic;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Init(int playerScoreId){
        goalLogic = new GoalLogic(playerScoreId);
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
