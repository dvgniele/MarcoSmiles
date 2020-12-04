using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCMOscillator : MonoBehaviour
{
    public double frequency = 440;
    private double increment;
    private double phase;
    private double sampling_frequency = 48000.0;

    public float gain;
    public float volume = 0.4f;



    // Start is called before the first frame update
    void Start()
    {
        if (_GM.isActive)
        {
            gain = volume;
        }
        else
        {
            gain = 0;           //in teoria -inf
        }
        changeNote(_GM.indexPlayingNote);
    }

    // Update is called once per frame
    void Update()
    {
        if (_GM.isActive)
        {
            gain = volume;
        }
        else
        {
            gain = 0;           //in teoria -inf
        }

        changeNote(_GM.indexPlayingNote);
        //Debug.Log(frequency);

    }

    void changeNote(int noteIndex)
    {
        switch (noteIndex)                  //sostituire con qualche design pattern?
        {
            case 0:     //C4
                frequency = 261.630f;
                break;
            case 1:     //C#4
                frequency = 277.180f;
                break;
            case 2:     //D4
                frequency = 293.660f;
                break;
            case 3:     //D#4
                frequency = 311.130f;
                break;
            case 4:     //E3
                frequency = 329.630f;
                break;
            case 5:     //F4
                frequency = 349.990f;
                break;
            case 6:     //F#4
                frequency = 369.990f;
                break;
            case 7:     //G4
                frequency = 392.000f;
                break;
            case 8:     //G#4
                frequency = 415.300f;
                break;
            case 9:     //A4
                frequency = 440.000f;
                break;
            case 10:     //A#4
                frequency = 466.160f;
                break;
            case 11:     //B4
                frequency = 493.880f;
                break;

            default:
                Debug.Log("Default case");
                break;
        }

    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        increment = frequency * 2.0 * Mathf.PI / sampling_frequency;

        for (int i = 0; i < (data.Length); i += channels)
        {

            phase += increment;

            data[i] = (float)(gain * Mathf.Sin((float)phase));     //SINEWAVE 


            if (channels == 2)
            {
                data[i + 1] = data[i];
            }

            if (phase > (Mathf.PI * 2))
            {
                phase = 0.0;
            }
        }
    }

}
