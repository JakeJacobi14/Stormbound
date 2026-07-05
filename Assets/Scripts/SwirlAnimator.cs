using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwirlAnimator : MonoBehaviour
{
    public Material swirlMaterial;  // Assign your material with the swirl shader.
    public float spinSpeed = 1.0f;    // Speed factor for the spin animation.
    private float spinTime = 0f;

    void Update()
    {
        // Increment spinTime over time.
        spinTime += Time.deltaTime * spinSpeed;
        // Update the shader property.
        swirlMaterial.SetFloat("_SpinTime", spinTime);
    }
}