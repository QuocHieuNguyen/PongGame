using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager gameManager;

    public static GameManager instance
    {
        get
        {
            if (!gameManager)
            {
                gameManager = FindObjectOfType (typeof (GameManager)) as GameManager;

                if (!gameManager)
                {
                    Debug.LogError ("There needs to be one active GameManager script on a GameObject in your scene.");
                }
                else
                {
                    
                }
            }

            return gameManager;
        }
    }
    #endregion



    private void Awake() {
       
    }
    public void Subscribe(string key, Action<object> value){
        //networkClient.Listen(key,value);
    }
    public void Invoke(string key, object value){
     //   networkClient.Invoke(key, value);
    }
    public void Unsubscribe(string key, Action<object> value){
     //   networkClient.StopListen(key, value);
    }
}
