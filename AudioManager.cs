using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioSource audioSource;
    public AudioClip startMusic;
    // Start is called before the first frame update
    public static AudioManager instance;
    void Awake()
    {
        instance = this;
        audioSource.clip = startMusic;
        
        
    }
    private void Start() {
        (float, float) volumes = GetVolume();
        Debug.Log(volumes.Item1 + " " + volumes.Item2);
        audioMixer.SetFloat("BackgroundVolume", volumes.Item1);
        audioMixer.SetFloat("SoundEffectsVolume", volumes.Item2);
        audioSource.Play();
    }

    public (float,float) GetVolume() {
        float backgroundVolume = (PlayerPrefs.HasKey("BackgroundVolume")) ? PlayerPrefs.GetFloat("BackgroundVolume"): 0 ;
        float soundEffectsVolume = (PlayerPrefs.HasKey("SoundEffectsVolume")) ? PlayerPrefs.GetFloat("SoundEffectsVolume") : 0; 
        
        return (backgroundVolume, soundEffectsVolume);
    }
    
    public void SetVolume(string mixerVariable, float volume) {
        audioMixer.SetFloat(mixerVariable, volume);
        
    }
    public void SaveVolumes(float bgvolume, float sfxvolume) {
        PlayerPrefs.SetFloat("BackgroundVolume", bgvolume);
        PlayerPrefs.SetFloat("SoundEffectsVolume", sfxvolume);
    }
   
}
