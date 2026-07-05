using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    private GameObject player;
    private PlayerScript playerScript;
    public PlayerHookScript playerHookScript;
    private Rigidbody2D playerRB;
    private bool isClose;
    private GameObject cHook;
    public ParticleSystem Targets;
    
    [SerializeField] float HookRange = 15;
    public float HookXPower = 10;
    public float HookYPower = 6;

    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHookScript = player.GetComponent<PlayerHookScript>();
        playerRB = player.GetComponent<Rigidbody2D>();
        Targets.GetComponent<ParticleSystem>();  
        
    }

   
    // Update is called once per frame
    void Update()
    {
        
        cHook = playerHookScript.currentHook;
        isClose = false;
        var dis = Vector2.Distance(player.transform.position, transform.position);
        if (dis < HookRange)
        {
            isClose = true;
        }
        var emission = Targets.emission;
        if (isClose)
        {
            if (cHook)
            {              
                var cHookDis = Vector2.Distance(player.transform.position, cHook.transform.position);               
                if (dis<cHookDis)
                {
                    emission.enabled = true;
                    playerHookScript.ChangeCurrentHook(gameObject);
                } else
                {
                     emission.enabled = true;
                }
                    
            }
            else {
                emission.enabled = true;
                playerHookScript.ChangeCurrentHook(gameObject);
                
            }
                
        }
            
        
        else
        {      
            if (cHook = gameObject)
            {
                cHook = null;
            }

            emission.enabled = false;
        }

    }


}
