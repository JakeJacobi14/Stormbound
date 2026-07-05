using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestCharge : MonoBehaviour
{
    private Rigidbody2D rb;
    public float MaxSpeed;
    public TMP_Text CollisionText;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // add this back if the ball ever gets way too fast vvvvv
        // rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -MaxSpeed, MaxSpeed), Mathf.Clamp(rb.velocity.y, -MaxSpeed, MaxSpeed));
        // Debug.Log($"X: {rb.velocity.x}    Y: {rb.velocity.y}");
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            GameObject.FindWithTag("GameController").GetComponent<GameController>().Score();
        }
        if (other.gameObject.CompareTag("Wall"))
        {
            GameObject.FindWithTag("GameController").GetComponent<GameController>().Collision();
        }
    
    }
}
