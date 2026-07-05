using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [HideInInspector] public int LevelsBeaten = 1;
    private bool gameOver = false;
    [HideInInspector] public int NumCharges;
    private float TimeScale = 1;
    private int BaseTimeScale = 1;
    // private float zoom = 18;
    private bool isStarted = false;
    [HideInInspector] public bool IsInUI;
    private bool showTraceOnReset = false;
    public GameObject ChargeBoxes;
    public LevelManager LevelManager;
    public UndoManager UndoManager;
    private bool paused = true;

    // [Header("Debug Mode")]
    // [SerializeField] private bool DrawArrowsInspector = false;
    // [SerializeField] private bool DrawTraceInspector = true;
    // [SerializeField] bool showTraceOnResetInspector = false;

    [HideInInspector] public bool DrawArrows = false;
    [HideInInspector] public bool DrawTrace = true;

    [Space(10)]
    [Header("Test Charge")]
    public GameObject TestCharge;
    [HideInInspector] public Vector2 testChargeStartPos;
    [SerializeField] TrailRenderer tr;
    [SerializeField] private Sprite DamagedSprite;
    [SerializeField] private Sprite NormalSprite;

    [Space(10)]
    [Header("Texts")]
    public TMP_Text MainText;
    public TMP_Text ChargesText;
    public TMP_Text LevelText;
    public TMP_Text TimeScaleText;
    [SerializeField] TMP_Text VolumeText;
    [SerializeField] TMP_Text MusicVolumeText;

    // [SerializeField] TMP_Text ZoomText;

    [Space(10)]
    [Header("UI")]
    public Vector3 UIStart;
    [SerializeField] private float SmoothMoveTime;

    private Vector3 MiddleOfScreen = Vector3.zero;
    public GameObject EndScreenPanel;
    public GameObject LevelScreenPanel;
    public GameObject SettingsPanel;
    public GameObject PauseMenuPanel;

    [SerializeField] Slider TimeScaleSlider;
    [SerializeField] Slider VolumeScaleSlider;
    [SerializeField] Slider MusicVolumeScaleSlider;

    // [SerializeField] Slider CameraZoomSlider;
    [SerializeField] AudioSource BackgroundMusic;
    [SerializeField] Toggle DrawArrowsToggle;
    [SerializeField] Toggle DrawTraceToggle;
    [SerializeField] Toggle ShowTraceOnResetToggle;
    [Space(10)]
    [SerializeField] GameObject GrayedBackground;

    [Space(10)]
    [Header("Stars")]
    public Sprite EmptyStarSprite;
    public Sprite FullStarSprite;
    public GameObject[] Stars;
    public GameObject[] EndPanelStars;
    [HideInInspector] public int ThreeStarsCharges;
    [HideInInspector] public int TwoStarsCharges;
    private int currentStarsFilled = 0;
    
    [Space(10)]
    [Header("SFX")]
    [SerializeField] private AudioClip WinSFX;
    [SerializeField] private AudioClip CollisionSFX;
    [SerializeField] private AudioClip[] popSFX;
    [SerializeField] private AudioClip WoodExplosionSFX;
    [SerializeField] private AudioSource ThunderSFX;
    [SerializeField] private AudioSource OceanSFX;

    private Camera mainCamera;
    #region  Start
    void Start()
    {  
        UpdateOceanSounds(SettingsManager.PlayAmbientSound);
        mainCamera = Camera.main;
        ResetAllUI();
        StartCoroutine(PlayThunderRoutine());
        // UpdateAllSettings();
        LevelsBeaten = SaveManager.Instance.highestLevelCompleted;
        // TestCharge.GetComponent<TrailRenderer>().time = 1;


    }
    #endregion
    void UpdateAllSettings()
    {
        VolumeScaleSlider.value = SettingsManager.MasterVolume/100;
        MusicVolumeScaleSlider.value = SettingsManager.MusicVolume/100;
        TimeScaleSlider.value = SettingsManager.TimeScale;
        // Debug.Log("Show arrows:" + SettingsManager.showArrows);
        DrawArrowsToggle.isOn = SettingsManager.showArrows;
        DrawTraceToggle.isOn = SettingsManager.ShowTrace;
        ShowTraceOnResetToggle.isOn = SettingsManager.showTraceOnReset;
        UpdateVolumeScale();
        UpdateMusicVolumeScale();
        UpdateTimeScale();
        

        DrawArrows = SettingsManager.showArrows;
        tr.enabled = SettingsManager.ShowTrace;
        showTraceOnReset = SettingsManager.showTraceOnReset;
    }
    #region  Awake
    void Awake()
    {
        // DrawArrowsToggle.isOn = DrawArrowsInspector;
        // DrawTraceToggle.isOn = DrawTraceInspector;
        // DrawTrace = DrawTraceInspector;
        // DrawArrows = DrawArrowsInspector;
        // showTraceOnReset = showTraceOnResetInspector;
        // tr = TestCharge.GetComponent<TrailRenderer>();
        UpdateAllSettings();
        // tr.enabled = DrawTrace;
    }
    #endregion
    #region  Update
    void Update()
    {
        
        Time.timeScale = BaseTimeScale * TimeScale;
        Time.fixedDeltaTime = 0.02f * Mathf.Min(1, Time.timeScale);
        TimeScaleText.text = (TimeScale).ToString("F2");
        ChargesText.text = $"Cyclones: {NumCharges}";
        // if (DrawArrowsToggle.isOn != SettingsManager.showArrows)
        // {
        //     SettingsManager.showArrows = DrawArrowsToggle.isOn;
        //     DrawArrows = DrawArrowsToggle.isOn;
        // }

        if (NumCharges <= ThreeStarsCharges && currentStarsFilled != 3)
        {
            UpdateStars(3);
        }
        else if (NumCharges <= TwoStarsCharges && NumCharges > ThreeStarsCharges && currentStarsFilled != 2)
        {
            UpdateStars(2);
        }
        else if (NumCharges > TwoStarsCharges && currentStarsFilled != 1)
        {
            UpdateStars(1);
        }
    }
    #endregion
    public void ShowTraceOnReset()
    {
        showTraceOnReset = ShowTraceOnResetToggle.isOn;
        SettingsManager.showTraceOnReset = showTraceOnReset;

    }
    // void CalculateMiddleOfScreen()
    // {
    //     MiddleOfScreen = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1));
    // }

    public void MinimalUI()
    {
        ChargeBoxes.SetActive(!ChargeBoxes.activeSelf);
        for (int i = 0; i < 3; i++)
        {
            Stars[i].SetActive(!Stars[i].activeSelf);
        }
        ChargesText.gameObject.SetActive(!ChargesText.gameObject.activeSelf);
        LevelText.gameObject.SetActive(!LevelText.gameObject.activeSelf);
    }

    public void UpdateTimeScale()
    {
        TimeScale = TimeScaleSlider.value;
        SettingsManager.TimeScale = TimeScale;
    }

    public void UpdateVolumeScale()
    {
        AudioListener.volume = VolumeScaleSlider.value;
        VolumeText.text = (VolumeScaleSlider.value * 100).ToString("F0");
        SettingsManager.MasterVolume = VolumeScaleSlider.value * 100;
    }
    public void UpdateMusicVolumeScale()
    {
        BackgroundMusic.volume = MusicVolumeScaleSlider.value/4f;
        MusicVolumeText.text = (MusicVolumeScaleSlider.value * 100).ToString("F0");
        SettingsManager.MusicVolume = MusicVolumeScaleSlider.value * 100;

    }

    // public void UpdateZoomScale()
    // {
    //     zoom = CameraZoomSlider.value * 0.04f + 18;
    //     mainCamera.DOOrthoSize(zoom, 0.15f).SetEase(Ease.InOutQuad).SetUpdate(true);
    //     ZoomText.text = $"Zoom: {zoom.ToString("F1")}";
    //     SettingsPanel.transform.localPosition = new Vector3(SettingsPanel.transform.localPosition.x, SettingsPanel.transform.localPosition.y, 0);
    // }

    public bool IsGameRunning()
    {
        if (gameOver)
        {
            return false;
        }
        return ((isStarted || BaseTimeScale == 1));
    }

    public void LoadLevelSelectPanel()
    {
        ResetAllUI();
        GrayOutBackground(true);
        // CalculateMiddleOfScreen();
        ChargeBoxes.SetActive(false);
        LevelManager.UpdateLevelScreenStars();

        LevelScreenPanel.transform.DOLocalMove(new Vector3(0, 0, -10), SmoothMoveTime).SetEase(Ease.InOutQuad).SetUpdate(true);
        IsInUI = true;
    }
    void GrayOutBackground(bool isOnOrOff)
    {
        GrayedBackground.SetActive(isOnOrOff);
    }
    public void UpdateStars(int numFull)
    {
        for (int i = 0; i < Stars.Length; i++)
        {
            Stars[i].GetComponent<SpriteRenderer>().sprite = EmptyStarSprite;
        }
        for (int i = 0; i < numFull; i++)
        {
            Stars[i].GetComponent<SpriteRenderer>().sprite = FullStarSprite;
        }
        currentStarsFilled = numFull;
    }
    public AudioClip RandomPopSound()
    {
        return popSFX[Random.Range(0, popSFX.Length)];
    }
    public void ResetTimeScaling()
    {
        TimeScaleSlider.value = 2;
        UpdateTimeScale();
        TimeScaleText.text = (TimeScale).ToString("F2");
    }
    public void ResetMasterVolume()
    {
        VolumeScaleSlider.value = 1;
        UpdateVolumeScale();
    }
    public void ResetMusicVolume()
    {
        MusicVolumeScaleSlider.value = 0.5f;
        UpdateMusicVolumeScale();
    }

    public void UpdateSomeTexts(string level)
    {
        LevelText.text = level;
    }

    public void Collision()
    {
        MainText.text = "Collision!";
        TestCharge.GetComponent<SpriteRenderer>().sprite = DamagedSprite;
        MainText.color = Color.red;
        MainText.gameObject.SetActive(true);
        // AudioSource.PlayClipAtPoint(CollisionSFX, Vector2.zero);
        AudioSource.PlayClipAtPoint(WoodExplosionSFX, Vector2.zero);

        BaseTimeScale = 0;
        gameOver = true;
    }
    public void UpdateOceanSounds(bool playOceanSound)
    {
        if (playOceanSound) 
        {
            OceanSFX.Play();
        }
        else 
        {
            OceanSFX.Stop();
        }
    }
    #region  Score
    public void Score()
    {
        MainText.text = "Goal!";
        TestCharge.GetComponent<SpriteRenderer>().sprite = DamagedSprite;
        MainText.color = Color.green;
        MainText.gameObject.SetActive(true);
        AudioSource.PlayClipAtPoint(WinSFX, new Vector3(0, 0, -10));
        BaseTimeScale = 0;
        gameOver = true;
        int currentLevel = LevelManager.currentLevelIndex;
        int currentChargeCount = GameObject.FindGameObjectsWithTag("Charge").Length;
        
        if (currentLevel - 1 == LevelsBeaten)
            LevelsBeaten++;

        if (currentStarsFilled > LevelManager.levels[currentLevel].stars)
        {
            LevelManager.levels[LevelManager.currentLevelIndex].stars = currentStarsFilled;
        }
       
        // save level solution
        if (LevelManager.levelCharges.ContainsKey(currentLevel))
        {
            if (currentChargeCount < LevelManager.levelCharges[currentLevel])
            {
                UndoManager.SaveLevelsBeaten(currentLevel);
                LevelManager.levelCharges[currentLevel] = currentChargeCount;
            }
        }
        else
        {
            LevelManager.levelCharges[currentLevel] = currentChargeCount;
            UndoManager.SaveLevelsBeaten(currentLevel);
        }

        SaveManager.Instance.LevelCompleted(LevelManager.currentLevelIndex, NumCharges);
        
        EndPanel();
    }
    #endregion
    public void ResetLevel()
    {
        Vector3 difference = (Vector3)testChargeStartPos-TestCharge.transform.position;
        if (showTraceOnReset)
        {
            tr.gameObject.transform.position -= difference;
        }
        else
        {
            tr.Clear();
        }

        LevelManager.ReloadLevel();


    }
    #region thunder sounds
    IEnumerator PlayThunderRoutine()
    {
        while (true)
        {
            float delay = Random.Range(5, 16);
            yield return new WaitForSecondsRealtime(delay);
            if (SettingsManager.PlayAmbientSound)
            {
                ThunderSFX.Play();
            }
            
        }
    }
    #endregion
    public void EndPanel()
    {
        ResetAllUI();
        GrayOutBackground(true);
        // CalculateMiddleOfScreen();
        for (int i = 0; i < EndPanelStars.Length; i++)
        {
            EndPanelStars[i].GetComponent<SpriteRenderer>().sprite = EmptyStarSprite;
        }
        for (int i = 0; i < currentStarsFilled; i++)
        {
            EndPanelStars[i].GetComponent<SpriteRenderer>().sprite = FullStarSprite;
        }
        EndScreenPanel.transform.DOLocalMove(new Vector3(0, 0, -10), SmoothMoveTime).SetEase(Ease.InOutQuad).SetUpdate(true);
        IsInUI = true;
    }
    #region LevelLoaded
    public void LevelLoaded(bool reload)
    {
        ResetAllUI(true);

        MainText.gameObject.SetActive(false);
        gameOver = false;
        isStarted = false;
        BaseTimeScale = 0;
        paused = true;
        TestCharge.transform.position = testChargeStartPos;
        TestCharge.GetComponent<SpriteRenderer>().sprite = NormalSprite;

        foreach (GameObject arrow in GameObject.FindGameObjectsWithTag("Arrow"))
        {
            DestroyImmediate(arrow);
        }
        foreach (GameObject explosion in GameObject.FindGameObjectsWithTag("Explosion"))
        {
            DestroyImmediate(explosion);
        }
        if (!reload || (reload && !showTraceOnReset)) tr.Clear();
        TestCharge.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        if (!reload)
        {
            foreach (GameObject charge in GameObject.FindGameObjectsWithTag("Charge"))
            {
                DestroyImmediate(charge);
            }
            // NumCharges = 0;
        }
    }
    #endregion
    public void ToggleArrows()
    {
        DrawArrows = DrawArrowsToggle.isOn;
        SettingsManager.showArrows = DrawArrowsToggle.isOn;

    }

    public void ToggleTrace()
    {
        tr.enabled = DrawTraceToggle.isOn;
        SettingsManager.ShowTrace = tr.enabled;

    }

    public void Restart()
    {
        ResetLevel();
        foreach (GameObject charge in GameObject.FindGameObjectsWithTag("Charge"))
        {
            DestroyImmediate(charge);
        }
       UndoManager.SaveScreen();
        // NumCharges = 0;
    }

    public void Retry()
    {
        ResetLevel();
    }

    public void Pause()
    {
        if (gameOver || !isStarted) return;
        paused = !paused;
        BaseTimeScale = paused ? 0 : 1;
    }

    public void StartGame()
    {
        // wont start when theres no charges on screen
        if (GameObject.FindGameObjectsWithTag("Charge").Length == 0) return;
        Retry();
        BaseTimeScale = 1;
        isStarted = true;
        paused = false;
        tr.gameObject.transform.localPosition = Vector2.zero;

        tr.Clear();

    }

    public void SettingsMenu()
    {
        ResetAllUI();
        GrayOutBackground(true);
        // CalculateMiddleOfScreen();
        SettingsPanel.transform.DOLocalMove(new Vector3(0, 0, -10), SmoothMoveTime).SetEase(Ease.InOutQuad).SetUpdate(true);
        IsInUI = true;
    }
    #region PAUSE MENU
    public void PauseMenu()
    {
        ResetAllUI();
        // if (DOTween.IsTweening(PauseMenuPanel.transform))
        // {
        //     PauseMenuPanel.transform.DOComplete();
        // }

        // CalculateMiddleOfScreen();
        bool isMoving = false;
        
        // Menu is currently closed (at UIStart position), so open it
        if (PauseMenuPanel.transform.localPosition == UIStart)
        {
            BaseTimeScale = 0;
            // PauseMenuPanel.transform.DOMove(MiddleOfScreen, SmoothMoveTime).SetEase(Ease.InOutQuad).SetUpdate(true);
            PauseMenuPanel.transform.DOLocalMove(new Vector3(0, 0, -10), SmoothMoveTime).SetEase(Ease.InOutQuad).SetUpdate(true);
            isMoving = true;
        }
        // Menu is open, so close it
        else
        {
            PauseMenuPanel.transform.DOLocalMove(UIStart, SmoothMoveTime).SetEase(Ease.InOutQuad).SetUpdate(true);
            BaseTimeScale = paused ? 0 : gameOver ? 0 : 1;
        }

        // ResetAllUI();
        if (isMoving) 
        {
            IsInUI = true;
            GrayOutBackground(true);
        }
    }
    #endregion
    public void MainMenu()
    {
        SceneManager.LoadScene("TitleScene");
    }
    #region  ResetAllUI
    public void ResetAllUI(bool leveLodaded = false)
    {
        // reset positions
        if (leveLodaded)
        {
            PauseMenuPanel.transform.localPosition = UIStart;
            SettingsPanel.transform.localPosition = UIStart;
            EndScreenPanel.transform.localPosition = UIStart;
            LevelScreenPanel.transform.localPosition = UIStart;
        }
        else
        {
            PauseMenuPanel.transform.DOLocalMove(UIStart, 0.3f).SetEase(Ease.InOutQuad).SetUpdate(true);
            SettingsPanel.transform.DOLocalMove(UIStart, 0.3f).SetEase(Ease.InOutQuad).SetUpdate(true);
            EndScreenPanel.transform.DOLocalMove(UIStart, 0.3f).SetEase(Ease.InOutQuad).SetUpdate(true);
            LevelScreenPanel.transform.DOLocalMove(UIStart, 0.3f).SetEase(Ease.InOutQuad).SetUpdate(true);
        }
        
        // fix this later it doesnt even work to fix the problem where you close the menu while the level screen is shifting
        // LevelManager.KillTweens();
        ChargeBoxes.SetActive(ChargesText.gameObject.activeSelf);
        IsInUI = false;
        GrayOutBackground(false);
    }
    #endregion

}