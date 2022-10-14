using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMonoBehavior : MonoBehaviour, IBallCollision
{
    private WallLogic wallLogic;
    // Start is called before the first frame update
    void Start()
    {
        wallLogic = new WallLogic();
    }
    private void OnCollisionEnter(Collision other) {
        IBall ball = other.gameObject.GetComponent<IBall>();
        if (ball != null){
            Collide(ball);
        }

    }
    public void Collide(IBall ball){
        Debug.Log("Collide with ball");
        
        wallLogic.Collide(ball, this);
    }
}
