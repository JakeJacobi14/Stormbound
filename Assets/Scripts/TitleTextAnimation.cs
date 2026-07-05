using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TitleTextAnimation : MonoBehaviour
{
    [Header("Title Texts")]
    [SerializeField] private TMP_Text TitleText;
    [SerializeField] private TMP_Text TitleText2;

    [Header("Settings")]
    [SerializeField] private string GameTitle;
    [SerializeField] private float TypeSpeedDelay;


    void Start()
    {
        TitleText.text = "";
        TitleText2.text = "";
        StartCoroutine(TitleTextAnim(GameTitle, TypeSpeedDelay));
    }
    
    
    IEnumerator TitleTextAnim(string word, float typeDelay)
    {
        string emptyWord = "";
        foreach (char letter in word)
        {
            emptyWord+=letter;
            StartCoroutine(TextCoroutine(TitleText, TitleText2, emptyWord, typeDelay));
            yield return new WaitForSeconds(typeDelay);
        }
    }
    IEnumerator TextCoroutine(TMP_Text textToAnim, TMP_Text textToAnim2, string textToShow, float typeDelay)
    {
        yield return new WaitForSeconds(typeDelay);
        textToAnim.text = textToShow;
        textToAnim2.text = textToShow;
    }
}
