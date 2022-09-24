using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMonoBehavior : MonoBehaviour, IPositionAdapter
{
    private PaddleInput paddleInput;
    private PaddleSimulation paddleSimulation;
    private PaddleLogic paddleLogic;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
}
