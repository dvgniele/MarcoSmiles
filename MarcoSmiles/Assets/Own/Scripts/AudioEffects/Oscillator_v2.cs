using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Oscillator_v2 : MonoBehaviour
{
    SawWave sawAudioWave;
    SquareWave squareAudioWave;
    SinusWave sinusAudioWave;

    private int sampleRate;  // samples per second
    private double dataLen;     // the data length of each channel
    double chunkTime;
    double dspTimeStep;
    double currentDspTime;

    private float nextOutput;

    // Waveshape output weights
    [Range(0f, 1f)]
    public double sinWeight;
    [Range(0f, 1f)]
    public double sqrWeight;
    [Range(0f, 1f)]
    public double sawWeight;

    //waves actual output
    private float sinOutput;
    private float sawOutput;
    private float sqrOutput;

    //general volume of the oscillators, the output is moltiplied by this value
    [Range(0f, 1f)]
    public float volume = 0.5f;


    // Manage Frequencies and notes 
    private float[] frequencies = new float[] {
                        261.630f , 277.180f , 293.660f , 311.130f , 329.630f , 349.990f ,
                        369.990f , 392.000f , 415.300f , 440.000f , 466.160f , 493.880f };

    private float octave = 1f;

    [Range(0,11)]
    public int freqIndex;

    void Awake()
    {
        sawAudioWave = new SawWave();
        squareAudioWave = new SquareWave();
        sinusAudioWave = new SinusWave();

        sampleRate = AudioSettings.outputSampleRate;

    }

    void Start()
    {
        //Initial Preset. You can eventually save different presets in a similar way.
        sinWeight = 0.75;
        sqrWeight = 0.25;
        sawWeight = 0.25;

        nextOutput = 0;
        sinOutput = 0;
        sawOutput = 0;
        sqrOutput = 0;
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        /* 
         * This is "the current time of the audio system", as given
         * by Unity. It is updated every time the OnAudioFilterRead() function
         * is called. It's usually every 1024 samples.
         * 
         * A note on the sample rate:
         * We don't actually see real numbers for the sample rate, we instead
         * read it from the system in the Start() function.
         */

        currentDspTime = AudioSettings.dspTime;
        dataLen = data.Length / channels;   // the actual data length for each channel
        chunkTime = dataLen / sampleRate;   // the time that each chunk of data lasts
        dspTimeStep = chunkTime / dataLen;  // the time of each dsp step. (the time that each individual audio sample (actually a float value) lasts)

        double preciseDspTime;
  
        for (int i = 0; i < dataLen; i++) // go through the data buffer
        {
            preciseDspTime = currentDspTime + i * dspTimeStep;

            float currentFreq = frequencies[freqIndex];

            sinOutput = (float)(sinWeight * sinusAudioWave.calculateSignalValue(preciseDspTime, currentFreq));

            sawOutput = (float)(sawWeight * sawAudioWave.calculateSignalValue(preciseDspTime, currentFreq));

            sqrOutput = (float)(sqrWeight * squareAudioWave.calculateSignalValue(preciseDspTime, currentFreq));


            nextOutput = sinOutput + sawOutput + sqrOutput - (sinOutput * sawOutput) -
                                    (sinOutput * sqrOutput) - (sawOutput * sqrOutput) + (sinOutput * sawOutput * sqrOutput);


            float x = volume * (float)nextOutput;
            //writes the sample on the available channels
            for (int j = 0; j < channels; j++)
            {
                data[i * channels + j] = x;
            }
        }
    }
}



