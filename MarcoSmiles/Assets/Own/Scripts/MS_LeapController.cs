using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leap;



/// <summary>
///     CLASS CREATED FOR MARCO_SMILES
/// </summary>
public class MS_LeapController : MonoBehaviour
{
    Controller controller;
    MS_LeapListener listener;

    // Start is called before the first frame update
    void Start()
    {

        controller = new Controller();

        controller.Device += MS_LeapListener.OnLeapConnect;
        //controller.FrameReady += MS_LeapListener.OnFrame;


    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (controller.IsServiceConnected)
        {
            Debug.Log("connesso");
        }

        if (controller.IsConnected)
        {
            Debug.Log("lollete");
        }
        */
    }

}
