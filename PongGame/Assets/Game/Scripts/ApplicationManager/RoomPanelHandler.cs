using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

public class RoomPanelHandler : MonoBehaviour
{
    public bool isHost = false;
    [SerializeField] private Text hostNameText;

    [SerializeField] private Text opponentNameText;
    [SerializeField] private Button startGameButton;
    private SocketInterface socketReference;

    public SocketInterface SocketReference
    {
        get
        {
            return socketReference = (socketReference == null) ? FindObjectOfType<SocketInterface>() : socketReference;
        }
    }

    private void OnEnable()
    {
        
        SubscribeToEvent();
        InvokeEventToNetworkClient();
    }
    private void SubscribeToEvent()
    {
        SocketReference.Off("DisplayPlayerData", SetName);
        SocketReference.Off("GameIsStarted", ChangeToGameplay);
        SocketReference.Off("OpponentEnterLobby", OnOpponentEnterLobby);
        
        
        SocketReference.On("DisplayPlayerData", SetName);
        SocketReference.On("GameIsStarted", ChangeToGameplay);
        SocketReference.On("OpponentEnterLobby", OnOpponentEnterLobby);

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
        SocketReference.isHost = false;
        hostNameText.text = socketIOEvent.data["host_name"].ToString();
        opponentNameText.text = socketIOEvent.data["opponent_name"].ToString();
        if (socketIOEvent.data["role"].ToString() != "host")
        {
            startGameButton.interactable = false;
            isHost = true;
        }
        Debug.Log(socketIOEvent.data);
    }
    public void RequestChangeScene(){
        
        SocketReference.Emit("startGame");
    }

    public void LeaveRoom()
    {
        SocketReference.Emit("leftRoom");
        ChangeScene(SceneList.LOBBY);
    }
    void ChangeToGameplay(SocketIOEvent socketIOEvent)
    {
        Debug.Log("Change Scene");
        SocketReference.isHost = true;
        ChangeScene(SceneList.GAME_PLAY);
    }

    void ChangeScene(string sceneName)
    {

        SceneManagementSystem.Instance.LoadLevel(sceneName, (value)=>{
            SceneManagementSystem.Instance.UnLoadLevel(SceneList.ROOM);
            
        });  
    }
    void OnOpponentEnterLobby(SocketIOEvent socketIOEvent){
        opponentNameText.text = socketIOEvent.data.ToString();
    }
}
