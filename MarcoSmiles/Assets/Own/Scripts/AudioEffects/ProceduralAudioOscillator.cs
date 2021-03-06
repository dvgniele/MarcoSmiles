using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(AudioSource))]
public class ProceduralAudioOscillator : MonoBehaviour
{


    SawWave sawAudioWave;
    SquareWave squareAudioWave;
    SinusWave sinusAudioWave;

    //For amplitude and frequency modulation
    SinusWave amplitudeModulationOscillator;
    SinusWave frequencyModulationOscillator;


    public double frequency = 440;


    private int sampleRate;  // samples per second
    private double dataLen;     // the data length of each channel
    double chunkTime;
    double dspTimeStep;
    double currentDspTime;

    // For single-pole low pass filter
    private float previousOutput;
    private float nextOutput;

    // Waveshape output weights
    [Range(0f, 1f)]
    public double sinWeight;
    [Range(0f, 1f)]
    public double sqrWeight;
    [Range(0f, 1f)]
    public double sawWeight;
    [Range(0f, 1f)]
    public double noiseWeight;

    //waves actual output
    private float sinOutput;
    private float sawOutput;
    private float sqrOutput;
    private float noiseOutput;

    [Header("Amplitude Modulation")]
    public bool useAmplitudeModulation;             //Boolean parameter that determines whether or not to apply amplitude modulation on the produced sound.
    [Range(0.2f, 30.0f)]
    public float amplitudeModulationOscillatorFrequency = 1.0f;     //Float parameter that determines the Amplitude Modulation Oscillator’s frequency.

    [Header("Frequency Modulation")]
    public bool useFrequencyModulation;             // Boolean Parameter that determines whether or not to apply frequency modulation on the produced sound.
    [Range(0.2f, 30.0f)]
    public float frequencyModulationOscillatorFrequency = 1.0f;         //Float parameter that determines the Frequency Modulation Oscillator’s frequency.
    [Range(1.0f, 100.0f)]
    public float frequencyModulationOscillatorIntensity = 10.0f;        //Float parameter that determines the Frequency Modulation Oscillator’s intensity.


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
    [Range(0f, 1f)]
    public float volume = 0;
    [Range(0f, 1f)]
    public float volumeValue= 0.3f;



    // Manage Frequencies and notes 
    private float[] frequencies = new float[] {
                        261.630f , 277.180f , 293.660f , 311.130f , 329.630f , 349.990f ,
                        369.990f , 392.000f , 415.300f , 440.000f , 466.160f , 493.880f };
    private float octave = 1f;
    public int octaveNumber = 3;

    public int thisFreq;


    /* These control the single pole low pass filter */
    [Range(0, 1)]
    public double lowPass;
    [Range(20, 20000)]
    public double filterFreq;



    void Awake()
    {
        sawAudioWave = new SawWave();
        squareAudioWave = new SquareWave();
        sinusAudioWave = new SinusWave();

        amplitudeModulationOscillator = new SinusWave();
        frequencyModulationOscillator = new SinusWave();

        sampleRate = AudioSettings.outputSampleRate;
        Debug.Log(sampleRate );
    }


    void Start()
    {

        //Initial Preset. You can eventually save different presets in a similar way.
        sinWeight = 0.75;
        sqrWeight = 0.25;
        sawWeight = 0.25;

        lowPass = 1;

        if (_GM.isActive)
        {
            volume = volumeValue;
        }
        else
        {
            volume = 0.0f;           
        }

        changeNote(_GM.indexPlayingNote);

    }



    void Update()
    {

        if (_GM.isActive)
        {
            volume = volumeValue;             /*questo valore si potrebbe moltiplicare per qualche valore scalato che rappresenti la distanza 
                                         * delle mani dal leap, così da dare espressività al suono
                                         * 
                                         */
        }
        else
        {
            volume = 0.0f;           
        }

        changeNote(_GM.indexPlayingNote);
        //Debug.Log(frequency);



    }

    /* Uses a switch case.... in order to check on the index of the playing note and to change the frequency of the oscillator */
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
        nextOutput = 0;
        sinOutput = 0;
        sawOutput = 0;
        sqrOutput = 0;
        noiseOutput = 0;



        for (int i = 0; i < dataLen; i++)               // go through data chunk
        {
            preciseDspTime = currentDspTime + i * dspTimeStep;

            double currentFreq = frequency;

            //Applies Frequency Modulation
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


            sinOutput = (float)(sinWeight * sinusAudioWave.calculateSignalValue(preciseDspTime, currentFreq));

            sawOutput = (float)(sawWeight * sawAudioWave.calculateSignalValue(preciseDspTime, currentFreq));

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




            if (useAmplitudeModulation)
            {
                nextOutput *= (float)mapValueD(amplitudeModulationOscillator.calculateSignalValue(preciseDspTime, amplitudeModulationOscillatorFrequency), -1.0, 1.0, 0.0, 1.0);
                amplitudeModulationRangeOut = (float)amplitudeModulationOscillator.calculateSignalValue(preciseDspTime, amplitudeModulationOscillatorFrequency) * 0.5f + 0.5f;
            }
            else
            {
                amplitudeModulationRangeOut = 0.0f;
            }



            float x = volume * 0.5f * (float)nextOutput;

            for (int j = 0; j < channels; j++)
            {
                data[i * channels + j] = x;
            }



        }



    }

  



    //pink noise approximation
    public class PinkNoise
    {
        private static System.Random rnd = new System.Random();

        public static float Noise()
        {
            return (float)rnd.NextDouble();
        }
    }


    public void OctaveUp()
    {
        octaveNumber += 1;
        UpdateOctaveNumber();

        for (int i = 0; i < frequencies.Length; i++)
        {
            frequencies[i] *= 2;
        }
    }

    public void OctaveDown()
    {
        octaveNumber -= 1;
        UpdateOctaveNumber();

        for (int i = 0; i < frequencies.Length; i++)
        {
            frequencies[i] /= 2;
        }
    }

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



    /* These are needed in order to control values fro the gui  */

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

    public void ChangeLowPass(float value)
    {
        lowPass = value;
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





}

