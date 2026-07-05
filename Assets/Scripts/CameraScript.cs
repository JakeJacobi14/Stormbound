using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public GameObject player;
    public Camera Camera;
    public bool isFollowing = true;
    private Vector3 target;

    public float Zoom = 5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(followPlayer());
    }

    IEnumerator followPlayer()
    {
        while (true)
        {

            if (isFollowing)
            {

                target = new Vector3(player.transform.position.x, player.transform.position.y,-10);
                transform.position = Vector3.Lerp(transform.position, target, 0.015f);
            }



            yield return new WaitForSeconds(0.008f);
        }
    }

    // Update is called once per frame
    void Update()
    {

       


    }
    public void startSmoothZoom(float zoomSpeed)
    {
        StartCoroutine(SmoothZoom(zoomSpeed));
    }
    public IEnumerator SmoothZoom(float zoomSpeed)
    {
      

        while (Mathf.Abs(Camera.orthographicSize - Zoom) > 0.01f)
        {
            Camera.orthographicSize = Mathf.Lerp(Camera.orthographicSize, Zoom, zoomSpeed);
            yield return null;
        }

        Camera.orthographicSize = Zoom;
        
    }

}
