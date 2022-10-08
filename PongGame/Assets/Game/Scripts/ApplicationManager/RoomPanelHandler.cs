using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

public class RoomPanelHandler : MonoBehaviour
{
    [SerializeField] private Text hostNameText;

    [SerializeField] private Text opponentNameText;
    private SocketIOComponent socketReference;

    public SocketIOComponent SocketReference
    {
        get
        {
            return socketReference = (socketReference == null) ? FindObjectOfType<SocketIOComponent>() : socketReference;
        }
    }

    private void OnEnable()
    {
        SubscribeToEvent();
        InvokeEventToNetworkClient();
    }
    private void SubscribeToEvent()
    {
        SocketReference.On("DisplayPlayerData", SetName);
        /*SocketReference.On("onDisplayLobbyData", OnFetchAllLobbyDataCallback);
        SocketReference.On("GetName", SetName);
        SocketReference.On("OnJoinSpecificLobby", ChangeToRoomScene);*/
    }
    private void InvokeEventToNetworkClient()
    {
        EmitEventDelay.Instance.ExecuteEventDelay(RequestGetLobbyPlayerName);
        //EmitEventDelay.Instance.ExecuteEventDelay(RequestGetName);
    }

    private void RequestGetLobbyPlayerName()
    {
        SocketReference.Emit("fetchPlayerDataInLobby");
    }

    private void SetName(SocketIOEvent socketIOEvent)
    {
        hostNameText.text = socketIOEvent.data["host_name"].ToString();
        opponentNameText.text = socketIOEvent.data["opponent_name"].ToString();
        Debug.Log(socketIOEvent.data);
    }
    public void ChangeScene(){
        Debug.Log("Change Scene");
        SceneManagementSystem.Instance.LoadLevel(SceneList.GAME_PLAY, (value)=>{
            //SceneManagementSystem.Instance.UnLoadLevel(SceneList.ROOM);
        });
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)){
            ChangeScene();
        }
    }
}
