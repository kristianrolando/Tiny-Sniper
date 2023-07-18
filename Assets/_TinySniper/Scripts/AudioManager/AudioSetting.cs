using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    public static System.Action OnSettingChanged;

    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider bgmSlider;

    float sfxVol;
    float bgmVol;

    private void Awake()
    {
        sfxSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
        bgmSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
        if (PlayerPrefs.HasKey("sfx vol") || PlayerPrefs.HasKey("bgm vol"))
        {
            Load();
        }
    }
    void UpdateVolume()
    {
        sfxVol = sfxSlider.value;
        bgmVol = bgmSlider.value;
        Save();
    }

    void UpdateSlider(float sfx, float bgm)
    {
        sfxSlider.value = sfx;
        bgmSlider.value = bgm;
    }
    void Save()
    {
        PlayerPrefs.SetFloat("sfx vol", sfxVol);
        PlayerPrefs.SetFloat("bgm vol", bgmVol);
        OnSettingChanged?.Invoke();
    }

    void Load()
    {
        sfxVol = PlayerPrefs.GetFloat("sfx vol");
        bgmVol = PlayerPrefs.GetFloat("bgm vol");
        UpdateSlider(sfxVol, bgmVol);
    }
}
