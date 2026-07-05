using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHookScript : MonoBehaviour
{
    public GameObject currentHook;
    public LineRenderer lineR;

    private GrapplingHook currentHookScript;

    public GameObject ropeHingeAnchor;
    public DistanceJoint2D ropeJoint;
    private Rigidbody2D ropeHingeAnchorRb;

    [SerializeField] float HookXPower = 10;
    [SerializeField] float HookYPower = 6;

    private PlayerScript playerScript;
    // Start is called before the first frame update
    void Start()
    {
        ropeJoint.enabled = false;
        playerScript = gameObject.GetComponent<PlayerScript>();
        lineR.SetPosition(0, transform.position);
        ropeHingeAnchorRb = ropeHingeAnchor.GetComponent<Rigidbody2D>();
    }

    private void ToggleRope(bool toggle)
    {
        lineR.enabled = toggle;
        ropeJoint.enabled = toggle;
        playerScript.isGrappling = toggle;
    }

    public void ChangeCurrentHook(GameObject hook)
    {
        
        

        if (Input.GetMouseButtonDown(1))
        {
            currentHook = hook;
            currentHookScript = hook.GetComponent<GrapplingHook>();

            HookXPower = currentHookScript.HookXPower;
            HookYPower = currentHookScript.HookYPower;
        }
            
            

            
        
        

    }
    // Update is called once per frame
    void Update()
    {
        lineR.SetPosition(0, transform.position);
        if (currentHook != null)
        {
            
            if (Input.GetMouseButton(1))
            {
                lineR.SetPosition(1, currentHook.transform.position);
                ToggleRope(true);
                Vector3 pullVector = new Vector3((currentHook.transform.position.x - transform.position.x) * HookXPower * Time.deltaTime, (currentHook.transform.position.y - transform.position.y) * HookYPower * Time.deltaTime, 0);
                transform.GetComponent<Rigidbody2D>().AddForce(pullVector, ForceMode2D.Impulse);
                ropeHingeAnchorRb.transform.position = currentHook.transform.position;
                ropeJoint.distance = Vector2.Distance(transform.position, currentHook.transform.position);


            }

            if (Input.GetMouseButtonUp(1))
            {
                ToggleRope(false);
                currentHook = null;
            }
        }
        
    }
}
