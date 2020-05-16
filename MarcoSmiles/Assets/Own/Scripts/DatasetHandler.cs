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
        return grads(h1.Direction.AngleTo(f1.Direction));
    }
    
    #endregion

    #endregion


    private static float grads(float num)
    {
        return num * 180.0f / Mathf.PI;
    }

}
