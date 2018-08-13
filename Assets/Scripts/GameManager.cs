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

    [Range(0.0f, 1.0f)]
    public float anger;

    public float angerReductionDestroyGarbage;
    public float angerReductionDeliverPackage;
    public float angerIncreaseWrongDelivery;
    public float angerIncreaseBurntPackage;
    public float angerIncreasePerBox;
    public float angerIncreaseFragileBoxHit;
    public float angerDecay;

    public float angryBossThreshold;
    public float uncomfortableBossThreshold;

    bool gameStarted = false;

    void Start()
    {
        playerPoints = 0;
        canvas = FindObjectOfType<Canvas>();
        bossScreen = FindObjectOfType<BossScreen>();
        _playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
     if (!gameStarted) { return; }
        anger -= angerDecay * Time.deltaTime;
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
            anger -= angerReductionDestroyGarbage;
        }
        else
        {
            anger -= angerReductionDeliverPackage;
        }

        bossScreen.Relaxed();
    }

    public void OnBoxSpawned()
    {
        anger += angerIncreasePerBox;
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
            anger += angerIncreaseBurntPackage;
        }
        else
        {
            anger += angerIncreaseWrongDelivery;
        }

        if (anger > angryBossThreshold)
        {
            bossScreen.Angry();
        }
        else
        {
            bossScreen.What();
        }
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
