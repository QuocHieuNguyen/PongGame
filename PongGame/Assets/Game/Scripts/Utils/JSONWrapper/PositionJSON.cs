using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PositionJSON
{
    public float[] position;

    public PositionJSON(Vector3 _position)
    {
        position = new float[] { _position.x, _position.y, _position.z };
    }
    public static PositionJSON CreateFromJSON(string data)
    {
        return JsonUtility.FromJson<PositionJSON>(data);
    }
}