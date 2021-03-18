using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe MonoBehaviour che gestisce il funzionmento dell'oscilloscopio, può funzionare in due modalità
/// </summary>
[DisallowMultipleComponent]
public class Oscilloscope : ModeChanger
{
    [SerializeField] private Modes mode;

    private static int samples = 1024;

    private LineRenderer lineRenderer;
    /// <summary>
    /// Audio Source collegato allo script Oscilloscope, lo script "ascolta" questo oggetto e disegna il suono
    /// </summary>
    public AudioSource audioSource;

    private enum Modes { Standard, LeftRightDeflection }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        //audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        float[] samplesL = new float[samples];
        float[] samplesR = new float[samples];

        audioSource.GetOutputData(samplesL, 0);
        audioSource.GetOutputData(samplesR, 1);

        Vector3[] positions = new Vector3[samples];
        Vector3[] old_positions = new Vector3[samples];
        lineRenderer.GetPositions(old_positions);

        for (int i = 0; i < samples; i++)
        {
            switch (mode)
            {
                case Modes.Standard:
                    positions[i] = new Vector3((float)i / (samples - 1) * 2 - 1, (samplesL[i] + samplesR[i]) / 2);
                    break;
                case Modes.LeftRightDeflection:
                    positions[i] = new Vector3(samplesL[i], samplesR[i]);
                    break;
            }
        }

        lineRenderer.positionCount = samples;
        lineRenderer.SetPositions(positions);
    }

    public override List<Dropdown.OptionData> GetModes()
    {
        List<Dropdown.OptionData> modes = new List<Dropdown.OptionData>();

        foreach (string name in System.Enum.GetNames(typeof(Modes)))
        {
            modes.Add(new Dropdown.OptionData(name));
        }

        return modes;
    }

    /// <summary>
    /// Cambia la modalità usata dall'scilloscopio
    /// </summary>
    /// <param name="dropdown"></param>
    public override void SetMode(Dropdown dropdown)
    {
        mode = (Modes)dropdown.value;
    }
}
