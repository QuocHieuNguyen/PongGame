using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneManagementSystem.Instance.LoadLevel(SceneList.LOGIN, (value)=> {

        });
    }

}
