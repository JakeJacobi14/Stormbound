using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoManager : MonoBehaviour
{
    // Maximum number of states we can store
    // 48 saves +1 because when 1 undo is left theres no more undos
    private const int MAX_UNDO_STATES = 49;
    [SerializeField] private AudioClip errorSFX;

    // Class to store a complete snapshot of the screen
    [System.Serializable]
    public class ScreenState
    {
        public List<Vector2> positions = new List<Vector2>();
        public List<bool> isPositive = new List<bool>();

        public ScreenState() { }

        // Deep copy constructor
        public ScreenState(ScreenState other)
        {
            if (other == null) return;
            positions = new List<Vector2>(other.positions);
            isPositive = new List<bool>(other.isPositive);
        }
    }

    // Wrapper classes for JSON serialization
    [System.Serializable]
    public class BestSolutionEntry
    {
        public int levelIndex;
        public ScreenState screenState;
    }

    [System.Serializable]
    public class BestSolutionsContainer
    {
        public List<BestSolutionEntry> entries = new List<BestSolutionEntry>();
    }

    // Prefab references
    public GameObject positiveChargePrefab;
    public GameObject negativeChargePrefab;

    // List of states and a pointer to the current state.
    private List<ScreenState> states = new List<ScreenState>();
    public Dictionary<int, ScreenState> bestSolutions = new Dictionary<int, ScreenState>();
    private int currentStateIndex = -1;

    // Key used to save our data in PlayerPrefs.
    private const string SaveKey = "BestSolutionsData";

    private void Awake()
    {
        // Load saved best solutions from disk.
        LoadBestSolutionsFromDisk();
        // Capture the initial state.
        SaveScreen();
    }

    public void SaveLevelsBeaten(int levelIndex)
    {
        ScreenState newState = CaptureCurrentState();
        bestSolutions[levelIndex] = newState;
        Debug.Log("Saved level. Jake is the coolest");
        // Save updated best solutions to disk.
        SaveBestSolutionsToDisk();
    }

    public void LoadBestSolution(int levelIndex)
    {
        if (bestSolutions.ContainsKey(levelIndex))
        {
            RestoreState(bestSolutions[levelIndex]);
        }
        else
        {
            Debug.LogWarning("No saved solution for level " + levelIndex);
        }
    }

    // Save the bestSolutions dictionary to disk using JSON.
    public void SaveBestSolutionsToDisk()
    {
        BestSolutionsContainer container = new BestSolutionsContainer();
        foreach (var kvp in bestSolutions)
        {
            BestSolutionEntry entry = new BestSolutionEntry();
            entry.levelIndex = kvp.Key;
            entry.screenState = kvp.Value;
            container.entries.Add(entry);
        }
        string json = JsonUtility.ToJson(container);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
        Debug.Log("Best solutions saved to disk.");
    }

    // Load the bestSolutions dictionary from disk.
    public void LoadBestSolutionsFromDisk()
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            string json = PlayerPrefs.GetString(SaveKey);
            BestSolutionsContainer container = JsonUtility.FromJson<BestSolutionsContainer>(json);
            bestSolutions.Clear();
            foreach (var entry in container.entries)
            {
                bestSolutions[entry.levelIndex] = entry.screenState;
            }
            Debug.Log("Loaded best solutions from disk.");
        }
        else
        {
            Debug.Log("No best solutions data found on disk.");
        }
    }
    public void ResetAllSaves()
    {
        // Clear best solutions dictionary
        bestSolutions.Clear();
        // Remove the saved data from PlayerPrefs
        PlayerPrefs.DeleteKey(SaveKey);
        PlayerPrefs.Save();
        Debug.Log("Best solution saves have been reset.");

        // Optional: Reset the undo states if desired.
        states.Clear();
        currentStateIndex = -1;
        SaveScreen();  // Capture a fresh starting state.
        Debug.Log("Undo states have also been reset.");
    }
    // Call this whenever a change happens (move, add, delete charge)
    public void SaveScreen()
    {
        // If we're not at the end of our list, remove all "future" states.
        if (currentStateIndex < states.Count - 1)
        {
            states.RemoveRange(currentStateIndex + 1, states.Count - currentStateIndex - 1);
        }

        ScreenState newState = CaptureCurrentState();

        // Prevent duplicate states.
        if (currentStateIndex >= 0 && AreStatesEqual(states[currentStateIndex], newState))
        {
            return;
        }

        states.Add(newState);
        currentStateIndex++;

        // Enforce maximum number of states.
        if (states.Count > MAX_UNDO_STATES)
        {
            int removeCount = states.Count - MAX_UNDO_STATES;
            states.RemoveRange(0, removeCount);
            currentStateIndex -= removeCount;
            if (currentStateIndex < 0) currentStateIndex = 0;
        }

        Debug.Log("Saved state. CurrentStateIndex: " + currentStateIndex + ", Total states: " + states.Count);
    }

    // Captures the current screen state and returns it.
    private ScreenState CaptureCurrentState()
    {
        ScreenState newState = new ScreenState();
        GameObject[] charges = GameObject.FindGameObjectsWithTag("Charge");

        foreach (GameObject charge in charges)
        {
            ExternalCharge chargeComponent = charge.GetComponent<ExternalCharge>();
            if (chargeComponent != null)
            {
                newState.positions.Add(charge.transform.position);
                newState.isPositive.Add(chargeComponent.isPositive);
            }
        }
        return newState;
    }

    // Helper method to compare two states.
    private bool AreStatesEqual(ScreenState a, ScreenState b)
    {
        if (a.positions.Count != b.positions.Count || a.isPositive.Count != b.isPositive.Count)
        {
            return false;
        }
        for (int i = 0; i < a.positions.Count; i++)
        {
            if (a.positions[i] != b.positions[i] || a.isPositive[i] != b.isPositive[i])
            {
                return false;
            }
        }
        return true;
    }

    // Clears all saved states (both undo and redo), typically when loading a new level.
    public void ClearSaves()
    {
        states.Clear();
        currentStateIndex = -1;
        SaveScreen(); // Capture the current state as the new starting point.
    }

    // Call this when the undo button is pressed.
    public void Undo()
    {
        if (GameObject.FindWithTag("GameController").GetComponent<GameController>().IsGameRunning()) 
        {
            // play error sfx
            AudioSource.PlayClipAtPoint(errorSFX, Vector2.zero);
            return;
        }
        if (currentStateIndex <= 0)
        {
            Debug.Log("Nothing to undo.");
            AudioSource.PlayClipAtPoint(errorSFX, Vector2.zero);
            RestoreState(states[currentStateIndex]);
            return;
        }
        // play pop sfx
        AudioSource.PlayClipAtPoint(GameObject.FindWithTag("GameController").GetComponent<GameController>().RandomPopSound(), Vector2.zero);
        currentStateIndex--;
        RestoreState(states[currentStateIndex]);
        Debug.Log("Undo performed. CurrentStateIndex: " + currentStateIndex);
    }

    // Call this when the redo button is pressed.
    public void Redo()
    {
        if (GameObject.FindWithTag("GameController").GetComponent<GameController>().IsGameRunning()) 
        {
            // play error sfx
            AudioSource.PlayClipAtPoint(errorSFX, Vector2.zero);
            return;
        }
        if (currentStateIndex >= states.Count - 1)
        {
            Debug.Log("Nothing to redo.");
            AudioSource.PlayClipAtPoint(errorSFX, Vector2.zero);
            RestoreState(states[currentStateIndex]);
            return;
        }
        // play pop sfx
        AudioSource.PlayClipAtPoint(GameObject.FindWithTag("GameController").GetComponent<GameController>().RandomPopSound(), Vector2.zero);
        currentStateIndex++;
        RestoreState(states[currentStateIndex]);
        Debug.Log("Redo performed. CurrentStateIndex: " + currentStateIndex);
    }

    // Restores the screen to match the given state.
    public void RestoreState(ScreenState state)
    {
        // First, remove all existing charges.
        GameObject[] currentCharges = GameObject.FindGameObjectsWithTag("Charge");
        foreach (GameObject charge in currentCharges)
        {
            Destroy(charge);
        }

        // Then recreate charges from the saved state.
        for (int i = 0; i < state.positions.Count; i++)
        {
            Vector2 position = state.positions[i];
            bool isPositive = state.isPositive[i];

            GameObject prefabToUse = isPositive ? positiveChargePrefab : negativeChargePrefab;
            if (prefabToUse != null)
            {
                Instantiate(prefabToUse, position, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Charge prefab is missing! Make sure to assign both positive and negative prefabs in the inspector.");
            }
        }
    }
}
