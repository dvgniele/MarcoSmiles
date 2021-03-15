using UnityEngine;

using Leap;



/// <summary>
/// Gestisce interfacciamento tra Unity3D e LeapMotion
/// </summary>
public class MS_LeapController : MonoBehaviour
{
    Controller controller;
    MS_LeapListener listener;

    bool connected = false;
    bool notified = false;
    bool notConnectedShowed = true;

    // Start is called before the first frame update
    void Start()
    {
        //  inizializza un novo controller per il leap
        controller = new Controller();

        //  assegna il device quando un sensore leap motion è connesso
        controller.Device += MS_LeapListener.OnLeapConnect;

        _GM.ShowConnectLeapPopup();
    }

    // Update is called once per frame
    void Update()
    {
        if(controller.Devices.Count == 0 && !notConnectedShowed)
        {
            notified = false;
            notConnectedShowed = true;
            MS_LeapListener.Connected = false;
            _GM.ShowConnectLeapPopup();
        }

        //  ascolta ogni frame del leap motion
        if (controller.Devices.Count > 0 && !connected)
        {
            controller.FrameReady += MS_LeapListener.OnFrame;
            connected = true;
        }

        if (MS_LeapListener.Connected && !notified)
        {
            notified = true;
            notConnectedShowed = false;
            _GM.HideConnectLeapPopup();
        }
    }

}
