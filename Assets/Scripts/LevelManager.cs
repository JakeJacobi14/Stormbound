using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    public GameObject TestCharge;
    private GameController GameController;
    private UndoManager UndoManager;
    public int currentLevelIndex = 2;  
    public GameObject LevelScreenPanel;
    public GameObject ChargeBoxes;
    public Vector2 LevelScreenPanelStart;
    public Vector2 LevelsLocation;
    public GameObject[] LevelsScreens;
    private int whichLevelScreenAreWeOn = 0;
    [SerializeField] private Sprite EmptyStarSprite;
    [SerializeField] private Sprite FullStarSprite;
    public GameObject[] LevelButtons;
    [SerializeField] private AudioClip ErrorSFX;
    public Dictionary<int, int> levelCharges = new Dictionary<int, int>();

    // List of levels, configurable in the Inspector
    [Space(10)]
    [Header("LEVELS!")]
    public bool AllLevelsMode;
    [Space(10)]

    public List<Level> levels;
    

    void Start()
    {
        GameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        UndoManager = GameObject.FindWithTag("UndoManager").GetComponent<UndoManager>();
        // LevelButtons = GameObject.FindGameObjectsWithTag("Level");
        if (!AllLevelsMode) 
        {
            LoadLevel(2);
            GameController.LoadLevelSelectPanel();

        }
        // so the locks arent there
        if (AllLevelsMode)
        {
            LoadLevel(2);
            SaveManager.Instance.highestLevelCompleted = 100; // so they can go to all levels
        }
    }
    public void LoadRealLevel(int levelIndex)
    {
        // Debug.Log(GameController.LevelsBeaten + " and " + levelIndex);
        if (levelIndex-1 > GameController.LevelsBeaten && !AllLevelsMode) 
        {
            Debug.Log("You don't have access to the next level yet!");
            AudioSource.PlayClipAtPoint(ErrorSFX, Vector2.zero);
            return;
        }
        ChargeBoxes.SetActive(true);
        LevelScreenPanel.transform.position = LevelScreenPanelStart;
        Camera.main.GetComponent<CameraZoom>().ResetCamera();
        LoadLevel(levelIndex);
        UndoManager.ClearSaves();
        UndoManager.LoadBestSolution(levelIndex);


    }
    public void UpdateLevelScreenStars()
    {
        for (int j = 0; j < LevelButtons.Length; j++)
        {
            List<GameObject> Stars = new List<GameObject>();
            GameObject lockObject = null;
            Transform[] allChildren = LevelButtons[j].GetComponentsInChildren<Transform>();

            foreach (Transform child in allChildren)
            {
                if (child.CompareTag("Star"))
                {
                    Stars.Add(child.gameObject);
                }
                if (child.CompareTag("Lock"))
                {
                    lockObject = child.gameObject;
                }
            }

            for (int i = 0; i < Stars.Count; i++)
            {
                Stars[i].GetComponent<SpriteRenderer>().sprite = EmptyStarSprite;
            }

            int savedStars = 0;
            int levelNumber = j; // Levels are 1-based.
            if (SaveManager.Instance.levelStars.ContainsKey(levelNumber))
            {
                savedStars = SaveManager.Instance.levelStars[levelNumber];
            }

            int starsToFill = Mathf.Min(savedStars, Stars.Count);
            for (int i = 0; i < starsToFill; i++)
            {
                Stars[i].GetComponent<SpriteRenderer>().sprite = FullStarSprite;
            }

            if (lockObject != null)
            {
                if (levelNumber-1 > SaveManager.Instance.highestLevelCompleted)
                {
                    lockObject.SetActive(true);
                    lockObject.transform.localPosition = new Vector3(lockObject.transform.localPosition.x, lockObject.transform.localPosition.y, 0);
                }
                else
                {
                    lockObject.SetActive(false);
                }
            }
        }
    }




    public void LoadLevel(int levelIndex, bool Reload = false)
    {
        if (levelIndex < 0 || levelIndex >= levels.Count)
        {
            Debug.LogError("Level index out of bounds!");
            return;
        }
        // update level index
        currentLevelIndex = levelIndex;
        ClearCurrentLevel();
        Level levelToLoad = levels[levelIndex];
        Instantiate(levelToLoad.obstaclesPrefab);
        // TestCharge.transform.position = levelToLoad.StartingLoc;
        GameController.testChargeStartPos = levelToLoad.StartingLoc;

        GameController.LevelLoaded(Reload);
        
        // GameController.MaxCharges = levelToLoad.maxCharges;
        GameController.ThreeStarsCharges = levelToLoad.ThreeStarsCharges;
        GameController.TwoStarsCharges = levelToLoad.TwoStarsCharges;

        GameController.UpdateSomeTexts(levelToLoad.levelNum);

        Debug.Log($"Loaded level: {levelIndex-1} with 3 star: {levelToLoad.ThreeStarsCharges}");
    }
    public void ReloadLevel()
    {
        LoadLevel(currentLevelIndex, true);
    }
    public void NextLevel()
    {
        if (currentLevelIndex > GameController.LevelsBeaten && !AllLevelsMode) 
        {
            Debug.Log("You don't have access to the next level yet!");
            AudioSource.PlayClipAtPoint(ErrorSFX, Vector2.zero);
            return;
        }
        if (currentLevelIndex + 1 < levels.Count)
        {
            currentLevelIndex++;
            LoadRealLevel(currentLevelIndex);
        }
        else
        {
            Debug.Log("No more levels!");
            AudioSource.PlayClipAtPoint(ErrorSFX, Vector2.zero);
        }
    }

    public void PreviousLevel()
    {
        if (currentLevelIndex - 1 >= 0)
        {
            currentLevelIndex--;
            LoadRealLevel(currentLevelIndex);
        }
        else
        {
            Debug.Log("This is the first level!");
            AudioSource.PlayClipAtPoint(ErrorSFX, Vector2.zero);
        }
    }

    private void ClearCurrentLevel()
    {
        // Remove all existing obstacles in the scene
        // foreach (GameObject wall in GameObject.FindGameObjectsWithTag("Wall"))
        // {
        //     Destroy(wall);
        // }
        // foreach (GameObject goal in GameObject.FindGameObjectsWithTag("Goal"))
        // {
        //     Destroy(goal);
        // }
        foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            Destroy(obstacle);
        }
        // foreach (GameObject charge in GameObject.FindGameObjectsWithTag("Charge"))
        // {
        //     Destroy(charge);
        // }
    }

    public void TransitionLevelScreens(int modifier)
    {
        if (whichLevelScreenAreWeOn + modifier < 0 || whichLevelScreenAreWeOn + modifier >= LevelsScreens.Length) return;
        LevelsScreens[whichLevelScreenAreWeOn].transform.DOLocalMove(LevelsScreens[whichLevelScreenAreWeOn].transform.position + new Vector3(2000 * -modifier, 0, 0), 0.5f).SetEase(Ease.OutQuad).SetUpdate(true);

        whichLevelScreenAreWeOn += modifier;
        LevelsScreens[whichLevelScreenAreWeOn].transform.DOLocalMove(LevelsLocation, 0.4f).SetEase(Ease.OutQuad).SetUpdate(true);

    }
    public void KillTweens()
    {
        foreach (GameObject thingy in LevelsScreens)
        {
            // DOTween.Kill(thingy);
            // thingy.transform.localPosition = new Vector3(0, -150, 0);   
            // thingy.transform.position = new Vector3(thingy.transform.position.x, LevelScreenPanel.transform.position.y, thingy.transform.position.z);
        }
    
    }

}
