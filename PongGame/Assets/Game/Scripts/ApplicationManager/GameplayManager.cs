using System;
using System.Collections;
using System.Collections.Generic;
using SocketIO;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    private SocketInterface socketReference;
    [SerializeField] private PaddleMonoBehavior paddlePrefab;
    public PaddleMonoBehavior paddleHost, paddleOpponent;

    [SerializeField] private GoalMonoBehavior hostGoal, opponentGoal;
    public Transform controllingPlayerPosition, opponentPosition;
    public SocketInterface SocketReference
    {
        get
        {
            return socketReference = (socketReference == null) ? FindObjectOfType<SocketInterface>() : socketReference;
        }
    }

    private void Start()
    {
        InitGame();
    }

    public void InitGame()
    {
        paddleHost = Instantiate(paddlePrefab, controllingPlayerPosition.position, Quaternion.identity);
        paddleOpponent = Instantiate(paddlePrefab, opponentPosition.position, Quaternion.identity);
        if (SocketReference.isHost)
        {
            paddleHost.SetHasAuthority(true);
            paddleOpponent.SetHasAuthority(false);
            hostGoal.Init(1);
            opponentGoal.Init(0);
        }
        else
        {
            paddleHost.SetHasAuthority(false);
            paddleOpponent.SetHasAuthority(true);
            hostGoal.Init(0);
            opponentGoal.Init(1);
        }

        paddleHost.Init();
        paddleOpponent.Init();
        SceneManagementSystem.Instance.MoveObjectToScene(paddleHost.gameObject, SceneList.GAME_PLAY);
        SceneManagementSystem.Instance.MoveObjectToScene(paddleOpponent.gameObject, SceneList.GAME_PLAY);
    }
}