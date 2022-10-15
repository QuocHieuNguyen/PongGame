using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIManager : MonoBehaviour
{
    [SerializeField]private GameObject UI_NotificationResult;

    [SerializeField]private TextMeshProUGUI textMeshProUGUI;

    [SerializeField]private Button leaveButton;

    public void EnableResult()
    {
        UI_NotificationResult.SetActive(true);
    }

    public void SetResultText(string text)
    {
        textMeshProUGUI.text = text;
    }

    public void AddListener(Action callback)
    {
        leaveButton.onClick.AddListener((() =>
                {
                    callback();
                }
                ));
    }
}
