using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLogic
{
    private BallSimulator ballSimulator;
    private ILocalPositionAdapter localPositionAdapter;
    public Action onDestroyed;

    public BallLogic(BallSimulator ballSimulator, ILocalPositionAdapter localPositionAdapter)
    {
        this.ballSimulator = ballSimulator;
        this.localPositionAdapter = localPositionAdapter;
    }

    public void Update(float deltaTime)
    {
        Vector3 newPos = ballSimulator.UpdatePosition(localPositionAdapter.LocalPosition, deltaTime);
        localPositionAdapter.LocalPosition = newPos;
    }
}
