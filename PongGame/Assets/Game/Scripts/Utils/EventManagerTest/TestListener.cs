using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestListener : MonoBehaviour
{
    private Action<object> onTest;

    private void Awake()
    {
        onTest += OnTest;
    }

    private void OnEnable()
    {
        EventManager.StartListening("Test", onTest);
    }

    private void OnDisable()
    {
        EventManager.StopListening("Test", onTest);
    }

    private void OnTest(object obj)
    {
        Debug.Log("received test!");
    }
}