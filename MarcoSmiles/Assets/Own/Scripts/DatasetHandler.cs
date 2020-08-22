using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Leap;

public class DatasetHandler : MonoBehaviour
{
    #region Getters
    public static float getFF(Finger f)
    {
        return grads(f.bones[0].Direction.AngleTo(f.bones[3].Direction));
    }

    public static float getNFA(Finger f1, Finger f2)
    {
        return grads(f1.Direction.AngleTo(f2.Direction));
    }


    #region FPA

    /// <summary>
    /// NON FUNZIONA BENEEEEEE
    /// </summary>
    /// <param name="h1">MANOOOOO</param>
    /// <param name="f1">DITOOOOOOO</param> 
    public static float getFPA(Hand h1, Finger f1)
    {
        var segment = h1.Fingers[0].Direction;

        var palmSegment = h1.PalmPosition;

        //Debug.Log($"PALM SEGMENT: {palmSegment.ToString()}");

        Debug.Log($"VETTORE DIREZZZIONE DI UN DITO: {h1.Fingers[1].Direction.ToString()}");

        //return grads(h1.Direction.AngleTo(f1.Direction));
        return 0.0f;
    }
    
    #endregion

    #endregion


    private static float grads(float num)
    {
        return num * 180.0f / Mathf.PI;
    }

}
