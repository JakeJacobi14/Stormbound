using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoTestCharge : MonoBehaviour
{
    private Rigidbody2D rb;
    public float MaxSpeed;
    [SerializeField] TutorialManager TutorialManager;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        TutorialManager = GameObject.FindWithTag("TutorialManager").GetComponent<TutorialManager>();
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
            if (TutorialManager.currentCheckpoint == 6)
            {
                TutorialManager.UpdateCheckpoints(7);
            }
            else if (TutorialManager.currentCheckpoint == 5)
            {
                TutorialManager.UpdateCheckpoints(6);
            }
            else if (TutorialManager.currentCheckpoint == 4)
            {
                TutorialManager.UpdateCheckpoints(5);
            }
            else if (TutorialManager.currentCheckpoint == 3)
            {
                TutorialManager.UpdateCheckpoints(4);
            }
            else if (TutorialManager.currentCheckpoint == 2)
            {
                TutorialManager.UpdateCheckpoints(3);
            }
            else if (TutorialManager.currentCheckpoint == 1)
            {
                TutorialManager.UpdateCheckpoints(2);
            }
        }
        if (other.gameObject.CompareTag("Wall"))
        {
            TutorialManager.Collision();
        }
    
    }
}