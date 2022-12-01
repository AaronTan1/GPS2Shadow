using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Object = System.Object;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundEffectSolo;
    [SerializeField] private AudioSource soundSourceSolo;
    [SerializeField] private AudioClip[] soundEffectCont;
    [SerializeField] private AudioSource soundSourceCont;
    [SerializeField] private AudioClip[] soundEffectAmbient;
    [SerializeField] private AudioSource soundSourceAmbient;
    private GameObject[] singletonCheck;
    private float masVol = 0.5f;
    private float sfxVol = 0.5f;
    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this) 
            Destroy(this); 
        else 
            Instance = this; 
        
        DontDestroyOnLoad (this);
        
        soundSourceCont.volume = sfxVol;
        soundSourceSolo.volume = sfxVol;
    }

    public void PlaySoundSolo(string soundName) //Single SFX, might need to make more than one
    {
        foreach (var t in soundEffectSolo)
        {
            if (t.name != soundName) continue;
            soundSourceSolo.clip = t;
            soundSourceSolo.Play();
        }
    }

    public void PlaySoundCont(string contSoundName)
    {
        foreach (var t in soundEffectCont)
        {
            if (t.name != contSoundName) continue;
            soundSourceCont.Stop();
            soundSourceCont.clip = t;
            soundSourceCont.Play();
        }
    }

    public void PlaySoundAmbient(string ambientSoundName) //Ambient
    {
        for (var i = 0; i < soundEffectCont.Length; i++)
        {
            if (soundEffectCont[i].name != ambientSoundName) continue;
            soundSourceAmbient.clip = soundEffectAmbient[i];
            soundSourceAmbient.Play();
        }
    }

    public void StopCont()
    {
        soundSourceCont.Stop();
    }

    public void ChangeMVol(float inM)
    {
        masVol = inM;
        AudioListener.volume = masVol;
    }
    
    public void ChangeSVol(float inS)
    {
        sfxVol = inS;
        soundSourceCont.volume = sfxVol;
        soundSourceSolo.volume = sfxVol;
    }

    public void ListenerUpdate()
    {
        AudioListener.volume = masVol;
    }
}