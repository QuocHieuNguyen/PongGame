using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class PaddleMonoBehavior : MonoBehaviour, IPositionAdapter
{
    public PaddleData paddleData;
    [SerializeField] private string inputAxisName;
    private PaddleInput paddleInput;
    private PaddleSimulation paddleSimulation;
    private PaddleLogic paddleLogic;

    private PaddleNetwork paddleNetwork;

    private bool isControlling =false;
    // Start is called before the first frame update
    void Awake()
    {

    }

    public void Init()
    {
        paddleInput = new PaddleInput(inputAxisName);
        paddleSimulation = new PaddleSimulation();
        paddleNetwork = new PaddleNetwork(FindObjectOfType<SocketIOComponent>(), isControlling);
        paddleLogic = new PaddleLogic(this, paddleData, paddleInput, paddleSimulation, paddleNetwork);
    }
    public void SetControlling(bool isControlling)
    {
        this.isControlling = isControlling;
        if (!isControlling)
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

    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
}
