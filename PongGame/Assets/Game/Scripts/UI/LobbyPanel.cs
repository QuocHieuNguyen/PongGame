using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using SimpleJSON;
using System;
using UnityEngine.UI;

public class LobbyPanel : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Transform roomButtonParent;
    [SerializeField] private RoomButton roomButton;
    [SerializeField] private List<RoomButton> _roomButtons;
    private SocketIOComponent socketReference;

    public SocketIOComponent SocketReference
    {
        get
        {
            return socketReference =
                (socketReference == null) ? FindObjectOfType<SocketIOComponent>() : socketReference;
        }
    }

    private void OnEnable()
    {
        SubscribeToEvent();
        InvokeEventToNetworkClient();
    }

    private void OnDisable()
    {
    }

    private void InvokeEventToNetworkClient()
    {
        EmitEventDelay.Instance.ExecuteEventDelay(GetLobbyData);
        EmitEventDelay.Instance.ExecuteEventDelay(RequestGetName);
    }

    private void SubscribeToEvent()
    {
        SocketReference.On("onDisplayLobbyData", OnFetchAllLobbyDataCallback);
        SocketReference.On("GetName", SetName);
        SocketReference.On("OnJoinSpecificLobby", ChangeToRoomScene);

        SocketReference.On("hostSucceed", HostRoomScene);
    }

    public void RefreshLobby()
    {
        SocketReference.Emit("fetchAllLobbyData");
    }

    public void GetLobbyData()
    {
        SocketReference.Emit("fetchAllLobbyData");
    }

    public void HostGame()
    {
        SocketReference.Emit("hostGame");
    }

    private void OnFetchAllLobbyDataCallback(SocketIOEvent socketEvent)
    {
        Debug.Log(socketEvent.data["lobbyArray"].Count);
        for (int i = 0; i < _roomButtons.Count; i++)
        {
            UnityEngine.Object.Destroy(_roomButtons[i].gameObject);
        }

        //_roomButtons.Clear();
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

    private void ChangeToRoomScene(SocketIOEvent socketEvent)
    {
        if (socketEvent.data["response_code"].ToString() == "-1")
        {
            Debug.Log("Switch room");
            ChangeRoom();
        }
    }

    private void HostRoomScene(SocketIOEvent socketIOEvent)
    {
        Debug.Log("Room is loaded");
        ChangeRoom();
    }

    void ChangeRoom()
    {
        LoadScene(SceneList.ROOM);
    }

    public void LeaveRoom()
    {
        SocketReference.Close();
        LoadScene(SceneList.LOGIN);
    }

    void LoadScene(string sceneName)
    {
        EmitEventDelay.Instance.ExecuteEventDelay((() =>
        {
            SceneManagementSystem.Instance.LoadLevel(sceneName, (value) =>
            {
                SocketReference.Off("onDisplayLobbyData");
                SocketReference.Off("GetName");
                SocketReference.Off("OnJoinSpecificLobby");
                SocketReference.Off("hostSucceed");
                SceneManagementSystem.Instance.UnLoadLevel(SceneList.LOBBY);
            });
        }));
    }

    private void RequestGetName()
    {
        SocketReference.Emit("getName");
    }

    private void SetName(SocketIOEvent socketEvent)
    {
        string value = socketEvent.data["name"].ToString(false);
        nameText.text = value;
    }
}