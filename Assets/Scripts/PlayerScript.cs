using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private float HorizontalInput;
    public float speed = 6f;
    public float jumpPower = 8f;
    private bool isFacingRight = true;
    public bool isGrappling = false;
    public float maxSpeed = 50f;
    public float GravMultiplier = 0.5f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private LayerMask groundLayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * GravMultiplier);
        }


    }

    private bool isGrounded()
    {
        return Physics2D.OverlapBox(groundChecker.position,new Vector2(0.9f,0.2f),0f,groundLayer);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x,-maxSpeed,maxSpeed),rb.velocity.y);
        if (!isGrappling)
        {
         


            if (Mathf.Abs(rb.velocity.x) <= speed)
            {
                rb.velocity = new Vector2(HorizontalInput * speed, rb.velocity.y);
            }
            else if (HorizontalInput != 0f)
            {
                if (rb.velocity.x > 0f && HorizontalInput < 0f)
                {
                    rb.velocity += new Vector2(HorizontalInput * speed/2, 0);
                }
                else if (rb.velocity.x < 0f && HorizontalInput > 0f)
                {
                    rb.velocity += new Vector2(HorizontalInput * speed/2, 0);
                } 
                
                
            } 
            
            if (Mathf.Abs(rb.velocity.x) >= speed)
            {
                rb.velocity = new Vector2(rb.velocity.x * 0.97f, rb.velocity.y);
            }
            
        }
        else if (HorizontalInput != 0f)
        {
            //rb.velocity = new Vector2(HorizontalInput * speed, rb.velocity.y);
        }
        

    }

}
