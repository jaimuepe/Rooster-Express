using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    private float playerPoints;
    private Transform _playerTransform;
    Canvas canvas;

    public Text pointsUI;
    public TextMeshProUGUI coinText;

    // TUTORIAL
    [Header("Debug")]
    public bool playerGrabbedFirstBox;
    public bool playerInspectedFirstBox;
    public bool playerCompletedFirstTask;
    public bool playerCompletedFirstTaskSuccessful;


    public bool playerCompletedSecondtask;
    public bool playerCompletedSecondTaskSuccessful;

    public bool playerCompletedThirdTask;
    public bool playerCompletedThirdTaskSuccessful;

    public bool tutorialDifferentDisplaysExplained = false;

    public WaveSystem waveSpawner;

    public DeliveryLogic[] deliveriesWithoutGarbage;

    BossScreen bossScreen;

    public Canvas gameCanvas;

    [Range(0.0f, 100f)]
    [SerializeField]
    private float _anger;

    public float Anger
    {
        get
        {
            return Mathf.Clamp(boxesInScreen * angerIncreasePerBox + _anger, 0.0f, 100f);
        }
    }

    public float angerReductionDestroyGarbage;
    public float angerReductionDeliverPackage;
    public float angerIncreaseWrongDelivery;
    public float angerIncreaseBurntPackage;
    public float angerIncreasePerBox;
    public float angerIncreaseFragileBoxHit;

    public float angryBossThreshold;
    public float uncomfortableBossThreshold;

    public AudioSource bgMusic;

    bool gameStarted = false;

    public Image fadePanel;

    int boxesInScreen = 0;

    void Start()
    {
        fadePanel.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);

        StartCoroutine(
            Coroutines.FadeAlpha10(fadePanel, 1.0f));

        playerPoints = 0;
        canvas = FindObjectOfType<Canvas>();
        bossScreen = FindObjectOfType<BossScreen>();
        _playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (!gameStarted) { return; }

        if (boxesInScreen * angerIncreasePerBox + _anger >= 100.0f)
        {
            Debug.Break();
        }
    }

    public void SuccessfulDelivery(bool destroyedGarbage)
    {
        if (!playerCompletedFirstTask)
        {
            playerCompletedFirstTask = true;
            playerCompletedFirstTaskSuccessful = true;
        }
        else
        {
            if (!playerCompletedSecondtask)
            {
                playerCompletedSecondtask = true;
                playerCompletedSecondTaskSuccessful = true;
            }
            else if (!playerCompletedThirdTask)
            {
                playerCompletedThirdTask = true;
                playerCompletedThirdTaskSuccessful = true;
            }
        }

        if (destroyedGarbage)
        {
            _anger -= angerReductionDestroyGarbage;
        }
        else
        {
            _anger -= angerReductionDeliverPackage;
        }

        bossScreen.Relaxed();

        boxesInScreen = Mathf.Max(0, boxesInScreen - 1);
    }

    public void OnBoxSpawned()
    {
        boxesInScreen++;
    }

    public void UnsucessfulDelivery(bool destroyedPackage)
    {
        if (!playerCompletedFirstTask)
        {
            playerCompletedFirstTask = true;
            playerCompletedFirstTaskSuccessful = false;
        }
        else
        {
            if (!playerCompletedSecondtask)
            {
                playerCompletedSecondtask = true;
                playerCompletedSecondTaskSuccessful = false;
            }
            else if (!playerCompletedThirdTask)
            {
                playerCompletedThirdTask = true;
                playerCompletedThirdTaskSuccessful = false;
            }
        }

        if (destroyedPackage)
        {
            _anger += angerIncreaseBurntPackage;
        }
        else
        {
            _anger += angerIncreaseWrongDelivery;
        }

        if (Anger > angryBossThreshold)
        {
            bossScreen.Angry();
        }
        else
        {
            bossScreen.What();
        }

        boxesInScreen = Mathf.Max(0, boxesInScreen - 1);
    }

    public void StartGame()
    {
        playerCompletedFirstTask = true;
        playerCompletedSecondtask = true;
        playerCompletedThirdTask = true;

        for (int i = 0; i < deliveriesWithoutGarbage.Length; i++)
        {
            deliveriesWithoutGarbage[i].District(i);
        }

        waveSpawner.gameObject.SetActive(true);
        gameCanvas.gameObject.SetActive(true);

        bgMusic.Play();

        waveSpawner.BeginWaves();
        gameStarted = true;
    }

    public void SwapDisplays()
    {
        int[] indexes = { 0, 1, 2, 3 };
        indexes.Shuffle();

        for (int i = 0; i < deliveriesWithoutGarbage.Length; i++)
        {
            deliveriesWithoutGarbage[i].SwapDisplay(indexes[i]);
        }
    }

    public void incrementPoints(float points)
    {
        this.playerPoints += 1;
        showCoinText("+2 coins");
        //pointsUI.text = "Points: " + playerPoints.ToString();
        Debug.Log("Caja correcta");
    }

    public void decrementPoints(float points)
    {
        this.playerPoints -= 1;
        //pointsUI.text = "Points: " + playerPoints.ToString();
        Debug.Log("Caja incorrecta");
    }


    private void showCoinText(string text)
    {
        coinText.text = text;
        coinText.autoSizeTextContainer = true;
    }

}
