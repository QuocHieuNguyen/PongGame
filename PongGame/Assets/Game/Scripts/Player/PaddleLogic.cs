using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleLogic
{
    private readonly IPositionAdapter positionAdapter;
    private readonly PaddleData paddleData;
    private readonly PaddleInput paddleInput;
    private readonly PaddleSimulation paddleSimulation;

    public PaddleLogic(IPositionAdapter positionAdapter, PaddleData paddleData, PaddleInput paddleInput, PaddleSimulation paddleSimulation)
    {
        this.positionAdapter = positionAdapter;
        this.paddleData = paddleData;
        this.paddleInput = paddleInput;
        this.paddleSimulation = paddleSimulation;
    }
}
