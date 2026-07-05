using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargePlacer : MonoBehaviour
{
    public KeyCode NegativeKeycode;
    public KeyCode PositiveKeycode;
    public KeyCode PauseKeycode;
    public KeyCode RestartKeycode;
    public KeyCode RetryKeycode;
    public GameObject NegativeCharge;
    public GameObject PositiveCharge;
    private Vector3 screenPosition;
    private Vector3 worldPosition;
    private KeyCode PauseMenuKeycode = KeyCode.Escape;
    private GameController GameController;
    private UndoManager UndoManager;
    public LevelManager levelManager;

    private bool hasExecuted = false;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        GameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        UndoManager = GameObject.FindWithTag("UndoManager").GetComponent<UndoManager>();

    }

    // Update is called once per frame
    void Update()
    {
        screenPosition = Input.mousePosition;
        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPosition.z = 0;
        if (Input.GetKeyDown(RestartKeycode) && !GameController.IsInUI)
        {
            GameController.Restart();
        }
        if (Input.GetKeyDown(RetryKeycode) && !GameController.IsInUI)
        {
            GameController.Retry();
        }
        if (Input.GetKeyDown(PauseKeycode) && !GameController.IsInUI)
        {
            GameController.StartGame();
        }
        if (Input.GetKeyDown(NegativeKeycode) && !GameController.IsGameRunning() && !GameController.IsInUI)
        {
            // if (GameController.NumCharges >= GameController.MaxCharges) return;
            GameObject charge = Instantiate(NegativeCharge, worldPosition, Quaternion.identity);
            charge.GetComponent<ExternalCharge>().SpawnedIn = true;
            // GameController.NumCharges++;
            UndoManager.SaveScreen();
        }
        if (Input.GetKeyDown(PositiveKeycode) && !GameController.IsGameRunning() && !GameController.IsInUI)
        {
            // if (GameController.NumCharges >= GameController.MaxCharges) return;
            GameObject charge = Instantiate(PositiveCharge, worldPosition, Quaternion.identity);
            charge.GetComponent<ExternalCharge>().SpawnedIn = true;
            // GameController.NumCharges++;
            UndoManager.SaveScreen();

        }
        if (Input.GetKeyDown(KeyCode.E) && !GameController.IsInUI)
        {
            levelManager.NextLevel();
        }
        if (Input.GetKeyDown(KeyCode.Q) && !GameController.IsInUI)
        {
            levelManager.PreviousLevel();
        }
        if (Input.GetKeyDown(PauseMenuKeycode))
        {
            GameController.PauseMenu();
        }
        // undo and ctrl + z but to make sure it doesnt happen every frame
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && !GameController.IsInUI)
        {
            if (Input.GetKeyDown(KeyCode.Z) && !hasExecuted)
            {
                UndoManager.Undo();
                hasExecuted = true;
            }
            if (Input.GetKeyDown(KeyCode.Y) && !hasExecuted)
            {
                UndoManager.Redo();
                hasExecuted = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.Y))
        {
            hasExecuted = false;
        }
        
    }
}
