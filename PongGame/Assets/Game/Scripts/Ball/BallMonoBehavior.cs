using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMonoBehavior : MonoBehaviour, ILocalPositionAdapter
{
    public Vector3 InitialVelocity;

    private BallLogic _ballLogic;
    private BallSimulator _ballSimulator;

    public Vector3 LocalPosition
    {
        get => transform.localPosition;
        set => transform.localPosition = value;
    }

    private void Awake()
    {
        _ballSimulator = new BallSimulator(InitialVelocity);
        _ballLogic = new BallLogic(_ballSimulator, this);
    }

    private void FixedUpdate()
    {
        _ballLogic.Update(Time.deltaTime);
    }
}
