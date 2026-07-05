using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TitleScene : MonoBehaviour
{
    [SerializeField] private GameObject InfoPanel;
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GameObject ControlsPanel;
    [SerializeField] private GameObject CreditsPanel;

    [SerializeField] private GameObject GrayedBackground;
    
    [Space(10)]
    [SerializeField] private SceneFader SceneFader;
    public void Start()
    {
        Time.timeScale = 1;
        ResetAllUI();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GrayedBackgroundIsEnabled())
            {
                LoadSettingsPanel(-50);
            }
            else
            {
                LoadSettingsPanel(0);
            }
            
        }
    }
    public void LoadGame()
    {
        // SceneManager.LoadScene("SampleScene");
        SceneFader.FadeOutScene("SampleScene");
    }
    public void LoadTutorial()
    {
        // SceneManager.LoadScene("SampleScene");
        SceneFader.FadeOutScene("TutorialScene");
    }
    public void LoadInfoPanel(int yLoc)
    {
        ResetAllUI();
        InfoPanel.transform.DOMove(new Vector2(0, yLoc), 0.3f).SetEase(Ease.InOutQuad).SetUpdate(true);
        GrayOutBackground(yLoc == 0 ? true : false);

    }
    public void LoadSettingsPanel(int yLoc)
    {
        ResetAllUI();
        SettingsPanel.transform.DOMove(new Vector2(0, yLoc), 0.3f).SetEase(Ease.InOutQuad).SetUpdate(true);
        GrayOutBackground(yLoc == 0 ? true : false);

    }
    public void LoadControlsPanel(int yLoc)
    {
        ResetAllUI();
        ControlsPanel.transform.DOMove(new Vector2(0, yLoc), 0.3f).SetEase(Ease.InOutQuad).SetUpdate(true);
        GrayOutBackground(yLoc == 0 ? true : false);
    }
    public void LoadCreditsPanel(int yLoc)
    {
        ResetAllUI();
        CreditsPanel.transform.DOMove(new Vector2(0, yLoc), 0.3f).SetEase(Ease.InOutQuad).SetUpdate(true);
        GrayOutBackground(yLoc == 0 ? true : false);
    }

    void GrayOutBackground(bool inOrOut)
    {
        GrayedBackground.SetActive(inOrOut);
    }
    bool GrayedBackgroundIsEnabled()
    {
        return GrayedBackground.activeSelf;
    }
    void ResetAllUI()
    {
        InfoPanel.transform.DOMove(new Vector2(0, -50), 0.3f).SetEase(Ease.InOutQuad).SetUpdate(true);
        ControlsPanel.transform.DOMove(new Vector2(0, -50), 0.3f).SetEase(Ease.InOutQuad).SetUpdate(true);
        SettingsPanel.transform.DOMove(new Vector2(0, -50), 0.3f).SetEase(Ease.InOutQuad).SetUpdate(true);
        CreditsPanel.transform.DOMove(new Vector2(0, -50), 0.3f).SetEase(Ease.InOutQuad).SetUpdate(true);
        GrayOutBackground(false);

    }
}
