using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitEventDelay : Singleton<EmitEventDelay>
{
    public void ExecuteEventDelay(Action callback, float time = 0.2f)
    {
        StartCoroutine(ExecuteDelay(callback, time));
    }

    private IEnumerator ExecuteDelay(Action callback, float time)
    {
        yield return new WaitForSeconds(time);
        callback.Invoke();
    }
}


