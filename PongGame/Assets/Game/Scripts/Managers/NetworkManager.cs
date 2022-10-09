using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class NetworkManager : MonoBehaviour
{
    public SocketIOComponent socket;
    public static NetworkManager instance;
    public PlayerInput localPlayer;
    public PlayerController otherPlayer;
    public string name = "hieu";
    
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        socket.On("play", OnPlay);
        socket.On("player move", OnPlayerMove);
        socket.On("other player connected", OnOtherPlayerConnected);
        socket.On("other player move", OnOtherPlayerMove);

    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UserJSON userJson = new UserJSON(name);
            string data = JsonUtility.ToJson(userJson);
            socket.Emit("play", new JSONObject(data));
        }
    }

    void OnPlay(SocketIOEvent socketIOEvent)
    {
        Debug.Log("you joined");
        string data = socketIOEvent.data.ToString();
        int clientLength = int.Parse(socketIOEvent.data[0].ToString());
        Debug.Log(clientLength);
        if (clientLength > 0)
        {
            otherPlayer.gameObject.SetActive(true);
        }
        /*
        string data = socketIOEvent.data.ToString();
        UserJSON currentUserJSON = UserJSON.CreateFromJSON(data);
        Vector3 position = new Vector3(currentUserJSON.position[0], currentUserJSON.position[1], currentUserJSON.position[2]);
        Quaternion rotation = Quaternion.Euler(currentUserJSON.rotation[0], currentUserJSON.rotation[1], currentUserJSON.rotation[2]);
        GameObject p = Instantiate(player, position, rotation) as GameObject;
        PlayerController pc = p.GetComponent<PlayerController>();
        Transform t = p.transform.Find("Healthbar Canvas");
        Transform t1 = t.transform.Find("Player Name");
        Text playerName = t1.GetComponent<Text>();
        playerName.text = currentUserJSON.name;
        pc.isLocalPlayer = true;
        p.name = currentUserJSON.name;
        */

    }
    public void CommandMove(Vector3 vec3)
    {
        string data = JsonUtility.ToJson(new PositionJSON(vec3));
        socket.Emit("player move", new JSONObject(data));
    }
    void OnPlayerMove(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        PositionJSON positionJson = PositionJSON.CreateFromJSON(data);
        localPlayer.transform.position = new Vector3(positionJson.position[0], positionJson.position[1], positionJson.position[2]);
        /*string data = socketIOEvent.data.ToString();
        UserJSON userJSON = UserJSON.CreateFromJSON(data);
        Vector3 position = new Vector3(userJSON.position[0], userJSON.position[1], userJSON.position[2]);
        // if it is the current player exit
        if (userJSON.name == playerNameInput.text)
        {
            return;
        }
        GameObject p = GameObject.Find(userJSON.name) as GameObject;
        if (p != null)
        {
            p.transform.position = position;
        }*/
    }
    void OnOtherPlayerConnected(SocketIOEvent socketIOEvent)
    {
        print("Someone else joined");
        string data = socketIOEvent.data.ToString();
        otherPlayer.gameObject.SetActive(true);
        /*UserJSON userJSON = UserJSON.CreateFromJSON(data);
        Vector3 position = new Vector3(userJSON.position[0], userJSON.position[1], userJSON.position[2]);
        Quaternion rotation = Quaternion.Euler(userJSON.rotation[0], userJSON.rotation[1], userJSON.rotation[2]);
        GameObject o = GameObject.Find(userJSON.name) as GameObject;
        if (o != null)
        {
            return;
        }
        GameObject p = Instantiate(player, position, rotation) as GameObject;
        // here we are setting up their other fields name and if they are local
        PlayerController pc = p.GetComponent<PlayerController>();
        Transform t = p.transform.Find("Healthbar Canvas");
        Transform t1 = t.transform.Find("Player Name");
        Text playerName = t1.GetComponent<Text>();
        playerName.text = userJSON.name;
        pc.isLocalPlayer = false;
        p.name = userJSON.name;
        // we also need to set the health
        Health h = p.GetComponent<Health>();
        h.currentHealth = userJSON.health;
        h.OnChangeHealth();*/
    }

    void OnOtherPlayerMove(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        PositionJSON positionJson = PositionJSON.CreateFromJSON(data);
        otherPlayer.transform.position = new Vector3(otherPlayer.transform.position.x, positionJson.position[1], positionJson.position[2]);
    }
}
