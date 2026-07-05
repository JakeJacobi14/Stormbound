using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreyChargeTuto : MonoBehaviour
{
    private TutorialManager TutorialManager;
    public LayerMask chargeLayer; // Assign the layer of your "Charge" object in the Inspector
    private float overlapRadius = 0.3f;

    void Start()
    {
        TutorialManager = GameObject.FindWithTag("TutorialManager").GetComponent<TutorialManager>();
    }

    void Update()
    {
        // Detect overlaps with objects on the specified layer within a circle around this object
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, overlapRadius, chargeLayer);

        foreach (Collider2D hitCollider in colliders)
        {
            // Check if the overlapping collider has the "Charge" tag (optional, but good practice)
            if (hitCollider.CompareTag("Charge") && !hitCollider.gameObject.GetComponent<TutoCharge>().isDragging)
            {
                if (TutorialManager.currentCheckpoint == 0) TutorialManager.UpdateCheckpoints(1);
                if (TutorialManager.currentCheckpoint == 2) TutorialManager.StartCoroutine(TutorialManager.CheckpointOneLoop()); // this is to save some time because his is the loop i want to run, just activating the start button and i dont want to make a checkpoint four into this
                hitCollider.gameObject.GetComponent<TutoCharge>().canBeDragged = false;
                Destroy(gameObject);
                // Optionally, you might want to destroy the other object as well
                // Destroy(hitCollider.gameObject);
                break; // Exit the loop after finding one "Charge" object
            }
        }
    }

}