using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce il sintetizzatore e genera audio
/// Utilizza le Classi SawWave, SquareWave e  SinusWave,
/// Scrive i campionamenti nel motore audio di unity utilizzando MonoBehaviour.OnAudioFilterRead
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class ProceduralAudioOscillator : MonoBehaviour
{

    SawWave sawAudioWave;                       //  Classe per Saw Wave
    SquareWave squareAudioWave;                 //  Classe per Square Wave
    SinusWave sinusAudioWave;                   //  Classe per Sine Wave

    //For amplitude and frequency modulation
    SinusWave amplitudeModulationOscillator;        //  Oscillatore usato per Modulazione di Ampiezza
    SinusWave frequencyModulationOscillator;        //  Oscillatore usato per Modulazione di Frequenza


    private int sampleRate;                         //  Frequenza di campionamento usata da Unity
    private double dataLen;                         //  Numero di campionamenti in data per ogni canale 
    double chunkTime;                               //  La durata di ogni porzione di data  
    double dspTimeStep;                                 
    double currentDspTime;                          //  Il tempo corrente del sistema sonoro di Unity

    //  Pesi delle diverse forme d'onda sull'output 
    [Range(0f, 1f)]
    public double sinWeight;
    [Range(0f, 1f)]
    public double sqrWeight;
    [Range(0f, 1f)]
    public double sawWeight;

    //  Output per ogni forma d'onda (sarebbero i campionamenti effettuati)
    private float sinOutput;
    private float sawOutput;
    private float sqrOutput;

    ///The output of the synthesizer. This contains the mixed output between all the waveforms. (tsarebbero i campionamenti effettuati che poi vengono scritti nel motore audio)
    private float nextOutput;

    [Header("Amplitude Modulation")]
    public bool useAmplitudeModulation;             //  Boolean parameter that determines whether or not to apply amplitude modulation on the produced sound.
    [Range(0.2f, 30.0f)]
    public float amplitudeModulationOscillatorFrequency = 1.0f;     //  Float parameter that determines the Amplitude Modulation Oscillator’s frequency.

    [Header("Frequency Modulation")]
    public bool useFrequencyModulation;             //  Boolean Parameter that determines whether or not to apply frequency modulation on the produced sound.
    [Range(0.2f, 30.0f)]
    public float frequencyModulationOscillatorFrequency = 1.0f;         //  Float parameter that determines the Frequency Modulation Oscillator’s frequency.
    [Range(1.0f, 100.0f)]
    public float frequencyModulationOscillatorIntensity = 10.0f;        //  Float parameter that determines the Frequency Modulation Oscillator’s intensity.


    /*
     These parameters are for external use, only (they are calculated, based on the previous parameters and time-dependent functions). 
     So, actually they do not control the Synthesizer, but can be used to “drive” (control) other scripts’ parameters.
     */
    [Header("Out Values")]
    [Range(0.0f, 1.0f)]
    public float amplitudeModulationRangeOut;       //The Amplitude Modulation Oscillator’s current value (range 0 to 1)
    [Range(0.0f, 1.0f)]
    public float frequencyModulationRangeOut;       //The Frequency Modulation Oscillator’s current value (range 0 to 1)


    /* These control the amplitude of the general signal  */
    public float gain;
    //  general volume of the oscillators, the output is moltiplied by this value
    [Range(0f, 1f)]
    public float volume = 0;
    //  the value that is assigned to the variable volume
    [Range(0f, 1f)]
    public float volumeValue;

    //  The frequency that the synth is playing
    private float frequency = 440;
    //  Contains the frequencies of an octave starting from C4
    private float[] frequencies = new float[] {
                        261.630f , 277.180f , 293.660f , 311.130f , 329.630f , 349.990f ,
                        369.990f , 392.000f , 415.300f , 440.000f , 466.160f , 493.880f };
    //  Indicates the name of the octave that the user can play. It starts with C4
    public int octaveNumber = 4;


    void Awake()
    {
        sawAudioWave = new SawWave();
        squareAudioWave = new SquareWave();
        sinusAudioWave = new SinusWave();

        amplitudeModulationOscillator = new SinusWave();
        frequencyModulationOscillator = new SinusWave();

        // Gets the Sample Rate of the audio system in Unity
        sampleRate = AudioSettings.outputSampleRate;
        //Debug.Log(sampleRate);

        //  The value that is assigned to the variable volume, this is the actual volume of the synth
        volumeValue = 0.3f;                             //  Change this value to make it louder
        volume = volumeValue;
}


    void Start()
    {
        //Initial Preset. You can eventually save different presets in a similar way.
        sinWeight = 0.75;
        sqrWeight = 0.25;
        sawWeight = 0.25;

        //  Used to change the note playing by the synth
        changeNote(_GM.indexPlayingNote);
    }

    void Update()
    {
        //  Used to change the note playing by the synth
        changeNote(_GM.indexPlayingNote);
        //Debug.Log(frequency);
    }


    /// <summary>
    ///   Uses a switch case... In order to check on the index of the playing note and change the frequency of the oscillator 
    /// </summary>
    /// <param name="noteIndex"> The index of the playing note. This index is contained inside the game master (_GM)</param>
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



    /// <summary>
    ///  OnAudioFilterRead consente di intercettare ciò che l' audio source collegato a questo oggetto sta riproducendo. In questo modo possiamo modificare 
    ///  ciò che l'audio source sta riproducendo, sia scrivendo nuovi campionamenti all'interno, che modificando l'audio clip che l'audio source sta riproducendo.
    /// </summary>
    /// <param name="data">Buffer contenente i campionamenti dell'aduio source.. possiamo scrivere qui per far rispodurre suoni all'oggetto audiosource</param>
    /// <param name="channels"> Numero di canali disponibili dal motore audio unity. Per audio stereo, channels = 2.</param>
    void OnAudioFilterRead(float[] data, int channels)
    {
        if (_GM.isActive)
        {
            /* 
             * This is "the current time of the audio system", as given
             * by Unity. It is updated every time the OnAudioFilterRead() function
             * is called. It's usually every 1024 samples.
             * 
             */

        currentDspTime = AudioSettings.dspTime;     // the current time of the audio system     
        dataLen = data.Length / channels;           // the actual data length for each channel
        chunkTime = dataLen / sampleRate;           // the time that each chunk of data lasts
        dspTimeStep = chunkTime / dataLen;          // the time of each dsp step. (the time that each individual audio sample (actually a float value) lasts)

        double preciseDspTime;                      //  used to get a precise approximation of the time
        nextOutput = 0;
        sinOutput = 0;
        sawOutput = 0;
        sqrOutput = 0;

        for (int i = 0; i < dataLen; i++)               // go through data chunk
        {
            preciseDspTime = currentDspTime + i * dspTimeStep;      //  we calculate the current dsp time adding the time of every step

            double currentFreq = frequency;                         //  this lets us modulate the frequency

            //  Applies Frequency Modulation
            if (useFrequencyModulation)
            {
                double freqOffset = (frequencyModulationOscillatorIntensity * frequency * 0.75) / 100.0;
                currentFreq += mapValueD(frequencyModulationOscillator.calculateSignalValue(preciseDspTime, frequencyModulationOscillatorFrequency), -1.0, 1.0, -freqOffset, freqOffset);
                frequencyModulationRangeOut = (float)frequencyModulationOscillator.calculateSignalValue(preciseDspTime, frequencyModulationOscillatorFrequency) * 0.5f + 0.5f;
            }
            else
            {
                frequencyModulationRangeOut = 0.0f;
            }

            //  the samples calculated for the sine wave
            sinOutput = (float)(sinWeight * sinusAudioWave.calculateSignalValue(preciseDspTime, currentFreq));
            //  the samples calculated for the saw wave
            sawOutput = (float)(sawWeight * sawAudioWave.calculateSignalValue(preciseDspTime, currentFreq));
            //  the samples calculated for the square wave
            sqrOutput = (float)(sqrWeight * squareAudioWave.calculateSignalValue(preciseDspTime, currentFreq));


            /*      Mixa assieme tutti gli output
             http://www.vttoth.com/CMS/index.php/technical-notes/68
             Let's say we have two signals, A and B. If A is quiet, we want to hear B on the output in unaltered form. If B 
            is quiet, we want to hear A on the output (i.e., A and B are treated symmetrically.) If both A and B have a non-zero amplitude,
            the mixed signal must have an amplitude between the greater of A and B, and the maximum permissible amplitude.
            If we take A and B to have values between 0 and 1, there is actually a simple equation that satisfies all of the
            above conditions:       Z= A + B − AB.
            Simple, isn't it! Moreover, it can be easily adapted for more than two signals. 
            Consider what happens if we mix another signal, C, to Z:  T= Z + C − Z C = A + B + C − AB − AC − BC + ABC.

             */
            nextOutput = sinOutput + sawOutput + sqrOutput - (sinOutput * sawOutput) -
                                    (sinOutput * sqrOutput) - (sawOutput * sqrOutput) + (sinOutput * sawOutput * sqrOutput);



            //  Applies Amplitude Modulation
            if (useAmplitudeModulation)
            {
                nextOutput *= (float)mapValueD(amplitudeModulationOscillator.calculateSignalValue(preciseDspTime, amplitudeModulationOscillatorFrequency), -1.0, 1.0, 0.0, 1.0);
                amplitudeModulationRangeOut = (float)amplitudeModulationOscillator.calculateSignalValue(preciseDspTime, amplitudeModulationOscillatorFrequency) * 0.5f + 0.5f;
            }
            else
            {
                amplitudeModulationRangeOut = 0.0f;
            }


            //  regulates the output based on the current volume of the synth
            float x = volume  * (float)nextOutput;

            //  Copies the samples on every available channels of the sound system
            for (int j = 0; j < channels; j++)
            {
                data[i * channels + j] = x;
            }

        }
        }

    }

  
    /// <summary>
    /// Change the starting octave that the user can play. This makes everything an octave up
    /// </summary>
    public void OctaveUp()
    {
        octaveNumber += 1;
        UpdateOctaveNumber();

        for (int i = 0; i < frequencies.Length; i++)
        {
            frequencies[i] *= 2;
        }
    }

    /// <summary>
    /// Change the starting octave that the user can play. This makes everything an octave down
    /// </summary>
    public void OctaveDown()
    {
        octaveNumber -= 1;
        UpdateOctaveNumber();

        for (int i = 0; i < frequencies.Length; i++)
        {
            frequencies[i] /= 2;
        }
    }

    /// <summary>
    /// Updates the number of the starting octave that the user can play inside the scene (gui)
    /// </summary>
    public void UpdateOctaveNumber()
    {
        var text = GameObject.Find("NumeroOttava").GetComponent<UnityEngine.UI.Text>();
        text.text = "C" + (octaveNumber) + " - C" + (octaveNumber + 1);

    }


    //These functions scale floats and double values from one range to another 
    float mapValue(float referenceValue, float fromMin, float fromMax, float toMin, float toMax)
    {
        /* This function maps (converts) a Float value from one range to another */
        return toMin + (referenceValue - fromMin) * (toMax - toMin) / (fromMax - fromMin);
    }

    double mapValueD(double referenceValue, double fromMin, double fromMax, double toMin, double toMax)
    {
        /* This function maps (converts) a Double value from one range to another */
        return toMin + (referenceValue - fromMin) * (toMax - toMin) / (fromMax - fromMin);
    }


    #region Change Synth Parameters From GUI

    /* These are needed in order to control values from the gui  */

    public void ChangeSinWeight(float weight)
    {
        sinWeight = weight;
    }

    public void ChangeSqrWeight(float weight)
    {
        sqrWeight = weight;
    }

    public void ChangeSawWeight(float weight)
    {
        sawWeight = weight;
    }

    public void ActivateFm()
    {
        useFrequencyModulation = !useFrequencyModulation;
    }

    public void ChangeFMFreq(float value)
    {
        frequencyModulationOscillatorFrequency = value;
    }


    public void ChangeFMIntensity(float value)
    {
        frequencyModulationOscillatorIntensity = value;
    }

    public void ActivateAm()
    {
        useAmplitudeModulation = !useAmplitudeModulation;
    }

    public void ChangeAMFreq(float value)
    {
        amplitudeModulationOscillatorFrequency = value;
    }

    #endregion



}

