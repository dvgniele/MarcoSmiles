﻿using UnityEngine;

using Leap;

/// <summary>
/// Classe che calcola le features FF ed NFA
/// </summary>
public class DatasetHandler : MonoBehaviour
{
    #region Getters

    /// <summary>
    /// Analizza la flessione di un dito
    /// </summary>
    /// <param name="f">Dito da analizzare</param>
    /// <param name="isThumb">vero se è pollice, falso altrimenti</param>
    /// <returns></returns>
    public static float getFF(Finger f, bool isThumb = false)
    {
        return (isThumb) ? grads(f.bones[1].Direction.AngleTo(f.bones[3].Direction)) :
            grads(f.bones[0].Direction.AngleTo(f.bones[3].Direction));
    }

    /// <summary>
    /// Analizza l'angolo tra le coppie di dita
    /// </summary>
    /// <param name="f1">Dito 1</param>
    /// <param name="f2">Dito 2</param>
    /// <returns></returns>
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

        //Debug.Log($"VETTORE DIREZZZIONE DI UN DITO: {h1.Fingers[1].Direction.ToString()}");

        //return grads(h1.Direction.AngleTo(f1.Direction));
        return 0.0f;
    }

    #endregion

    #endregion

    /// <summary>
    /// Converte un float in gradianti
    /// </summary>
    /// <param name="num">valore da convertire</param>
    /// <returns>Valore in gradianti</returns>
    private static float grads(float num)
    {
        return num * 180.0f / Mathf.PI;
    }

}
