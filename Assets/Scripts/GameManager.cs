using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private float playerPoints;
    Canvas canvas;

    public Text pointsUI;

    // TUTORIAL
    [Header("Debug")]
    public bool playerGrabbedFirstBox;
    public bool playerInspectedFirstBox;
    public bool playerCompletedFirstTask;
    public bool playerCompletedFirstTaskSuccessful;

    public bool playerCompletedSecondtask;
    public bool playerCompletedSecondTaskSuccessful;

    public bool tutorialDifferentDisplaysExplained = false;

    public WaveSystem waveSpawner;

    BossScreen bossScreen;

    void Start()
    {
        playerPoints = 0;
        canvas = FindObjectOfType<Canvas>();
        bossScreen = FindObjectOfType<BossScreen>();
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
        }
        bossScreen.Relaxed();
    }

    public void UnsucessfulDelivery(bool destroyedPackage)
    {
        if (!playerCompletedFirstTask)
        {
            playerCompletedFirstTask = true;
            playerCompletedFirstTaskSuccessful = false;
            bossScreen.What();
        }
        else
        {
            if (!playerCompletedSecondtask)
            {
                playerCompletedSecondtask = true;
                playerCompletedSecondTaskSuccessful = false;
                bossScreen.What();
            }
            else
            {
                if (destroyedPackage)
                {
                    bossScreen.Angry();
                }
                else
                {
                    bossScreen.What();
                }
            }
        }
    }

    public void StartGame()
    {
        playerCompletedFirstTask = true;
        playerCompletedSecondtask = true;
        waveSpawner.gameObject.SetActive(true);
        waveSpawner.BeginWaves();
    }

    public void incrementPoints(float points)
    {
        this.playerPoints += 1;
        pointsUI.text = "Points: " + playerPoints.ToString();
        Debug.Log("Caja correcta");
    }

    public void decrementPoints(float points)
    {
        this.playerPoints -= 1;
        pointsUI.text = "Points: " + playerPoints.ToString();
        Debug.Log("Caja incorrecta");
    }

}
