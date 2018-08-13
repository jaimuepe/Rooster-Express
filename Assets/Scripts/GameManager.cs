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

    DeliveryLogic[] deliveries;

    BossScreen bossScreen;


    void Start()
    {
        playerPoints = 0;
        canvas = FindObjectOfType<Canvas>();
        bossScreen = FindObjectOfType<BossScreen>();
        deliveries = FindObjectsOfType<DeliveryLogic>();
        _playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        coinText.transform.localEulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);
        coinText.transform.position = new Vector3(_playerTransform.position.x, _playerTransform.position.y + 1.3f, _playerTransform.position.z);
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


    private void showCoinText(string text) {
        coinText.text = text;
        coinText.autoSizeTextContainer = true;
    }

}
