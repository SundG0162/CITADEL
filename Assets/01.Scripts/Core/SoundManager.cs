using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using DG.Tweening;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioMixer audioMixer;

    public Slider masterAudioSlider;
    public Slider bgmAudioSlider;
    public Slider sfxAudioSlider;

    public bool soundSettingOn = false;

    RectTransform _soundSetting;

    private void Awake()
    {
        Instance = this;
        masterAudioSlider = transform.Find("Setting/MasterSlider").GetComponent<Slider>();
        bgmAudioSlider = transform.Find("Setting/BGMSlider").GetComponent<Slider>();
        sfxAudioSlider = transform.Find("Setting/SFXSlider").GetComponent<Slider>();
        if (SceneManager.GetActiveScene().name == "SampleScene") return;
        _soundSetting = transform.Find("Setting").GetComponent<RectTransform>();
    }

    public void MasterAudioControl()
    {
        float masterSound = masterAudioSlider.value;

        if (masterSound == -40f) audioMixer.SetFloat("Master", -80f);
        else audioMixer.SetFloat("Master", masterSound);
    }

    public void BGMAudioControl()
    {
        float bgmSound = bgmAudioSlider.value;

        if (bgmSound == -40f) audioMixer.SetFloat("BackgroundMusic", -80f);
        else audioMixer.SetFloat("BackgroundMusic", bgmSound);
    }

    public void SFXAudioControl()
    {
        float sfxSound = sfxAudioSlider.value;

        if (sfxSound == -40f) audioMixer.SetFloat("SFX", -80f);
        else audioMixer.SetFloat("SFX", sfxSound);
    }

    public void SoundSetting()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene") return;
        if (!soundSettingOn)
        {
            _soundSetting.DOAnchorPosY(-61f, 0.5f).SetEase(Ease.OutCubic);
            soundSettingOn = true;
        }
        else
        {
            soundSettingOn = false;
            _soundSetting.DOAnchorPosY(-1080f, 0.5f).SetEase(Ease.OutCubic);
        }
    }
}
