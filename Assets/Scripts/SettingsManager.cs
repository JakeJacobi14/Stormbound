using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static float MasterVolume = 100f;
    public static float MusicVolume = 50f;
    public static float TimeScale = 2.0f;
    public static bool ResetSave = false;
    public static bool ShowTrace = true;
    public static bool showTraceOnReset = false;
    public static bool showArrows = false;
    public static bool ShowMuffledArrows = false;
    public static bool PlayAmbientSound = true;

    // [SerializeField] AudioSource BackgroundMusic;

    void Start()
    {
        // Debug.Log("RANM");
        // MasterVolume = 100f;
        // MusicVolume = 50f;
        // TimeScale = 1.0f;
        // ResetSave = false;
        // ShowTrace = true;
        // showTraceOnReset = false;
        // showArrows = false;
    }
    void Awake()
    {
        // MasterVolumeSlider.value = MasterVolume;
        // AudioListener.volume = MasterVolume;
        // MusicVolumeSlider.value = MusicVolume/3f;
        // BackgroundMusic.volume = MusicVolume/3f;
        // TimeScaleSlider.value = TimeScale;
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.M))
        // {
        //     Debug.Log($"Master V {MasterVolume}     Music V {MusicVolume}      TimeScale {TimeScale}      ShowTrace {ShowTrace}       showTraceOnReset {showTraceOnReset}       showArrows {showArrows}");
        // }
    }
    
}
