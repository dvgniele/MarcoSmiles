using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leap;


/// <summary>
///     CLASS CREATED FOR MARCO_SMILES
/// </summary>
public class MS_LeapListener : MonoBehaviour
{
    /// <summary>
    /// Evento attivato all'esecuzione, se il leap rileva le mani.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public static void OnLeapConnect(object sender, DeviceEventArgs args)
    {
        //  stampa un messaggio di log
        Debug.Log("Connesso");
    }

    /// <summary>
    /// rileva frame
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public static void OnFrame(object sender, FrameEventArgs args)
    {
        //Debug.Log("Frame disponibile");

        Frame frame = args.frame;

        Debug.Log($"" +
            $"Frame id: {frame.Id}," +
            $"timestamp: {frame.Timestamp}," +
            $" hands: {frame.Hands.Count}");


        //  per ogni mano, sceglie se è dx o sx, e stampa i seguenti dati
        //  DATI GENERICI:          dx o sx, tupla di coords (x,y,z) dal sensore leap, numero dita,
        //  ROTAZIONI relative:     rotazione su asse-x (hand pitch), rotazione su asse-z (hand roll), rotazione su asse-y (hand yaw)
        foreach (var hand in frame.Hands)
        {
            var handType = hand.IsRight ? "Right Hand" : "Left Hand";

            //Debug.Log($"{handType}, Hand id: {hand.Id}, palm position: {hand.PalmPosition}");

            Vector normal = hand.PalmNormal;
            Vector direction = hand.Direction;
            Debug.Log($"{handType},   Hand id: {hand.Id},   Palm position: {hand.PalmPosition}," +
                $"      Hand pitch: {direction.Pitch * 180.0f / (float)Mathf.PI} degrees," +
                $"      Hand Roll: {normal.Roll * 180.0f / (float)Mathf.PI} degrees," +
                $"      Hand Yaw: {direction.Yaw * 180.0f / (float)Mathf.PI} degrees" +
                $"      ");

            //var kek = hand.Fingers[1];

            // hand.Fingers[3].bones[2].Direction.Yaw


        }
    }
}
