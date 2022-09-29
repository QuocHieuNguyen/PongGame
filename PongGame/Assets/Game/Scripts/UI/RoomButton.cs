using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RoomButton : MonoBehaviour
{
    public Text labelText;
    public Button onClickButton;

    public void SetText(string value){
        labelText.text = value;
    }
    public void AddListener(Action callback){
        onClickButton.onClick.AddListener(()=>{
            callback.Invoke();
        });
    }
}
