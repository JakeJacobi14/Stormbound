using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public CameraScript PlayerCameraScript;
    public float SetZoom;
    public float zoomSpeed = 0.015f;

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            if (other.CompareTag("Player"))
            {
                PlayerCameraScript.Zoom = SetZoom;
                PlayerCameraScript.startSmoothZoom(zoomSpeed);
            }


        }
    }

    


}
