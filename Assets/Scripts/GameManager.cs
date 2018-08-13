using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    private Transform _playerTransform;
    Canvas canvas;

    public Text pointsUI;
    public TextMeshProUGUI coinText;

    public int correctBoxes;
    public int wrongBoxes;
    public int burntBoxes;
    public int fragileBoxesHits;
    public float maximumThrowDistance;
    public int itemsThrown;
    public int itemsThrownCorrect;
    public int garbageCollected;

    public Canvas statsCanvas;
    public TextMeshProUGUI correctBoxesText;
    public TextMeshProUGUI wrongBoxesText;
    public TextMeshProUGUI burntBoxesText;
    public TextMeshProUGUI fragileBoxesHitsText;
    public TextMeshProUGUI maximumThrowDistanceText;
    public TextMeshProUGUI itemsThrownText;
    public TextMeshProUGUI itemsThrownCorrectText;
    public TextMeshProUGUI garbageCollectedText;
    public TextMeshProUGUI totalMoneyText;

    private float totalMoney;

    public float moneyCorrect;
    public float moneyWrong;
    public float moneyBurnt;
    public float moneyFragileHits;
    public float moneyThrowDistance;
    public float moneyThrownCorrectly;
    public float moneyGarbage;

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

    public EndGameObject endGame;

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

    public ParticleSystem playerSweatParticleSystem;

    void Start()
    {
        totalMoney = 0;
        fadePanel.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);

        StartCoroutine(
            Coroutines.FadeAlpha10(fadePanel, 1.0f));

        canvas = FindObjectOfType<Canvas>();
        bossScreen = FindObjectOfType<BossScreen>();
        _playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (!gameStarted || endGame.gameEnded) { return; }

        if (Anger > uncomfortableBossThreshold)
        {
            playerSweatParticleSystem.Play(true);
        }
        else
        {
            playerSweatParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        if (boxesInScreen * angerIncreasePerBox + _anger >= 100.0f)
        {
            playerWon = false;
            endGame.EndGame();
        }
    }

    public bool playerWon = false;

    public void UpdatePoints() {

        correctBoxesText.text = "Correct boxes: " + correctBoxes + " (<color=\"green\">+$" + moneyCorrect * correctBoxes + "</color>)";
        wrongBoxesText.text = "Wrong boxes: " + wrongBoxes + " (<color=\"red\">-$" + moneyWrong * wrongBoxes + "</color>)";
        burntBoxesText.text = "Burnt boxes: " + burntBoxes + " (<color=\"red\">-$" + moneyBurnt * burntBoxes + "</color>)";
        fragileBoxesHitsText.text = "Fragile items broken: " + fragileBoxesHits + " (<color=\"red\">-$" + moneyFragileHits * fragileBoxesHits + "</color>)";
        maximumThrowDistanceText.text = "Max. throw distance: " + System.Math.Round(maximumThrowDistance, 0) + "m (<color=\"green\">+$" 
            + System.Math.Round(moneyThrowDistance * maximumThrowDistance, 0) + "</color>)";
        itemsThrownText.text = "Items thrown: " + itemsThrown;
        itemsThrownCorrectText.text = "Items thrown correctly: " + itemsThrownCorrect + " (<color=\"green\">+$" 
            + moneyThrownCorrectly * itemsThrownCorrect + "</color>)";
        garbageCollectedText.text = "Garbage collected: " + garbageCollected + " (<color=\"green\">+$" + moneyGarbage * garbageCollected + "</color>)";
        totalMoney = (moneyCorrect * correctBoxes) - (moneyWrong * wrongBoxes) - (moneyBurnt * burntBoxes) 
            - (moneyFragileHits * fragileBoxesHits) + (moneyThrowDistance * maximumThrowDistance) + (moneyThrownCorrectly * itemsThrownCorrect)
            + (garbageCollected * moneyGarbage);
        totalMoneyText.autoSizeTextContainer = true;
        totalMoneyText.text = "YOUR SALARY: $" + System.Math.Round(totalMoney, 0);
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
            if (gameStarted)
            {
                _anger = Mathf.Clamp(_anger - angerReductionDestroyGarbage, 0.0f, 100.0f);
            }
            recollectedGarbage();
        }
        else
        {
            if (gameStarted)
            {
                _anger = Mathf.Clamp(_anger - angerReductionDeliverPackage, 0.0f, 100.0f);
            }
            incrementPoints();
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
            if (gameStarted)
            {
                _anger = Mathf.Clamp(_anger + angerIncreaseBurntPackage, 0.0f, 100.0f);
            }
            burntBoxPoints();
        }
        else
        {
            if (gameStarted)
            {
                _anger = Mathf.Clamp(_anger + angerIncreaseWrongDelivery, 0.0f, 100.0f);
            }
            decrementPoints();
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

    public void incrementPoints()
    {
        Debug.Log("Caja correcta");
        correctBoxes += 1;
    }

    public void decrementPoints()
    {
        Debug.Log("Caja incorrecta");
        wrongBoxes += 1;
    }

    public void burntBoxPoints()
    {
        burntBoxes += 1;
    }

    public void recollectedGarbage()
    {
        garbageCollected += 1;
    }
}