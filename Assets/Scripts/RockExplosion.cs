using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockExplosion : MonoBehaviour
{
    public float RockLength;
    private ParticleSystem ps;
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        
        var shape = ps.shape;

        Vector3 currentScale = shape.scale;

        Vector3 newScale = new Vector3(currentScale.x, RockLength, currentScale.z);

        shape.scale = newScale;
    }
    
}
