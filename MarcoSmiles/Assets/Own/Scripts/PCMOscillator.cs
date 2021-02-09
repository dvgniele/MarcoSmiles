using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCMOscillator : MonoBehaviour
{
    public float frequency = 440;
    private double increment;
    private double phase;
    private double sampling_frequency = 48000.0;

    public float gain;
    public float volume = 0.4f;

    private float[] frequencies = new float[] { 
                        261.630f , 277.180f , 293.660f , 311.130f , 329.630f , 349.990f ,
                        369.990f , 392.000f , 415.300f , 440.000f , 466.160f , 493.880f };

    // Start is called before the first frame update
    void Start()
    {
        if (_GM.isActive)
        {
            gain = volume;
        }
        else
        {
            gain = 0.0f;           //in teoria -inf
        }
        changeNote(_GM.indexPlayingNote);

    }

    // Update is called once per frame
    void Update()
    {
        if (_GM.isActive)
        {
            gain = volume;              /*questo valore si potrebbe moltiplicare per qualche valore scalato che rappresenti la distanza 
                                         * delle mani dal leap, così da dare espressività al suono
                                         * 
                                         */
        }
        else
        {
            gain = 0.0f;           //in teoria -inf
        }

        changeNote(_GM.indexPlayingNote);
        //Debug.Log(frequency);

    }

    void changeNote(int noteIndex)
    {
        switch (noteIndex)                  //sostituire con qualche design pattern?
        {
            case 0:     //C4
                frequency = frequencies[0];
                break;
            case 1:     //C#4
                frequency = frequencies[1];
                break;
            case 2:     //D4
                frequency = frequencies[2];
                break;
            case 3:     //D#4
                frequency = frequencies[3];
                break;
            case 4:     //E3
                frequency = frequencies[4];
                break;
            case 5:     //F4
                frequency = frequencies[5];
                break;
            case 6:     //F#4
                frequency = frequencies[6];
                break;
            case 7:     //G4
                frequency = frequencies[7];
                break;
            case 8:     //G#4
                frequency = frequencies[8];
                break;
            case 9:     //A4
                frequency = frequencies[9];
                break;
            case 10:     //A#4
                frequency = frequencies[10];
                break;
            case 11:     //B4
                frequency = frequencies[11];
                break;
            case 12:     //C5
                frequency = frequencies[0] * 2;
                break;
            case 13:     //C#5
                frequency = frequencies[1] * 2;
                break;
            case 14:     //D5
                frequency = frequencies[2] * 2;
                break;
            case 15:     //D#5
                frequency = frequencies[3] * 2;
                break;
            case 16:     //E5
                frequency = frequencies[4] * 2;
                break;
            case 17:     //F5
                frequency = frequencies[5] * 2;
                break;
            case 18:     //F#5
                frequency = frequencies[6] * 2;
                break;
            case 19:     //G5
                frequency = frequencies[7] * 2;
                break;
            case 20:     //G#5
                frequency = frequencies[8] * 2;
                break;
            case 21:     //A5
                frequency = frequencies[9] * 2;
                break;
            case 22:     //A#5
                frequency = frequencies[10] * 2;
                break;
            case 23:     //B5
                frequency = frequencies[11] * 2;
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
