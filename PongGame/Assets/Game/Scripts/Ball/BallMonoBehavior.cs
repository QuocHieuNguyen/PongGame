using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class BallMonoBehavior : MonoBehaviour, ILocalPositionAdapter
{
    public Vector3 InitialVelocity;

    private BallLogic _ballLogic;
    private BallSimulator _ballSimulator;

    private BallNetwork _ballNetwork;

    public Vector3 LocalPosition
    {
        get => transform.localPosition;
        set => transform.localPosition = value;
    }

    private void Awake()
    {
        _ballSimulator = new BallSimulator(InitialVelocity);
        _ballNetwork = new BallNetwork(FindObjectOfType<SocketIOComponent>());
        _ballLogic = new BallLogic(_ballSimulator,_ballNetwork,  this);
    }

    private void FixedUpdate()
    {
        //_ballLogic.Update(Time.deltaTime);
    }
}
