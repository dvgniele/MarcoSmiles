using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Leap;

public class DatasetHandler : MonoBehaviour
{
    public static float getFF(Finger f)
    {
        return grads(f.bones[0].Direction.AngleTo(f.bones[2].Direction));
    }

    public static float getNFA(Finger f1, Finger f2)
    {
        return grads(f1.Direction.AngleTo(f2.Direction));
    }


    private static float grads(float num)
    {
        return num * 180.0f / Mathf.PI;
    }

}
