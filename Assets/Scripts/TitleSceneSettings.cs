using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleSceneSettings : MonoBehaviour
{
    [SerializeField] Slider MasterVolumeSlider;
    [SerializeField] Slider MusicVolumeSlider;
    [SerializeField] Slider TimeScaleSlider;
    [SerializeField] Toggle ShowTraceToggle;
    [SerializeField] Toggle ShowTraceOnResetToggle;
    [SerializeField] Toggle ShowArrowsToggle;

    [SerializeField] TMP_Text TimeScaleText;
    [SerializeField] TMP_Text MasterVolumeText;
    [SerializeField] TMP_Text MusicVolumeText;

    [SerializeField] AudioSource BackgroundMusic;

    // Start is called before the first frame update
    void Awake()
    {
        // Debug.Log(SettingsManager.showArrows);
        MasterVolumeSlider.value = SettingsManager.MasterVolume/100;
        MusicVolumeSlider.value = SettingsManager.MusicVolume/100;
        TimeScaleSlider.value = SettingsManager.TimeScale;
        ShowTraceToggle.isOn = SettingsManager.ShowTrace;
        ShowArrowsToggle.isOn = SettingsManager.showArrows;
        ShowTraceOnResetToggle.isOn = SettingsManager.showTraceOnReset;
        // Debug.Log("2 " + ShowArrowsToggle.isOn);
        UpdateTimeScale();
        UpdateMasterVolume();
        UpdateMusicVolume();

    }
    public void UpdateMasterVolume()
    {
        SettingsManager.MasterVolume = MasterVolumeSlider.value*100;
        AudioListener.volume = MasterVolumeSlider.value;
        MasterVolumeText.text = (SettingsManager.MasterVolume).ToString("F0");

    }
    public void UpdateMusicVolume()
    {
        SettingsManager.MusicVolume = MusicVolumeSlider.value*100;
        BackgroundMusic.volume = MusicVolumeSlider.value/4f;
        MusicVolumeText.text = (SettingsManager.MusicVolume).ToString("F0");

    }
    public void UpdateTimeScale()
    {
        SettingsManager.TimeScale = TimeScaleSlider.value;
        TimeScaleText.text = (SettingsManager.TimeScale).ToString("F2");
    }
    public void UpdateDrawArrows()
    {
        SettingsManager.showArrows = ShowArrowsToggle.isOn;
    }
    public void UpdateDrawTrace()
    {
        SettingsManager.ShowTrace = ShowTraceToggle.isOn;
    }
    public void UpdateDrawTraceOnReset()
    {
        SettingsManager.showTraceOnReset = ShowTraceOnResetToggle.isOn;
    }
    public void ResetSave()
    {
        SettingsManager.ResetSave = true;
    }
    // Update is called once per frame
    void Update()
    {
    //     if (MasterVolumeSlider.value * 100 != SettingsManager.MasterVolume)
    //     {
    //         SettingsManager.MasterVolume = MasterVolumeSlider.value * 100;
    //     }
    //     if (MusicVolumeSlider.value * 100 != SettingsManager.MusicVolume)
    //     {
    //         SettingsManager.MusicVolume = MusicVolumeSlider.value * 100;
    //     }
    //     if (TimeScaleSlider.value != SettingsManager.TimeScale)
    //     {
    //         SettingsManager.TimeScale = TimeScaleSlider.value;
    //     }
        // if (ShowTraceToggle.isOn != SettingsManager.ShowTrace)
        // {
        //     SettingsManager.ShowTrace = ShowTraceToggle.isOn;
        // }
        // if (ShowArrowsToggle.isOn != SettingsManager.showArrows)
        // {
        //     SettingsManager.showArrows = ShowArrowsToggle.isOn;
        // }
        // if (ShowTraceOnResetToggle.isOn != SettingsManager.showTraceOnReset)
        // {
        //     SettingsManager.showTraceOnReset = ShowTraceOnResetToggle.isOn;
        // }

        


    }
    public void ResetMusicVolume()
    {
        MusicVolumeSlider.value = 0.5f;
    }
    public void ResetMasterVolume()
    {
        MasterVolumeSlider.value = 1;
    }
    public void ResetTimeScale()
    {
        TimeScaleSlider.value = 2;
    }
    private void ResetAllSettings()
    {
        ResetTimeScale();
        ResetMusicVolume();
        ResetMasterVolume();
    }
}
