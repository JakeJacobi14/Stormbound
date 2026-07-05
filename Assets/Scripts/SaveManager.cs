using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    // Save data: highest level completed and star rating for each level.
    public int highestLevelCompleted = 1;
    public Dictionary<int, int> levelStars = new Dictionary<int, int>();

    // Total number of levels (adjust as needed).
    private const int totalLevels = 100;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (SettingsManager.ResetSave)
            {
                SettingsManager.ResetSave = false;
                ResetSave(false);
            }
            LoadProgress();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LevelCompleted(int level, int stars)
    {
        // Update highest level if applicable.
        if (level > highestLevelCompleted)
        {
            highestLevelCompleted = level;
        }

        // If we already saved a star rating for this level, update if the new rating is higher.
        if (levelStars.ContainsKey(level))
        {
            if (stars < levelStars[level])
            {
                levelStars[level] = stars;
            }
        }
        else
        {
            levelStars.Add(level, stars);
        }

        SaveProgress();
    }

    public void SaveProgress()
    {
        PlayerPrefs.SetInt("HighestLevel", highestLevelCompleted);
        foreach (KeyValuePair<int, int> entry in levelStars)
        {
            PlayerPrefs.SetInt("LevelStars_" + entry.Key, entry.Value);
        }
        PlayerPrefs.Save();
        Debug.Log("Progress saved: Highest Level = " + highestLevelCompleted);
    }

    public void LoadProgress()
    {
        if (PlayerPrefs.HasKey("HighestLevel"))
        {
            highestLevelCompleted = PlayerPrefs.GetInt("HighestLevel");
        }
        else
        {
            highestLevelCompleted = 1;
        }

        // Loop through levels to load their star ratings.
        for (int i = 1; i <= totalLevels; i++)
        {
            string key = "LevelStars_" + i;
            if (PlayerPrefs.HasKey(key))
            {
                int stars = PlayerPrefs.GetInt(key);
                levelStars[i] = stars;
            }
        }
        Debug.Log("Level stars loaded.");
    }
    public void ResetSave(bool changeScene = true)
    {
        PlayerPrefs.DeleteAll();
        GameObject.FindWithTag("UndoManager").GetComponent<UndoManager>().ResetAllSaves();
        highestLevelCompleted = 1; // or the default starting level (often 1)
        levelStars.Clear();
        PlayerPrefs.Save();
        Debug.Log("Save data reset.");
        if (changeScene) SceneManager.LoadScene("TitleScene");
    }
    
}
