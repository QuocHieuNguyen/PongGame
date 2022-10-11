using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMonoBehavior : MonoBehaviour
{
    private WallLogic wallLogic;
    // Start is called before the first frame update
    void Start()
    {
        wallLogic = new WallLogic(1);
    }
    private void OnCollisionEnter(Collision other) {
        Debug.Log(other.gameObject.name);
    }
}
