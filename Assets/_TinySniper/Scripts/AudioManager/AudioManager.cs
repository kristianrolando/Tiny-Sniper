using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private Soundfx[] soundfx;
    [SerializeField] private SoundBgm[] soundBgm;

    [SerializeField] AudioSource sourceSfx;
    [SerializeField] AudioSource sourceBgm;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        SetupAudioSource();
    }
    private void OnEnable()
    {
        AudioSetting.OnSettingChanged += SetupAudioSource;
    }
    private void OnDisable()
    {
        AudioSetting.OnSettingChanged -= SetupAudioSource;
    }

    void SetupAudioSource()
    {
        sourceSfx.playOnAwake = false;
        sourceBgm.playOnAwake = false;
        sourceSfx.loop = false;
        sourceBgm.loop = true;

        if (PlayerPrefs.HasKey("sfx vol") || PlayerPrefs.HasKey("bgm vol"))
        {
            sourceSfx.volume = PlayerPrefs.GetFloat("sfx vol");
            sourceBgm.volume = PlayerPrefs.GetFloat("bgm vol");
        }
        else
        {
            sourceSfx.volume = 1;
            sourceBgm.volume = 1;
        }
        
    }
    public void PlaySfx(string message)
    {
        Soundfx _sound = Array.Find(soundfx, sound => sound.name == message);
        if (_sound == null)
        {
            Debug.LogError("Sound : " + message + " not found");
            return;
        }
        sourceSfx.clip = _sound.clip;
        sourceSfx.Play();
    }
    public void PlayBgm(string message)
    {
        SoundBgm _sound = Array.Find(soundBgm, sound => sound.name == message);
        if (_sound == null)
        {
            Debug.LogError("Sound : " + message + " not found");
            return;
        }
        sourceBgm.clip = _sound.clip;
        sourceBgm.Play();
    }

}

[Serializable]
public class Soundfx
{
    public string name;
    public AudioClip clip;

    [HideInInspector]
    public AudioSource source;
}

[Serializable]
public class SoundBgm
{
    public string name;
    public AudioClip clip;

    [HideInInspector]
    public AudioSource source;
}