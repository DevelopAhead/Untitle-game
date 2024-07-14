using System;
using System.Collections;
using System.Collections.Generic;
using Hellmade.Sound;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering;
[DefaultExecutionOrder(10)]
public class SoundManager : MonoInstance<SoundManager>
{
    public AudioContainer AudioMusic;
    public AudioContainer AudioSoundFX;
    public AudioContainer AudioSoundUI;

    public float GlobalVolume{ get { return EazySoundManager.GlobalVolume;} set { EazySoundManager.GlobalVolume = value;}}
    public float GlobalMusicVolume{ get { return EazySoundManager.GlobalMusicVolume;} set { EazySoundManager.GlobalMusicVolume = value;}}
    public float GlobalSFXVolume{ get { return EazySoundManager.GlobalSoundsVolume;} set { EazySoundManager.GlobalSoundsVolume = value;}}
    public float GlobalUISoundsVolume{ get { return EazySoundManager.GlobalUISoundsVolume;} set { EazySoundManager.GlobalUISoundsVolume = value;}}

    public void PlaySoundFX(string name) => EazySoundManager.PlaySound(AudioSoundFX.Get(name));
    public void PlayMusic(string name) => EazySoundManager.PlayMusic(AudioMusic.Get(name));
    public void PlayUISound(string name) => EazySoundManager.PlayUISound(AudioSoundUI.Get(name));
    public void ChangeCassette(SoundType soundType,AudioContainer to)
    {
        switch(soundType)
        {
            case SoundType.Music : AudioMusic = to; break;
            case SoundType.SFX : AudioSoundFX = to; break;
            case SoundType.UI : AudioSoundUI = to; break;
        }
    }

    [Button]
    public void TestChangeCassette(SoundType soundType,AudioContainer to)
    {
        ChangeCassette(soundType,to);
    }
}
