using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearAfterDelay : MonoBehaviour
{
    
    [SerializeField] private float delayToAppear;
    [SerializeField] GameObject objectToAppear;

    void Start()
    {
        objectToAppear.SetActive(false);
        StartCoroutine(Appear(delayToAppear));
    }

    IEnumerator Appear(float delay)
    {
        yield return new WaitForSeconds(delay);
        objectToAppear.SetActive(true);
    }
}
