using UnityEngine;
using UnityEngine.Audio;
using AudioManipulation;

public class AudioMixerController : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVol", Mathf.Clamp(AudioUnitConversions.RelToDB(volume), -80, 0));
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVol", Mathf.Clamp(AudioUnitConversions.RelToDB(volume), -80, 0));
    }
}
