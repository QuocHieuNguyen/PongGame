using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class GameplayManager : MonoBehaviour
{
    private SocketIOComponent socketReference;
    [SerializeField] private PaddleMonoBehavior paddlePrefab;
    public PaddleMonoBehavior paddleControllingPlayer, paddleOpponent;
    public Transform controllingPlayerPosition, opponentPosition;
    public SocketIOComponent SocketReference
    {
        get
        {
            return socketReference = (socketReference == null) ? FindObjectOfType<SocketIOComponent>() : socketReference;
        }
    }

    private void Start()
    {
        InitGame();
    }

    public void InitGame()
    {
        paddleControllingPlayer = Instantiate(paddlePrefab, controllingPlayerPosition.position, Quaternion.identity);
        paddleOpponent = Instantiate(paddlePrefab, opponentPosition.position, Quaternion.identity);
        paddleControllingPlayer.SetControlling(true);
        paddleOpponent.SetControlling(false);
        
        paddleControllingPlayer.Init();
        paddleOpponent.Init();
        SceneManagementSystem.Instance.MoveObjectToScene(paddleControllingPlayer.gameObject, SceneList.GAME_PLAY);
        SceneManagementSystem.Instance.MoveObjectToScene(paddleOpponent.gameObject, SceneList.GAME_PLAY);
    }
}
