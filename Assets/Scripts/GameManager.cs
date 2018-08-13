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

    public int correctBoxes;
    public int wrongBoxes;
    public int burntBoxes;
    public int fragileBoxesHits;
    public float maximumThrowDistance;
    public int boxesThrown;
    public int boxesThrownCorrect;

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

    DeliveryLogic[] deliveries;

    BossScreen bossScreen;

    TextFollows textFollows;


    void Start()
    {
        playerPoints = 0;
        canvas = FindObjectOfType<Canvas>();
        bossScreen = FindObjectOfType<BossScreen>();
        deliveries = FindObjectsOfType<DeliveryLogic>();
        textFollows = FindObjectOfType<TextFollows>();
    }

    public void SuccessfulDelivery()
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

        bossScreen.Relaxed();
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
        bossScreen.What();
    }

    public void StartGame()
    {
        playerCompletedFirstTask = true;
        playerCompletedSecondtask = true;
        playerCompletedThirdTask = true;

        for (int i = 0; i < deliveries.Length; i++)
        {
            if (deliveries[i].state != "E")
            {
                deliveries[i].District();
            }
        }

        waveSpawner.gameObject.SetActive(true);
        waveSpawner.BeginWaves();
    }

    public void SwapDisplays()
    {
        for (int i = 0; i < deliveries.Length; i++)
        {
            if (deliveries[i].state != "E")
            {
                deliveries[i].SwapDisplay();
            }
        }
    }

    public void incrementPoints(Caja caja)
    {
        this.playerPoints += 1;
        textFollows.showMessage(textFollows.NORMAL_TEXT, textFollows.COLOR_GREEN);
        //pointsUI.text = "Points: " + playerPoints.ToString();
        Debug.Log("Caja correcta");
        correctBoxes += 1;
    }

    public void decrementPoints(Caja caja)
    {
        this.playerPoints -= 1;
        textFollows.showMessage(textFollows.WRONG_TEXT, textFollows.COLOR_RED);
        //pointsUI.text = "Points: " + playerPoints.ToString();
        Debug.Log("Caja incorrecta");
        wrongBoxes += 1;
    }

    public void burntBoxPoints(Caja caja) {
        this.playerPoints -= 5;
        textFollows.showMessage(textFollows.BURNT_TEXT, textFollows.COLOR_RED);
        burntBoxes += 1;
    }

}
