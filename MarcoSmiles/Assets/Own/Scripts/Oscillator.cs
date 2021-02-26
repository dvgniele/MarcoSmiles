﻿using UnityEngine;


public class Oscillator : MonoBehaviour
{

  

    public double frequency = 440;
    // For oscillation
    private double phaseIncrement;
    private double phase;

    // Samplerate information
    private static int _FS;
    private static double _sampleDuration;

    // For single-pole low pass filter
    private float previousOutput;
    private float nextOutput;

    private float sinOutput;
    private float sawOutput;
    private float sqrOutput;
    private float noiseOutput;


    public float gain;
    public float volume = 0.05f;

    private float[] frequencies = new float[] {
                        261.630f , 277.180f , 293.660f , 311.130f , 329.630f , 349.990f ,
                        369.990f , 392.000f , 415.300f , 440.000f , 466.160f , 493.880f };
    private float octave = 1f;

    public int thisFreq;

    // Waveshape weights
    [Range(0f, 1f)]
    public double sinWeight;
    [Range(0f, 1f)]
    public double sqrWeight;
    [Range(0f, 1f)]
    public double sawWeight;
    [Range(0f, 1f)]
    public double noiseWeight;




    [Range(0, 1)]
    public double lowPass;
    [Range(20, 20000)]
    public double filterFreq;
    [Range(1, 5)]
    public float resonance;



    void Start()
    {
      


        sinWeight = 0.75;
        sqrWeight = 0.25;
        sawWeight = 0.25;


        //thisWaveform = 0;

        // Grab the sample rate from the system
        _FS = AudioSettings.outputSampleRate;
        // Calculate how long each sample lasts
        _sampleDuration = 1.0 / _FS;
        // Generate frequencies corresponding to all MIDI values
        //frequencies = GenerateFrequencies();

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

        /* 
         * This is "the current time of the audio system", as given
         * by Unity. It is updated every time the OnAudioFilterRead() function
         * is called. It's usually every 1024 samples.
         * 
         * A note on the sample rate:
         * We don't actually see real numbers for the sample rate, we instead
         * read it from the system in the Start() function.
         */

        //currentDspTime = AudioSettings.dspTime;

        for (int i = 0; i < data.Length; i += channels)
        {
            /*
             * Sample duration is just 1/fs. Because dspTime isn't updated every
             * sample, we "update" it ourselves so our envelope sounds smooth.
             */
            //  currentDspTime += _sampleDuration;
            //  envelope = ComputeAmplitude(currentDspTime) * volume;

            /*
             * The phase variable below increments by the calculated amount for
             * every audio sample. We can then use this value to calculate what
             * each waveform's value should be.
             * 
             *             2pi * f
             *     phase = -------
             *               fs
             * 
             * When phase is greater than 2pi, we just reset
             * it so we don't have an ever-increasing variable that will cause
             * an overflow error.
             */
            phaseIncrement = (frequency * octave) * 2.0 * Mathf.PI / _FS;
            phase += phaseIncrement;
            if (phase > (Mathf.PI * 2))
            {
                phase = phase % (Mathf.PI * 2);
            }

            nextOutput = 0;
            sinOutput = 0;
            sawOutput = 0;
            sqrOutput = 0;
            noiseOutput = 0;

            // Adds sinusoidal wave to the next output sample                   sinOutput = (float)(sinWeight * Mathf.Sin((float)phase));
            sinOutput = (float)(sinWeight * SineHP.Sin((float)phase));
            // nextOutput += (float)(sinWeight * Mathf.Sin((float)phase));

            // Adds sawtooth wave to the next output sample
            sawOutput = (float)sawWeight - (float)(sawWeight / Mathf.PI * phase);

            // nextOutput += (float)(sawWeight * ((phase) / (2 * Mathf.PI)));



            // Adds square wave to the next output sample         
            if (phase > Mathf.PI)
            {
                sqrOutput = (float)(sqrWeight);
                //nextOutput += (float) (sqrWeight ) ;
            }
            else
            {
                sqrOutput = (-(float)(sqrWeight));
                //nextOutput += (-(float) ( sqrWeight) ) ;
            }


            // Adds noise wave to the next output sample    
            noiseOutput = PinkNoise.Noise();


        /*      Mixa tutti gli output
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

        nextOutput += noiseOutput * (float)noiseWeight;      //da cambiare
           
            /*
             * Here we apply a single-pole low-pass filter. Even if the filter
             * is completely open (lowPass = 1) we still compute this step so we
             * don't have to have any conditional logic.
             */
            nextOutput = (float)((nextOutput * lowPass) + (previousOutput * (1 - lowPass)));

            nextOutput = nextOutput * gain;



            // Write the output to the audio filter
            data[i] += nextOutput;

            // This is for the next low-pass calculation
            previousOutput = nextOutput;

            // Copy the sound from one channel into the next channel for stereo
            if (channels == 2) data[i + 1] = data[i];
        }

    }


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

   


    //pink noise approximation
    public class PinkNoise
    {
        private static System.Random rnd = new System.Random();

        public static float Noise()
        {
            return (float) rnd.NextDouble();
        }
    }

    // High percision sine approximation
    public class SineHP
    {
        private static float sin = 0; 

        public static float Sin(float x)
        {
            if (x < -3.14159265f)
                x += 6.28318531f;
            else
                if (x > 3.14159265f)
                x -= 6.28318531f;

            if (x < 0)
            {
                sin = x * (1.27323954f + 0.405284735f * x);

                if (sin < 0)
                    sin = sin * (-0.255f * (sin + 1) + 1);
                else
                    sin = sin * (0.255f * (sin - 1) + 1);
            }
            else
            {
                sin = x * (1.27323954f - 0.405284735f * x);

                if (sin < 0)
                    sin = sin * (-0.255f * (sin + 1) + 1);
                else
                    sin = sin * (0.255f * (sin - 1) + 1);
            }

            return sin;
        }

    }


    public void OctaveUp()
    {


        for (int i = 0; i < frequencies.Length; i++)
        {
            frequencies[i] *= 2;
        }
    }

    public void OctaveDown()
    {
        for (int i = 0; i < frequencies.Length; i++)
        {
            frequencies[i] /= 2;
        }
    }
}

