using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropCharge : MonoBehaviour
{
    public GameObject objectToSpawn;
    private GameObject spawnedObject;
    private bool isDragging = false;
    private GameController GameController;
    private UndoManager UndoManager;
    [SerializeField] private LayerMask chargeLayerMask;
    void Start()
    {
        GameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        UndoManager = GameObject.FindWithTag("UndoManager").GetComponent<UndoManager>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !GameController.IsGameRunning())
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, chargeLayerMask);

            if (hit.collider != null && hit.transform == transform)
            {
                // if (GameController.NumCharges >= GameController.MaxCharges) return; 
                spawnedObject = Instantiate(objectToSpawn, mousePos, Quaternion.identity);
                spawnedObject.GetComponent<ExternalCharge>().SpawnedIn = true;
                isDragging = true;
                // GameController.NumCharges++;
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
            UndoManager.SaveScreen();

        }
    }
    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("Charge"))
    //     {
    //         Destroy(other);
    //     }
    // }
}
