using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropTuto : MonoBehaviour
{
    public GameObject objectToSpawn;
    private GameObject spawnedObject;
    private bool isDragging = false;
    [SerializeField] TutorialManager TutorialManager;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.transform == transform && !TutorialManager.isInGame)
            {
                spawnedObject = Instantiate(objectToSpawn, mousePos, Quaternion.identity);
                spawnedObject.GetComponent<TutoCharge>().SpawnedIn = true;
                spawnedObject.GetComponent<TutoCharge>().isDragging = true;

                isDragging = true;
                if (TutorialManager.currentCheckpoint < 3) gameObject.SetActive(false);

            }
        }

        if (isDragging && spawnedObject != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawnedObject.transform.position = mousePos;
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            spawnedObject = null;

        }
    }
}
