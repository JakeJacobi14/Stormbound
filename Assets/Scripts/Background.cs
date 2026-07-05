using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    // very important setting the time scale to 1
    void Start()
    {
        Time.timeScale = 1;
    }
    [SerializeField] private float backgroundSpeed = 1;
    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(-1 * backgroundSpeed, 0, 0) * Time.deltaTime;
        if (transform.position.x <= -170.5f)
        {
            transform.position = new Vector2(79.4f, 3.6f);
        }
    }
}
