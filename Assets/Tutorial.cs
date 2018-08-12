using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public TutorialCanvas tutorialCanvas;
    public BoxSpawner spawner;

    PlayerController controller;

    void Start()
    {
        spawner = FindObjectOfType<BoxSpawner>();
        controller = FindObjectOfType<PlayerController>();
        PlayTutorial();
    }

    void PlayTutorial()
    {

        WaveInfo waveInfo = new WaveInfo
        {
            minNumberOfBoxes = 1,
            maxNumberOfBoxes = 2,
            minNumberOfDecals = 1,
            maxNumberOfDecals = 2,
            spawnAfterSeconds = 0.0f
        };

        StartCoroutine(Coroutines.Chain(
            Coroutines.Wait(2.0f),
            tutorialCanvas.DisplayText("Hello!", 3.0f),
            Coroutines.Wait(1.0f),
            tutorialCanvas.DisplayText("Welcome to \"Rooster express, Inc.\" We hope that you enjoy this job as much as we enjoy having you here =)", 5.0f),
            Coroutines.Wait(1.0f),
            tutorialCanvas.DisplayText("Your job here will consist in picking up packages from that place over there and putting them in these conveyor belts...", 5.0f),
            Coroutines.Wait(1.0f),
            tutorialCanvas.DisplayText("But you have to make sure they go in the right conveyor, or some clients won't receive their packages =(", 3.0f),
            Coroutines.Wait(1.0f),
            Coroutines.Join(
                tutorialCanvas.DisplayText("Look, let's try it...", 1.0f),
                Coroutines.Wrap(() => spawner.SpawnBoxes(waveInfo))),
            Coroutines.Wait(2.0f),
            tutorialCanvas.DisplayCommand("Pick that box up to continue."),
            Coroutines.WaitFor(x => controller.playerGrabbedFirstBox),
            Coroutines.Join(
                tutorialCanvas.HideCommand(),
                tutorialCanvas.DisplayText("Good job!", 3.0f))
            ));
    }
}
