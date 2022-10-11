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

    public BallLogic(BallSimulator ballSimulator,BallNetwork ballNetwork, ILocalPositionAdapter localPositionAdapter)
    {
        this.ballSimulator = ballSimulator;
        this.ballNetwork = ballNetwork;
        this.localPositionAdapter = localPositionAdapter;
        ballNetwork.onUpdatePosition += Update;
    }

    public void Update(float x, float y)
    {
        Vector3 currentPos = new Vector3(x,y, localPositionAdapter.LocalPosition.z);
        Vector3 newPos = ballSimulator.UpdatePosition(currentPos, Time.deltaTime);
        localPositionAdapter.LocalPosition = newPos;
    }
}
