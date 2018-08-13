using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameObject : MonoBehaviour
{

    public PlayerController controller;
    public GrabItems grabber;
    public WaveSystem waveSystem;
    public BoxSpawner spawner;

    public GameManager gm;

    public Canvas endGameCanvas;
    public BossScreen bossScreen;

    public TextMeshProUGUI bossText;

    public bool gameEnded = false;

    public Canvas statisticsCanvas;

    public void EndGame()
    {
        gameObject.SetActive(true);

        gameEnded = true;
        StopAllCoroutines();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        gm.UpdatePoints();
        controller.ExitDetailView();
        grabber.DropBox(0.0f, controller.transform.position);
        controller.enabled = false;
        grabber.enabled = false;
        waveSystem.gameObject.SetActive(false);
        bossScreen.Relaxed();

        if (gm.playerWon)
        {
            StartCoroutine(CoroutineEndOk());
        }
        else
        {
            StartCoroutine(CoroutineEndFail());
        }
    }

    IEnumerator CoroutineEndOk()
    {
        return Coroutines.Chain(
               Coroutines.Join(
                    Coroutines.Wrap(() => spawner.SpawnBoxes(500, 0.01f)),
                    DisplayText("You think you are good at your job? Let's see how you handle THIS...", 4.0f)),
               Coroutines.Wait(2.0f),
               DisplayText("... Oh, wait, your turn is over. Don't worry, someone else will pick this up. See you tomorrow...", 6.0f),
               Coroutines.Wait(1.0f),
               Coroutines.Wrap(() => statisticsCanvas.gameObject.SetActive(true)));
    }

    IEnumerator CoroutineEndFail()
    {
        return Coroutines.Chain(
            Coroutines.Wait(2.0f),
            DisplayText("Well... that was a disaster. You are fired.", 4.0f),
            Coroutines.Wait(1.0f),
            Coroutines.Wrap(() => statisticsCanvas.gameObject.SetActive(true)));
    }

    IEnumerator DisplayText(string text, float length)
    {
        return Coroutines.Chain(
            Coroutines.Join(
                Coroutines.Wrap(() => bossScreen.Talk()),
                Coroutines.Wrap(() => bossText.text = text),
                Coroutines.FadeAlpha01(bossText, 0.5f)
                ),
            Coroutines.Wait(length),
            Coroutines.Join(
                Coroutines.FadeAlpha10(bossText, 0.5f),
                Coroutines.Wrap(() => bossScreen.Relaxed()))
        );
    }
}
