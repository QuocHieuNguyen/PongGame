using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class PaddleMonoBehavior : MonoBehaviour, IPositionAdapter, IBallCollision
{
    public PaddleData paddleData;
    [SerializeField] private string inputAxisName;
    private PaddleInput paddleInput;
    private PaddleSimulation paddleSimulation;
    private PaddleLogic paddleLogic;

    private PaddleNetwork paddleNetwork;

    private bool hasAuthority =false;
    // Start is called before the first frame update
    void Awake()
    {

    }

    public void Init()
    {
        paddleInput = new PaddleInput(inputAxisName);
        paddleSimulation = new PaddleSimulation();
        paddleNetwork = new PaddleNetwork(FindObjectOfType<SocketIOComponent>(), hasAuthority);
        paddleLogic = new PaddleLogic(this, paddleData, paddleInput, paddleSimulation, paddleNetwork);
    }
    public void SetHasAuthority(bool hasAuthority)
    {
        this.hasAuthority = hasAuthority;
        if (!hasAuthority)
        {
            inputAxisName = "";
        }

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (paddleLogic != null)
        {
            paddleLogic.Update(Time.deltaTime);
        }
       
    }

    public void Collide(IBall ball)
    {
        paddleLogic.Collide(ball, this);
    }

    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
    private void OnCollisionEnter(Collision other) {
        Debug.Log("Reflect from paddle");
        IBall ball = other.gameObject.GetComponent<IBall>();
        if (ball != null){
            Collide(ball);
        }
        
    }

    private void OnDisable()
    {
        paddleLogic.OnDisable();
    }
}
