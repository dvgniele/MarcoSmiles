﻿using UnityEngine;

using Leap;


/// <summary>
///  Gestisce la connessione del LeapMotion a Unity3d.
/// </summary>
public class MS_LeapListener : MonoBehaviour
{
    public static bool Connected = false;

    /// <summary>
    /// Evento attivato all'esecuzione, se il leap rileva le mani.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public static void OnLeapConnect(object sender, DeviceEventArgs args)
    {
        //  stampa un messaggio di log
        Debug.Log("Connesso");
        Connected = true;
    }
    /// <summary>
    /// Evento attivato se il Leap Moton viene disconnesso
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public static void OnLeapDisconnect(object sender, DeviceEventArgs args)
    {
        //  stampa un messaggio di log
        Debug.Log("DISCONNESSO");
        Connected = false;
    }

    /// <summary>
    /// Rileva frame
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public static void OnFrame(object sender, FrameEventArgs args)
    {

        Frame frame = args.frame;

        //  Se ci sono delle mani rilevate
        if(frame.Hands.Count > 1)
        {
            _GM.isActive = true;            //  deve suonare
            //  qui attivi

            //  per ogni mano, sceglie se è dx o sx, e stampa i seguenti dati
            //  DATI GENERICI:          dx o sx, tupla di coords (x,y,z) dal sensore leap, numero dita,
            //  ROTAZIONI relative:     rotazione su asse-x (hand pitch), rotazione su asse-z (hand roll), rotazione su asse-y (hand yaw)
            foreach (var hand in frame.Hands)
            {
                //  seleziona se è la mano destra

                if (hand.IsRight)
                    _GM.hand_R = hand;
                else if (hand.IsLeft)
                    _GM.hand_L = hand;
            }
        }
        else
        {
            //  non ci sono delle mani rilevate
            //  qui disattivi
            _GM.isActive = false;           //  non deve suonare
        }

    }
}
