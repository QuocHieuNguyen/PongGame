using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using SimpleJSON;
using System;
using UnityEngine.UI;

public class LobbyPanel : MonoBehaviour
{
    private SocketIOComponent socketReference;
    [SerializeField] private Text nameText; 
    [SerializeField] private Transform roomButtonParent;
    [SerializeField] private RoomButton roomButton;
    [SerializeField] private List<RoomButton> _roomButtons;

    public SocketIOComponent SocketReference
    {
        get
        {
            return socketReference = (socketReference == null) ? FindObjectOfType<SocketIOComponent>() : socketReference;
        }
    }
    private void OnEnable() {
        SocketReference.On("onDisplayLobbyData", OnFetchAllLobbyDataCallback);
        SocketReference.On("GetName", SetName);
        StartCoroutine(GetLobbyDataDelay(GetLobbyData));
        StartCoroutine(GetLobbyDataDelay(RequestGetName));
    }
    public void RefreshLobby(){
        SocketReference.Emit("fetchAllLobbyData");
    }
    public void GetLobbyData(){
        SocketReference.Emit("fetchAllLobbyData");
    }
    private void OnFetchAllLobbyDataCallback(SocketIOEvent socketEvent){
        Debug.Log(socketEvent.data["lobbyArray"].Count);
        for (int i = 0; i < socketEvent.data["lobbyArray"].Count; i++)
        {
            RoomButton instance = Instantiate(roomButton, roomButtonParent);
            instance.SetText(socketEvent.data["lobbyArray"][i].ToString());
            string onJoinSpecificLobbyPayload = socketEvent.data["lobbyArray"][i].ToString();
            instance.AddListener((() =>
            {
                Debug.Log("Join " + onJoinSpecificLobbyPayload);
                SocketReference.Emit("joinSpecificLobby", onJoinSpecificLobbyPayload);
            }));
            _roomButtons.Add(instance);
        }
        //JSONNode jsonData = JSON.Parse(System.Text.Encoding.UTF8.GetString(socketEvent.data));

    }

    private IEnumerator GetLobbyDataDelay(Action postDelayFunction)
    {
        yield return new WaitForSeconds(0.2f);
        postDelayFunction();
    }

    private void RequestGetName()
    {
        SocketReference.Emit("getName");
    }
    private void SetName(SocketIOEvent socketEvent)
    {
        nameText.text = socketEvent.data.ToString();
    }
    private void OnDisable() {
        
    }

}
