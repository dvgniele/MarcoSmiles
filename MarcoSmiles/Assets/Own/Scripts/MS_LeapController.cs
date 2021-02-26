using UnityEngine;

using Leap;



/// <summary>
///     CLASS CREATED FOR MARCO_SMILES
/// </summary>
public class MS_LeapController : MonoBehaviour
{
    Controller controller;
    MS_LeapListener listener;

    bool connected = false;

    // Start is called before the first frame update
    void Start()
    {
        //  inizializza un novo controller per il leap
        controller = new Controller();

        //  assegna il device quando un sensore leap motion è connesso
        controller.Device += MS_LeapListener.OnLeapConnect;
        
    }

    // Update is called once per frame
    void Update()
    {
        //  ascolta ogni frame del leap motion
        if (controller.Devices.Count > 0 && !connected)
        {
            controller.FrameReady += MS_LeapListener.OnFrame;
            connected = true;
        }
    }

}
