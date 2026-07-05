using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBlock : MonoBehaviour
{
    [SerializeField] private AudioClip boing1SFX;
    [SerializeField] private AudioClip boing2SFX;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Test Charge"))
        {
            // 0 inclusive 2 exclusive means 0 or 1
            AudioClip boingAudio = Random.Range(0, 2) == 0 ? boing1SFX : boing2SFX;
            AudioSource.PlayClipAtPoint(boingAudio, Vector2.zero);
        }
    }
}
