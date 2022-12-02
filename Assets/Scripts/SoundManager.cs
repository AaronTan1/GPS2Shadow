using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Object = System.Object;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClipSolo;
    [SerializeField] private AudioSource audioSourceSolo;
    [SerializeField] private AudioClip[] audioEffectCont;
    [SerializeField] private AudioSource audioSourceCont;
    [SerializeField] private AudioClip[] audioEffectMusic;
    [SerializeField] private AudioSource audioSourceMusic;
    [SerializeField] private UnityEngine.UI.Slider sfxSlider;
    [SerializeField] private UnityEngine.UI.Slider musicSlider;
    private GameObject[] singletonCheck;
    private float musicVol = 0.5f;
    private float sfxVol = 0.5f;
    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this) 
            Destroy(this); 
        else 
            Instance = this; 
        
        DontDestroyOnLoad (this);

        audioSourceCont.volume = musicVol;
        audioSourceSolo.volume = sfxVol;
        audioSourceMusic.volume = sfxVol;
        
        if(sfxSlider != null)
        {
            sfxSlider.onValueChanged.AddListener(ChangeSFXVolume);
            musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
            sfxSlider.value = sfxVol;
            musicSlider.value = musicVol;
        }
        
    }

    public void PlaySoundSolo(string soundName) //Single SFX, might need to make more than one
    {
        foreach (var t in audioClipSolo)
        {
            if (t.name != soundName) continue;
            audioSourceSolo.clip = t;
            audioSourceSolo.Play();
        }
    }

    public void PlaySoundCont(string contSoundName)
    {
        foreach (var t in audioEffectCont)
        {
            if (t.name != contSoundName) continue;
            audioSourceCont.Stop();
            audioSourceCont.clip = t;
            audioSourceCont.Play();
            audioSourceCont.loop = true;
        }
    }

    public void PlaySoundAmbient(string ambientSoundName) //Ambient
    {
        for (var i = 0; i < audioEffectCont.Length; i++)
        {
            if (audioEffectCont[i].name != ambientSoundName) continue;
            audioSourceMusic.clip = audioEffectMusic[i];
            audioSourceMusic.Play();
        }
    }

    public void StopCont()
    {
        audioSourceCont.Stop();
    }

    public void ChangeMusicVolume(float vol)
    {
        musicVol = vol;
        audioSourceMusic.volume = musicVol;
    }
    
    public void ChangeSFXVolume(float vol)
    {
        sfxVol = vol;
        audioSourceCont.volume = sfxVol;
        audioSourceSolo.volume = sfxVol;
    }

    public void FindSliderReference()
    {
        
    }
}