using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using SimpleJSON;
using System;
public class LobbyPanel : MonoBehaviour
{
    private SocketIOComponent socketReference;
    [SerializeField] private Transform roomButtonParent;
    [SerializeField] private RoomButton roomButton;

    public SocketIOComponent SocketReference
    {
        get
        {
            return socketReference = (socketReference == null) ? FindObjectOfType<SocketIOComponent>() : socketReference;
        }
    }
    private void OnEnable() {
        SocketReference.On("onDisplayLobbyData", OnFetchAllLobbyDataCallback);
        //GetLobbyData();
    }
    public void RefreshLobby(){
        SocketReference.Emit("fetchAllLobbyData");
    }
    public void GetLobbyData(){
        SocketReference.Emit("fetchAllLobbyData");
    }
    private void OnFetchAllLobbyDataCallback(SocketIOEvent socketEvent){
        Debug.Log(socketEvent.data["lobbyArray"].Count);
        //JSONNode jsonData = JSON.Parse(System.Text.Encoding.UTF8.GetString(socketEvent.data));

    }
    private void OnDisable() {
        
    }

}
