using System.Collections;
using System.Collections.Generic;
using SocketIO;
using UnityEngine;
public struct InputPayload
{
    public int tick;
    public Vector3 inputVector;
}

public struct StatePayload
{
    public int tick;
    public Vector3 position;
}
public class ClientPrediction
{
  // Shared
    private float timer;
    private int currentTick;
    private float minTimeBetweenTicks;
    private const float SERVER_TICK_RATE = 30f;
    private const int BUFFER_SIZE = 1024;

    // Client specific
    private StatePayload[] stateBuffer;
    private InputPayload[] inputBuffer;
    private StatePayload latestServerState;
    private StatePayload lastProcessedState;

    //Network Property
    private SocketIOComponent _socketIOComponent;
    private Transform _transform;

    public ClientPrediction(SocketIOComponent socketIOComponent, Transform transform)
    {
        _socketIOComponent = socketIOComponent;
        _transform = transform;
        socketIOComponent.On("updateBallBufferState", OnServerMovementState);
        Init();
    }


    void Init()
    {
        minTimeBetweenTicks = 1f / SERVER_TICK_RATE;

        stateBuffer = new StatePayload[BUFFER_SIZE];
        inputBuffer = new InputPayload[BUFFER_SIZE];
    }

    public void Update()
    {
        
        timer += Time.deltaTime;

        while (timer >= minTimeBetweenTicks)
        {
            timer -= minTimeBetweenTicks;
            HandleTick();
            currentTick++;
        }
    }

    public void OnServerMovementState(StatePayload serverState)
    {
        latestServerState = serverState;
        Debug.Log("Update");
    }

    private void OnServerMovementState(SocketIOEvent socketIOEvent)
    {
        int tick = (int)socketIOEvent.data["tick"].f;
        float x = socketIOEvent.data["position"]["x"].f;
        float y = socketIOEvent.data["position"]["y"].f;

        StatePayload statePayload = new StatePayload()
        {
            tick = tick,
            position = new Vector3(x,y,0)
        };
        OnServerMovementState(statePayload);
    }

    void HandleTick()
    {
        if (!latestServerState.Equals(default(StatePayload)) &&
            (lastProcessedState.Equals(default(StatePayload)) ||
            !latestServerState.Equals(lastProcessedState)))
        {
            HandleServerReconciliation();
        }

        int bufferIndex = currentTick % BUFFER_SIZE;

        // Add payload to inputBuffer
        InputPayload inputPayload = new InputPayload();
        inputPayload.tick = currentTick;
        inputPayload.inputVector = _transform.position;
        inputBuffer[bufferIndex] = inputPayload;

        // Add payload to stateBuffer
        stateBuffer[bufferIndex] = ProcessMovement(inputPayload);

        // Send input to server
        _socketIOComponent.Emit("updateBallBufferInput", JsonUtility.ToJson(inputPayload));
       // StartCoroutine(SendToServer(inputPayload));
    }

    StatePayload ProcessMovement(InputPayload input)
    {
        // Should always be in sync with same function on Server
        _transform.position = input.inputVector  * minTimeBetweenTicks;

        return new StatePayload()
        {
            tick = input.tick,
            position = _transform.position,
        };
    }

    void HandleServerReconciliation()
    {
        lastProcessedState = latestServerState;

        int serverStateBufferIndex = latestServerState.tick % BUFFER_SIZE;
        float positionError = Vector3.Distance(latestServerState.position, stateBuffer[serverStateBufferIndex].position);

        if (positionError > 0.001f)
        {
            Debug.Log("We have to reconcile bro");
            // Rewind & Replay
            _transform.position = latestServerState.position;

            // Update buffer at index of latest server state
            stateBuffer[serverStateBufferIndex] = latestServerState;

            // Now re-simulate the rest of the ticks up to the current tick on the client
            int tickToProcess = latestServerState.tick + 1;

            while (tickToProcess < currentTick)
            {
                int bufferIndex = tickToProcess % BUFFER_SIZE;

                // Process new movement with reconciled state
                StatePayload statePayload = ProcessMovement(inputBuffer[bufferIndex]);

                // Update buffer with recalculated state
                stateBuffer[bufferIndex] = statePayload;

                tickToProcess++;
            }
        }
    }

    ~ClientPrediction()
    {
        _socketIOComponent.On("updateBallBufferState", OnServerMovementState);
    }
}
