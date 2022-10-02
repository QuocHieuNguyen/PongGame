using System.Collections.Generic;
using System.Collections;
using SocketIO;
using UnityEngine.UI;
using UnityEngine;
public class LoginHandler : MonoBehaviour
{
    [SerializeField] private InputField inputField;

    private SocketIOComponent socketReference;

    public SocketIOComponent SocketReference
    {
        get
        {
            return socketReference = (socketReference == null) ? FindObjectOfType<SocketIOComponent>() : socketReference;
        }
    }

    public void Connect()
    {
        SocketReference.Connect();
        EmitEventDelay.Instance.ExecuteEventDelay((() =>
        {
            SocketReference.Emit("setName", inputField.text);
            SceneManagementSystem.Instance.LoadLevel(SceneList.LOBBY, (value) =>
            {
                SceneManagementSystem.Instance.UnLoadLevel(SceneList.LOGIN);
            
            });
        }));

    }
}