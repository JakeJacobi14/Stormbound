using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [HideInInspector] public bool checkpoint1;
    [HideInInspector] public bool checkpoint2;
    [HideInInspector] public bool checkpoint3;
    [HideInInspector] public bool checkpoint4;
    [HideInInspector] public bool checkpoint5;
    [HideInInspector] public bool checkpoint6;
    [HideInInspector] public bool checkpoint7;

    [HideInInspector] public int currentCheckpoint = 0;
    [HideInInspector] public bool isInGame;
    private GameObject firstCharge;
    private Vector2 testChargeStartPos = new Vector2(-12, 0);
    [Header("Objects")]
    [SerializeField] GameObject NegativeChargeBox;
    [SerializeField] GameObject PositiveChargeBox;
    [SerializeField] GameObject GreyPositiveCharge;
    [SerializeField] GameObject GreyNegativeCharge;
    [SerializeField] Button StartButton;
    [SerializeField] Button ResetButton;
    [SerializeField] Button ClearButton;
    [SerializeField] Image BlackBackground;
    [SerializeField] GameObject TestCharge;
    [SerializeField] TrailRenderer Trail;
    [SerializeField] Sprite DamagedSprite;
    [SerializeField] Sprite NormalSprite;
    private bool hasCollided = false;

    [Header("Particles")]
    [SerializeField] ParticleSystem LightRain;
    [Header("Typing Settings")]
    [SerializeField] TMP_Text mainText;
    [SerializeField] float typingDelay;

    [Header("Audio")]
    private AudioSource audioSrc;
    [SerializeField] AudioSource RainSFX;
    [SerializeField] AudioSource ThunderSFX;
    [SerializeField] AudioClip keyPressClips;
    [SerializeField] Vector2 pitchRange = new Vector2(0.9f, 1.2f);
    [SerializeField] AudioClip WinSFX;

    [Header("Obstacles")]
    [SerializeField] GameObject Obstacles1;
    [SerializeField] GameObject Obstacles2;
    [SerializeField] GameObject Obstacles3;
    [SerializeField] GameObject Obstacles4;

    // Added for typing skip
    private bool isTyping = false;
    private bool skipTyping = false;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        isInGame = false;
        checkpoint1 = false;
        checkpoint2 = false;
        checkpoint3 = false;
        checkpoint4 = false;
        checkpoint5 = false;
        checkpoint6 = false;
        checkpoint7 = false;
        currentCheckpoint = 0;
        audioSrc = GetComponent<AudioSource>();
        // Time.timeScale = 1;
        StartCoroutine(MainTutorialLoop());
    }

    // Update is called once per frame
    void Update()
    {
        CheckCheckpoints();

        // Check for spacebar input to skip typing
        if (Input.GetKeyDown(KeyCode.Space) && isTyping)
        {
            skipTyping = true;
             // Optional: stop the typing sound immediately
            // Assuming audioSrc is used for typing sounds
            if (audioSrc != null && audioSrc.isPlaying && audioSrc.clip == keyPressClips)
            {
                 audioSrc.Stop();
            }
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene("TitleScene");
        }
    }

    void CheckCheckpoints()
    {
        if (checkpoint7)
        {
            StartCoroutine(CheckpointSevenLoop());
            checkpoint7 = false;
        }
        else if (checkpoint6)
        {
            StartCoroutine(CheckpointSixLoop());
            checkpoint6 = false;
        }
        else if (checkpoint5)
        {
            StartCoroutine(CheckpointFiveLoop());
            checkpoint5 = false;
        }
        else if (checkpoint4)
        {
            StartCoroutine(CheckpointFourLoop());
            checkpoint4 = false;
        }
        else if (checkpoint3)
        {
            StartCoroutine(CheckpointThreeLoop());
            checkpoint3 = false;
        }
        else if (checkpoint2)
        {
            StartCoroutine(CheckpointTwoLoop());
            checkpoint2 = false;
        }
        else if (checkpoint1)
        {
            StartCoroutine(CheckpointOneLoop());
            checkpoint1 = false;
        }
    }

    IEnumerator MainTutorialLoop()
    {
        mainText.text = "";
        yield return new WaitForSecondsRealtime(1.5f);
        yield return new WaitForSecondsRealtime(TypeTextOut("Ah, what a lovely day to be out on the ocean. ", typingDelay, mainText));
        yield return new WaitForSecondsRealtime(0.7f);
        yield return new WaitForSecondsRealtime(TypeTextOut("The weather is so peaceful! ", typingDelay, mainText, "Ah, what a lovely day to be out on the ocean. "));
        yield return new WaitForSecondsRealtime(0.7f);
        yield return new WaitForSecondsRealtime(TypeTextOut("Time to kick back and relax.", typingDelay, mainText, "Ah, what a lovely day to be out on the ocean. The weather is so peaceful! "));
        yield return new WaitForSecondsRealtime(2f);
        LightRain.Play();
        RainSFX.Play();
        yield return new WaitForSecondsRealtime(2f);
        yield return new WaitForSecondsRealtime(TypeTextOut("Hey look! ", typingDelay, mainText));
        yield return new WaitForSecondsRealtime(0.7f);
        yield return new WaitForSecondsRealtime(TypeTextOut("It's starting to rain a little bit. ", typingDelay, mainText, "Hey look! "));
        yield return new WaitForSecondsRealtime(0.7f);
        yield return new WaitForSecondsRealtime(TypeTextOut("I hope I don't get too wet...", typingDelay, mainText, "Hey look! It's starting to rain a little bit. "));
        yield return new WaitForSecondsRealtime(2.5f);
        yield return new WaitForSecondsRealtime(TypeTextOut("Come to think of it, the forecast ", typingDelay, mainText));
        yield return new WaitForSecondsRealtime(typingDelay);
        mainText.text += "<i>h";
        yield return new WaitForSecondsRealtime(typingDelay);
        mainText.text += "a";
        yield return new WaitForSecondsRealtime(typingDelay);
        mainText.text += "d</i> ";
        yield return new WaitForSecondsRealtime(typingDelay);
        yield return new WaitForSecondsRealtime(TypeTextOut("predicted there to be a storm today.", typingDelay, mainText, "Come to think of it, the forecast <i>had</i> "));
        yield return new WaitForSecondsRealtime(3f);
        ThunderSFX.Play();
        StartCoroutine(PlayThunderRoutine());
        yield return new WaitForSecondsRealtime(0.5f);
        yield return new WaitForSecondsRealtime(TypeTextOut("Thunder!", typingDelay, mainText));
        yield return new WaitForSecondsRealtime(3f);
        yield return new WaitForSecondsRealtime(TypeTextOut("I should get back to shore before the storm gets worse.", typingDelay, mainText));
        yield return new WaitForSecondsRealtime(2f);
        yield return new WaitForSecondsRealtime(TypeTextOut("Drag and drop a blue cyclone onto the marker. ", typingDelay, mainText));
        firstCharge = Instantiate(GreyNegativeCharge, new Vector3(10, 0, 5), Quaternion.identity);
        yield return new WaitForSecondsRealtime(0.7f);
        yield return new WaitForSecondsRealtime(TypeTextOut("It will pull my raft in its direction. ", typingDelay, mainText, "Drag and drop a blue cyclone onto the marker. "));
        yield return new WaitForSecondsRealtime(0.7f);
        yield return new WaitForSecondsRealtime(TypeTextOut("Pull the raft into the goal on the right side of the screen.", typingDelay, mainText, "Drag and drop a blue cyclone onto the marker. It will pull my raft in its direction. "));
        NegativeChargeBox.SetActive(true);
        // the player drags a blue charge. once they release, the text says "Press start when you're ready"

    }
    public IEnumerator CheckpointOneLoop()
    {
        yield return new WaitForSecondsRealtime(TypeTextOut("Press the start button to test out your configuration.", typingDelay, mainText));
        StartButton.gameObject.SetActive(true);
        // NegativeChargeBox.SetActive(false);

    }
    IEnumerator CheckpointTwoLoop()
    {
        AudioSource.PlayClipAtPoint(WinSFX, Vector2.zero);
        Time.timeScale = 0;
        isInGame = false;

        StartButton.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(TypeTextOut("Very nice! Thank you!", typingDelay, mainText));
        yield return new WaitForSecondsRealtime(1.2f);
        yield return new WaitForSecondsRealtime(FadeOut());
        foreach (GameObject charge in GameObject.FindGameObjectsWithTag("Charge"))
        {
            Destroy(charge);
        }
        testChargeStartPos = new Vector2(-12, 0);
        TestCharge.transform.position = testChargeStartPos;
        Trail.Clear();
        yield return new WaitForSecondsRealtime(FadeIn());
        mainText.text = "";
        Instantiate(GreyPositiveCharge, new Vector3(-26, 0, 5), Quaternion.identity);
        yield return new WaitForSecondsRealtime(TypeTextOut("The red cyclones push the raft instead of pulling it. ", typingDelay, mainText));
        yield return new WaitForSecondsRealtime(0.7f);
        yield return new WaitForSecondsRealtime(TypeTextOut("Try it out.", typingDelay, mainText, "The red cyclones push the raft instead of pulling it. "));

        PositiveChargeBox.SetActive(true);
    }
    IEnumerator CheckpointThreeLoop()
    {
        AudioSource.PlayClipAtPoint(WinSFX, Vector2.zero);
        Time.timeScale = 0;
        isInGame = false;
        StartButton.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(TypeTextOut("Perfect.", typingDelay, mainText));
        yield return new WaitForSecondsRealtime(1.2f);
        yield return new WaitForSecondsRealtime(FadeOut());
        foreach (GameObject charge in GameObject.FindGameObjectsWithTag("Charge"))
        {
            Destroy(charge);
        }
        testChargeStartPos = new Vector2(-18, 0);
        TestCharge.transform.position = testChargeStartPos;
        Trail.Clear();
        Instantiate(Obstacles1, Vector2.zero, Quaternion.identity);
        yield return new WaitForSecondsRealtime(FadeIn());
        mainText.text = "";
        yield return new WaitForSecondsRealtime(TypeTextOut("That's a wood plank. ", typingDelay, mainText));
        yield return new WaitForSecondsRealtime(0.7f);     
        yield return new WaitForSecondsRealtime(TypeTextOut("Avoid it at all costs. ", typingDelay, mainText, "That's a wood plank. "));
        yield return new WaitForSecondsRealtime(0.7f);     
        yield return new WaitForSecondsRealtime(TypeTextOut("Navigate me around it and into the goal. ", typingDelay, mainText, "That's a wood plank. Avoid it at all costs. "));
        yield return new WaitForSecondsRealtime(0.7f);     
        yield return new WaitForSecondsRealtime(TypeTextOut("Please don't crash my raft!", typingDelay, mainText, "That's a wood plank. Avoid it at all costs. Navigate me around it and into the goal. "));
        PositiveChargeBox.SetActive(true);
        PositiveChargeBox.SetActive(true);
        NegativeChargeBox.SetActive(true);
        yield return new WaitForSecondsRealtime(4f);
        yield return new WaitForSecondsRealtime(TypeTextOut("Press the start button when you're ready. ", typingDelay, mainText));
        yield return new WaitForSecondsRealtime(0.7f);     
        yield return new WaitForSecondsRealtime(TypeTextOut("If you mess up, press reset to try again.", typingDelay, mainText, "Press the start button when you're ready. "));

        StartButton.gameObject.SetActive(true);
        ResetButton.gameObject.SetActive(true);
        ClearButton.gameObject.SetActive(true);
    }
    IEnumerator CheckpointFourLoop()
    {
        AudioSource.PlayClipAtPoint(WinSFX, Vector2.zero);
        Time.timeScale = 0;
        isInGame = false;
        StartButton.gameObject.SetActive(false);
        ResetButton.gameObject.SetActive(false);
        ClearButton.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(TypeTextOut("Yes! Thanks.", typingDelay, mainText));
        yield return new WaitForSecondsRealtime(1.2f);
        yield return new WaitForSecondsRealtime(FadeOut());
        // clear charges
        foreach (GameObject charge in GameObject.FindGameObjectsWithTag("Charge"))
        {
            Destroy(charge);
        }
        // clear obstacles
        foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            Destroy(obstacle);
        }
        testChargeStartPos = new Vector2(-18, 0);
        TestCharge.transform.position = testChargeStartPos;
        Trail.Clear();
        Instantiate(Obstacles2, Vector2.zero, Quaternion.identity);
        yield return new WaitForSecondsRealtime(FadeIn());
        mainText.text = "";
        yield return new WaitForSecondsRealtime(TypeTextOut("That's a line of buoys. ", typingDelay, mainText));
        yield return new WaitForSecondsRealtime(0.7f);
        yield return new WaitForSecondsRealtime(TypeTextOut("When the raft hits them, it will bounce off of them!", typingDelay, mainText, "That's a line of buoys. "));
        yield return new WaitForSecondsRealtime(0.7f);
        yield return new WaitForSecondsRealtime(TypeTextOut("Don't be afraid to utilize the bounciness of the buoys to manipulate the raft into the goal.", typingDelay, mainText));
        PositiveChargeBox.SetActive(true);
        NegativeChargeBox.SetActive(true);
        StartButton.gameObject.SetActive(true);
        ResetButton.gameObject.SetActive(true);
        ClearButton.gameObject.SetActive(true);
    }
    IEnumerator CheckpointFiveLoop()
    {
        AudioSource.PlayClipAtPoint(WinSFX, Vector2.zero);
        Time.timeScale = 0;
        isInGame = false;
        StartButton.gameObject.SetActive(false);
        ResetButton.gameObject.SetActive(false);
        ClearButton.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(TypeTextOut("Finally!", typingDelay, mainText));
        yield return new WaitForSecondsRealtime(1.2f);
        yield return new WaitForSecondsRealtime(FadeOut());
        // clear charges
        foreach (GameObject charge in GameObject.FindGameObjectsWithTag("Charge"))
        {
            Destroy(charge);
        }
        // clear obstacles
        foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            Destroy(obstacle);
        }
        testChargeStartPos = new Vector2(-18, 0);
        TestCharge.transform.position = testChargeStartPos;
        Trail.Clear();
        Instantiate(Obstacles3, Vector2.zero, Quaternion.identity);
        yield return new WaitForSecondsRealtime(FadeIn());
        mainText.text = "";
        yield return new WaitForSecondsRealtime(TypeTextOut("That looks like a muffler block. ", typingDelay, mainText));
        yield return new WaitForSecondsRealtime(0.7f);
        yield return new WaitForSecondsRealtime(TypeTextOut("The cyclone's forces can't travel through it. ", typingDelay, mainText, "That looks like a muffler block. "));
        PositiveChargeBox.SetActive(true);
        NegativeChargeBox.SetActive(true);
        StartButton.gameObject.SetActive(true);
        ResetButton.gameObject.SetActive(true);
        ClearButton.gameObject.SetActive(true);
    }
    IEnumerator CheckpointSixLoop()
    {
        AudioSource.PlayClipAtPoint(WinSFX, Vector2.zero);
        Time.timeScale = 0;
        isInGame = false;
        StartButton.gameObject.SetActive(false);
        ResetButton.gameObject.SetActive(false);
        ClearButton.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(TypeTextOut("Good job!", typingDelay, mainText));
        yield return new WaitForSecondsRealtime(1.2f);
        yield return new WaitForSecondsRealtime(FadeOut());
        // clear charges
        foreach (GameObject charge in GameObject.FindGameObjectsWithTag("Charge"))
        {
            Destroy(charge);
        }
        // clear obstacles
        foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            Destroy(obstacle);
        }
        testChargeStartPos = new Vector2(-18, 0);
        TestCharge.transform.position = testChargeStartPos;
        Trail.Clear();
        Instantiate(Obstacles4, Vector2.zero, Quaternion.identity);
        yield return new WaitForSecondsRealtime(FadeIn());
        mainText.text = "";
        yield return new WaitForSecondsRealtime(TypeTextOut("Those are stone blocks over there. ", typingDelay, mainText));
        yield return new WaitForSecondsRealtime(0.7f);
        yield return new WaitForSecondsRealtime(TypeTextOut("They act just like muffler blocks, except they are solid and break after a few hits.", typingDelay, mainText, "Those are stone blocks over there. "));
        PositiveChargeBox.SetActive(true);
        NegativeChargeBox.SetActive(true);
        StartButton.gameObject.SetActive(true);
        ResetButton.gameObject.SetActive(true);
        ClearButton.gameObject.SetActive(true);
    }
    IEnumerator CheckpointSevenLoop()
    {
        AudioSource.PlayClipAtPoint(WinSFX, Vector2.zero);
        Time.timeScale = 0;
        isInGame = false;
        StartButton.gameObject.SetActive(false);
        ResetButton.gameObject.SetActive(false);
        ClearButton.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(TypeTextOut("Free at last!", typingDelay, mainText));
        yield return new WaitForSecondsRealtime(1.2f);
        yield return new WaitForSecondsRealtime(TypeTextOut("Thank you for helping me evade those obstacles. ", typingDelay, mainText));
        yield return new WaitForSecondsRealtime(0.7f);
        yield return new WaitForSecondsRealtime(TypeTextOut("Let's get to work. ", typingDelay, mainText, "Thank you for helping me evade those obstacles. "));
        yield return new WaitForSecondsRealtime(0.7f);
        yield return new WaitForSecondsRealtime(TypeTextOut("There's still much more to do before I get home.", typingDelay, mainText, "Thank you for helping me evade those obstacles. Let's get to work. "));
        yield return new WaitForSecondsRealtime(0.7f);
        yield return new WaitForSecondsRealtime(FadeOut());
        SceneManager.LoadScene("TitleScene");

    }
    public void ResetLevel()
    {
        // to reset the stone blocks
        if (currentCheckpoint == 6)
        {
            // clear obstacles
            foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
            {
                Destroy(obstacle);
            }
            Instantiate(Obstacles4, Vector2.zero, Quaternion.identity);
        }
        TestCharge.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        TestCharge.transform.position = testChargeStartPos;
        TestCharge.GetComponent<SpriteRenderer>().sprite = NormalSprite;
        Trail.Clear();
        Time.timeScale = 0;
        isInGame = false;

    }
    public void ClearScreen()
    {
        foreach (GameObject charge in GameObject.FindGameObjectsWithTag("Charge"))
        {
            Destroy(charge);
        }
        ResetLevel();
    }
    public void Collision()
    {
        Time.timeScale = 0;
        isInGame = false;
        TestCharge.GetComponent<SpriteRenderer>().sprite = DamagedSprite;
        string[] deathMessages = { "Ouch!", "Oof.", "Not quite.", "That's going to leave a mark!", "Almost!", "Stop hitting the obstacles!", "My shirt!", "That hurts!", "Help me!", "Try again...", "My raft!" };
        if (!hasCollided)
        {
            TypeTextOut("Ouch! Looks like I crashed. Press reset to try again.", typingDelay, mainText);
            hasCollided = true;
        }
        else
        {
            TypeTextOut(deathMessages[Random.Range(0, deathMessages.Length)], typingDelay, mainText);
        }
    }
    public void StartButtonPressed()
    {
        // wont start when theres no charges on screen
        if (GameObject.FindGameObjectsWithTag("Charge").Length == 0) return;
        ResetLevel();
        Time.timeScale = 1.8f;
        isInGame = true;
    }
    float FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
        return 1;
    }
    float FadeIn()
    {
        StartCoroutine(FadeInCoroutine());
        return 1f;
    }
    IEnumerator FadeOutCoroutine()
    {
        Color currentColor = BlackBackground.color;
        float targetAlpha = 0;
        for (int i = 0; i < 50; i++)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            targetAlpha += 0.02f;
            currentColor.a = targetAlpha;

            BlackBackground.color = currentColor;
        }

    }
    IEnumerator FadeInCoroutine()
    {
        Color currentColor = BlackBackground.color;
        float targetAlpha = 1;

        for (int i = 0; i < 50; i++)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            targetAlpha -= 0.02f;
            currentColor.a = targetAlpha;

            BlackBackground.color = currentColor;
        }
    }
    IEnumerator PlayThunderRoutine()
    {
        while (true)
        {
            float delay = Random.Range(5, 16);
            yield return new WaitForSecondsRealtime(delay);
            ThunderSFX.Play();
        }
    }
    float TypeTextOut(string textToType, float delay, TMP_Text userText, string startingText = "")
    {
        userText.text = startingText;
        StartCoroutine(typeAnimation(textToType, delay, userText, startingText)); 
        return textToType.Length * delay; 
    }

    IEnumerator typeAnimation(string textToType, float delay, TMP_Text userText, string startingText) 
    {
        isTyping = true;
        skipTyping = false; 

        userText.text = startingText;

        int everyOther = 0;
        for (int i = 0; i < textToType.Length; i++)
        {
            if (skipTyping)
            {
                userText.text = startingText + textToType; 
                yield return null;
                break; 
            }

            userText.text += textToType[i];

            if (everyOther % 2 == 0)
            {
                audioSrc.pitch = Random.Range(pitchRange.x, pitchRange.y);
                audioSrc.PlayOneShot(keyPressClips);
            }
            everyOther++;

            yield return new WaitForSecondsRealtime(delay);
        }

        isTyping = false;
        skipTyping = false; 
    }

    public void UpdateCheckpoints(int checkpointNum)
    {
        switch (checkpointNum)
        {
            case 1:
                checkpoint1 = true;
                break;
            case 2:
                checkpoint2 = true;
                break;
            case 3:
                checkpoint3 = true;
                break;
            case 4:
                checkpoint4 = true;
                break;
            case 5:
                checkpoint5 = true;
                break;
            case 6:
                checkpoint6 = true;
                break;
            case 7:
                checkpoint7 = true;
                break;
            default:
                Debug.Log($"{checkpointNum} is not a valid checlpoint");
                return;
        }
        currentCheckpoint = checkpointNum;
    }
}