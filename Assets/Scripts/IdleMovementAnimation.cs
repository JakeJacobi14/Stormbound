using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleMovementAnimation : MonoBehaviour
{

    // taken from a youtube tutorial
    [Header("Motion Settings")]
    [SerializeField] float maxOffset = 0.5f;

    [SerializeField] float noiseSpeed = 0.15f;

    [SerializeField] float xNoiseScale = 1.5f;

    [SerializeField] float yNoiseScale = 1.2f;
    
    [SerializeField] GameObject OptionalOtherObject;
    
    private Vector3 _startPosition;
    private Vector3 _startPosition2;
    private float _timeX;
    private float _timeY;
    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
        if (OptionalOtherObject != null) _startPosition2 = OptionalOtherObject.transform.position;
        _timeX = Random.Range(0f, 100f);
        _timeY = Random.Range(0f, 100f);
    }

    // Update is called once per frame
    void Update()
    {
        _timeX += Time.deltaTime * noiseSpeed;
        _timeY += Time.deltaTime * noiseSpeed;

        // Sample 2D Perlin noise for smooth, organic movement
        float noiseX = Mathf.PerlinNoise(_timeX * xNoiseScale, 0f);
        float noiseY = Mathf.PerlinNoise(0f, _timeY * yNoiseScale);

        // Remap the noise from 0-1 to -maxOffset/2 to +maxOffset/2 for centered movement
        float offsetX = (noiseX - 0.5f) * maxOffset * 2f;
        float offsetY = (noiseY - 0.5f) * maxOffset * 2f;

        // Apply the offset to the starting position
        transform.position = _startPosition + new Vector3(offsetX, offsetY, 0f);
        if (OptionalOtherObject != null)
        {
            OptionalOtherObject.transform.position = _startPosition2 + new Vector3(offsetX, offsetY, 0f);
        }
    }
}
