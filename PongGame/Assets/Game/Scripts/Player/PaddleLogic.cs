using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleLogic
{
    private readonly IPositionAdapter positionAdapter;
    private readonly PaddleData paddleData;
    private readonly PaddleInput paddleInput;
    private readonly PaddleSimulation paddleSimulation;

    private readonly PaddleNetwork paddleNetwork;

    public PaddleLogic(IPositionAdapter positionAdapter, 
    PaddleData paddleData, 
    PaddleInput paddleInput, 
    PaddleSimulation paddleSimulation,
    PaddleNetwork paddleNetwork)
    {
        this.positionAdapter = positionAdapter;
        this.paddleData = paddleData;
        this.paddleInput = paddleInput;
        this.paddleSimulation = paddleSimulation;
        this.paddleNetwork = paddleNetwork;
        paddleNetwork.onUpdatePosition += UpdatePositionAdapter;
    }

    public void Update(float deltaTime)
    {
        if (!paddleNetwork.GetIsControlling())
        {
            return;
        }
        float inputAxisReading = paddleInput.ReadInput();
        float yPos = paddleSimulation.UpdatePosition(inputAxisReading, paddleData.MovementSpeedScaleFactor, deltaTime);

        Vector3 curPos = positionAdapter.Position;
        Vector3 newPos = new Vector3(curPos.x, yPos * paddleData.PositionScale, curPos.z);
        if (newPos != curPos)
        {
            Debug.Log("Send Data");
            paddleNetwork.SendData(curPos.x, yPos * paddleData.PositionScale);
        }
        
    }
    private void UpdatePositionAdapter(float x, float y){
        Vector3 curPos = positionAdapter.Position;
        positionAdapter.Position = new Vector3(curPos.x, y * paddleData.PositionScale, curPos.z);;
    }
}
