using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private Image blackBackground;
    [SerializeField] float targetAlpha = 0;
    [SerializeField] bool doFadeIn = false;
    [SerializeField] private AudioSource Music;
    private float StartingMusicVolume;
    // Start is called before the first frame update
    void Start()
    {
        StartingMusicVolume = Music.volume;
        if (doFadeIn) 
        {
            Color currentColor = blackBackground.color;
            currentColor.a = targetAlpha;
            blackBackground.color = currentColor;
            Music.volume = 0;
            StartCoroutine(FadeIn());
        }
    }


    public void FadeOutScene(string SceneName)
    {   
        StartCoroutine(FadeOut(SceneName));
    }
    IEnumerator FadeOut(string SceneName)
    {
        Color currentColor = blackBackground.color;
        for (int i = 0; i < 50; i++)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            targetAlpha += 0.02f;
            Music.volume -= StartingMusicVolume/50f;
            currentColor.a = targetAlpha;

            blackBackground.color = currentColor;
        }
        SceneManager.LoadScene(SceneName);
        
    }
    
    IEnumerator FadeIn()
    {
        Color currentColor = blackBackground.color;
        
        for (int i = 0; i < 50; i++)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            targetAlpha -= 0.02f;
            Music.volume += StartingMusicVolume/50f;
            currentColor.a = targetAlpha;

            blackBackground.color = currentColor;
        }
        
    }
}
