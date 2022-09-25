using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMonoBehavior : MonoBehaviour, IPositionAdapter
{
    public PaddleData paddleData;
    [SerializeField] private string inputAxisName;
    private PaddleInput paddleInput;
    private PaddleSimulation paddleSimulation;
    private PaddleLogic paddleLogic;
    // Start is called before the first frame update
    void Awake()
    {
        paddleInput = new PaddleInput(inputAxisName);
        paddleSimulation = new PaddleSimulation();
        paddleLogic = new PaddleLogic(this, paddleData, paddleInput, paddleSimulation);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        paddleLogic.Update(Time.deltaTime);
    }

    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
}
