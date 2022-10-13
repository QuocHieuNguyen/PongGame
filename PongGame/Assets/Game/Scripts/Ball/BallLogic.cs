using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLogic
{
    private BallSimulator ballSimulator;
    private BallNetwork ballNetwork;
    private ILocalPositionAdapter localPositionAdapter;
    public Action onDestroyed;

    private Vector3 syncTransform;

    public BallLogic(BallSimulator ballSimulator,BallNetwork ballNetwork, ILocalPositionAdapter localPositionAdapter)
    {
        this.ballSimulator = ballSimulator;
        this.ballNetwork = ballNetwork;
        this.localPositionAdapter = localPositionAdapter;
        ballNetwork.onUpdatePosition += UpdatePosition;
    }
    public void Update(){
        localPositionAdapter.LocalPosition 
        = Vector3.Lerp(localPositionAdapter.LocalPosition, syncTransform, Time.deltaTime);
    }
    public void UpdatePosition(float x, float y)
    {
        Vector3 currentPos = new Vector3(x,y, localPositionAdapter.LocalPosition.z);
        Vector3 newPos = ballSimulator.UpdatePosition(currentPos, Time.deltaTime);
        syncTransform = newPos;
    }
    public void OnCollisionEnter(IBallCollision collider)
    {
        Debug.Log("Reflect from wall");
        ballNetwork.RequestReflectFromWall();
        syncTransform = localPositionAdapter.LocalPosition;
    }
}
