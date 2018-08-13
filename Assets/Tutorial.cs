using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public TutorialCanvas tutorialCanvas;
    public BoxSpawner spawner;

    PlayerController controller;
    GameManager gm;

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        spawner = FindObjectOfType<BoxSpawner>();
        controller = FindObjectOfType<PlayerController>();
        PlayTutorial();
    }

    private bool skipped;

    private void Update()
    {
        if (!skipped && Input.GetKeyDown(KeyCode.Escape))
        {
            SkipTutorial();
            skipped = true;
        }
    }

    void SkipTutorial()
    {
        StopAllCoroutines();
        EndTutorial();
    }

    void EndTutorial()
    {
        tutorialCanvas.CleanState();

        /*
         StartCoroutine(Coroutines.Chain(
             tutorialCanvas.DisplayText("Congratulations! You've passed the trial period with great results.", 3.0f),
             tutorialCanvas.DisplayText("And because of that, we are going to heat the things up a little bit.", 3.0f),
             tutorialCanvas.DisplayText("Oh, and by the way - I don't like messes. So keep this place in order or you are fired.", 3.0f),
             Coroutines.Wrap(() =>
             {
             }
             )));
        */

        gm.StartGame();
        tutorialCanvas.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    void PlayTutorial()
    {

        WaveInfo waveInfo = new WaveInfo
        {
            minNumberOfBoxes = 1,
            maxNumberOfBoxes = 2,
            minNumberOfDecals = 1,
            maxNumberOfDecals = 2,
            spawnAfterSeconds = 0.0f,
            canSpawnGarbage = false,
            canSpawnCrossedLabel = false
        };

        StartCoroutine(Coroutines.Chain(
            Coroutines.Wait(2.0f),
            tutorialCanvas.DisplayText("Hello!", 3.0f),
            Coroutines.Wait(1.0f),
            tutorialCanvas.DisplayText("Welcome to \"Rooster Express, Inc.\" We hope that you enjoy this job as much as we enjoy having you here!", 5.5f),
            Coroutines.Wait(1.0f),
            tutorialCanvas.DisplayText("Your job here will consist in picking up packages from that place over there and putting them in these conveyor belts...", 5.5f),
            Coroutines.Wait(1.0f),
            tutorialCanvas.DisplayText("But you have to make sure they go in the right conveyor, or some clients won't receive their packages in time.", 5.5f),
            Coroutines.Wait(1.0f),
            Coroutines.Join(
                tutorialCanvas.DisplayText("Look, let's try it...", 2.0f),
                Coroutines.Wrap(() => spawner.SpawnBoxes(waveInfo))),
                Coroutines.Wait(2.5f),
                Coroutines.Wrap(() => CreateTutorialBoxSpawnAndDeliver(waveInfo))
        ));
    }

    private void CreateTutorialBoxSpawnAndDeliver(WaveInfo waveInfo)
    {
        StartCoroutine(Coroutines.Chain(
            _CreateTutorialBoxSpawnAndDeliver(waveInfo),
            Coroutines.Wrap(() =>
            {
                if (!gm.playerCompletedFirstTaskSuccessful)
                {
                    gm.playerCompletedFirstTask = false;
                    StartCoroutine(Coroutines.Chain(
                            Coroutines.Join(
                                Coroutines.Wait(2.0f),
                                tutorialCanvas.DisplayText("... Ok, let's try again.", 2.0f),
                                Coroutines.Wrap(() => spawner.SpawnBoxes(waveInfo))),
                            Coroutines.Wait(3.0f),
                            Coroutines.Wrap(() => CreateTutorialBoxSpawnAndDeliver(waveInfo))
                        ));
                    return;
                }
                else
                {
                    StartCoroutine(Coroutines.Chain(
                        tutorialCanvas.DisplayText("Good job!", 2.0f),
                        Coroutines.Wrap(() => CreateTutorialDifferentDisplayGarbage())
                        ));
                }
            })));
    }

    private void CreateTutorialDifferentDisplayGarbage()
    {
        gm.playerCompletedFirstTask = true;

        WaveInfo waveInfo = new WaveInfo
        {
            minNumberOfBoxes = 1,
            maxNumberOfBoxes = 2,
            minNumberOfDecals = 1,
            maxNumberOfDecals = 2,
            spawnAfterSeconds = 0.0f,
            canSpawnGarbage = false,
            canSpawnCrossedLabel = false,
            districtCode = "B"
        };

        WaveInfo garbage = new WaveInfo
        {
            minNumberOfBoxes = 1,
            maxNumberOfBoxes = 2,
            minNumberOfDecals = 1,
            maxNumberOfDecals = 2,
            spawnAfterSeconds = 0.0f,
            canSpawnGarbage = true,
            canSpawnCrossedLabel = false,
            districtCode = "E"
        };

        StartCoroutine(Coroutines.Chain(
            Coroutines.Wrap(() => spawner.SpawnBoxes(waveInfo)),
            _CreateTutorialDifferentDisplayGarbage(),
            Coroutines.Wrap(() =>
            {
                if (!gm.playerCompletedSecondTaskSuccessful)
                {
                    gm.playerCompletedSecondtask = false;
                    StartCoroutine(Coroutines.Chain(
                         Coroutines.Wait(2.0f),
                         tutorialCanvas.DisplayText("... Ok, let's try again.", 2.0f),
                         Coroutines.Wait(1.0f),
                         Coroutines.Wrap(() => CreateTutorialDifferentDisplayGarbage())
                        ));
                    return;
                }
                else
                {
                    StartCoroutine(Coroutines.Chain(
                        Coroutines.Join(
                                Coroutines.Wrap(() => spawner.SpawnBoxes(garbage)),
                                tutorialCanvas.DisplayText("Good job!", 3.5f)),

                        tutorialCanvas.DisplayText("Now...", 3.0f),

                        tutorialCanvas.DisplayText("... How did that got here? Please, get rid of any goods you find that are not properly labeled.", 5.0f),
                        
                        Coroutines.WaitFor(x => gm.playerCompletedThirdTask),

                        Coroutines.IfCondition(
                            x => !gm.playerCompletedThirdTaskSuccessful,
                            tutorialCanvas.DisplayText("Oh god...", 2.0f)),

                        tutorialCanvas.DisplayText("Congratulations! You've passed the trial period with great results.", 4.0f),
                        tutorialCanvas.DisplayText("And because of that, we are going to heat the things up a little bit.", 4.0f),
                        tutorialCanvas.DisplayText("Oh, and by the way - I don't like messes. So keep this place in order or you are fired.", 5.0f),
                        Coroutines.Wrap(() => EndTutorial())));
                }
            })));
    }

    private IEnumerator _CreateTutorialDifferentDisplayGarbage()
    {
        return Coroutines.Chain(
            Coroutines.IfCondition(
                x => !gm.tutorialDifferentDisplaysExplained,

                Coroutines.Chain(
                    tutorialCanvas.DisplayText("Sometimes we also deliver based on priorities. Look at the screens above the conveyors...", 5.0f),
                    Coroutines.Wrap(() => gm.SwapDisplays()),
                    Coroutines.Wait(5.0f),
                    Coroutines.Wrap(() => gm.tutorialDifferentDisplaysExplained = true))),

            tutorialCanvas.DisplayCommand("Drop it in the correct conveyor [LeftMouseClick or the \"A\" button]."),
            Coroutines.WaitFor(x => gm.playerCompletedSecondtask),
            tutorialCanvas.HideCommand());
    }

    private IEnumerator _CreateTutorialBoxSpawnAndDeliver(WaveInfo waveInfo)
    {
        return Coroutines.Chain(

            Coroutines.IfCondition(

                x => !gm.playerGrabbedFirstBox,

                Coroutines.Chain(
                    tutorialCanvas.DisplayCommand("Pick up that box to continue [LeftMouseClick or the \"A\" button]."),
                    Coroutines.WaitFor(x => gm.playerGrabbedFirstBox),
                    Coroutines.Join(
                        tutorialCanvas.HideCommand(),
                        tutorialCanvas.DisplayText("Good job!", 3.0f)))),

            Coroutines.IfCondition(

                x => !gm.playerInspectedFirstBox,

                Coroutines.Chain(
                    tutorialCanvas.DisplayText("As you know, out city is distributed in districts. Each one of these conveyors...", 4.0f),
                    tutorialCanvas.DisplayText("... delivers to a specific district. The district info can be found in the package labels.", 5.0f),
                    tutorialCanvas.DisplayText("Now please, pay attention to the box.", 2.0f),
                    tutorialCanvas.DisplayCommand(
                        "Inspect the box to continue [RightMouseClick or the \"B\" button]."),

                    Coroutines.WaitFor(x => gm.playerInspectedFirstBox || gm.playerCompletedFirstTask),
                    Coroutines.IfCondition(
                        x => !gm.playerCompletedFirstTask,

                        Coroutines.Join(
                            Coroutines.Chain(
                                tutorialCanvas.HideCommand(),
                                tutorialCanvas.DisplayCommand("Zoom in / out [MouseScrollWheel or the Left/right triggers].")),

                            Coroutines.Chain(
                                tutorialCanvas.DisplayText("Good job!", 2.0f),
                                tutorialCanvas.DisplayText("But be careful, because sometimes the packages are redirected and have to go through another district.", 5.0f)))))),

            Coroutines.IfCondition(

                x => !gm.playerCompletedFirstTask,

                Coroutines.Chain(
                    Coroutines.Wait(0.5f),
                    tutorialCanvas.DisplayCommand("Drop it in the correct conveyor"),
                    Coroutines.WaitFor(x => gm.playerCompletedFirstTask),
                    tutorialCanvas.HideCommand())));
    }
}
