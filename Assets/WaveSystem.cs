using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    public WaveInfo[] waves;
    private BoxSpawner spawner;

    public float totalGameTime;
    public AnimationCurve wavesCurve;
    public AnimationCurve boxesSpawnedInTimeCurve;
    public AnimationCurve randomDecalsInTimeCurve;
    public AnimationCurve displayModeSwapCurve;

    private int lastWave = -1;
    private int lastDisplayModeSwap = 0;

    private float totalTime;

    private bool started;

    GameManager gm;

    private void Start()
    {
        spawner = FindObjectOfType<BoxSpawner>();
        gm = FindObjectOfType<GameManager>();
    }

    public void BeginWaves()
    {
        started = true;
        Debug.Log("Starting wave spawn...");
    }

    private void Update()
    {
        if (!started) { return; }

        int wave = (int)Math.Floor(wavesCurve.Evaluate(totalTime));
        if (wave != lastWave)
        {
            SpawnWave(wave);
            lastWave = wave;
        }

        int display = (int)Mathf.Floor(displayModeSwapCurve.Evaluate(totalTime));
        if (display != lastDisplayModeSwap)
        {
            gm.SwapDisplays();
            lastDisplayModeSwap = display;
        }
        totalTime += Time.deltaTime;
    }

    private void SpawnWave(int wave)
    {
        int boxes = (int)boxesSpawnedInTimeCurve.Evaluate(totalTime);
        int decals = (int)randomDecalsInTimeCurve.Evaluate(totalTime);

        Debug.Log("Spawning wave " + wave + ": boxes = " + boxes + ", decals = " + decals);

        WaveInfo waveInfo = new WaveInfo
        {
            canSpawnCrossedLabel = true,
            canSpawnGarbage = true,

            minNumberOfBoxes = (int)Mathf.Floor(0.9f * boxesSpawnedInTimeCurve.Evaluate(totalTime)),
            maxNumberOfBoxes = (int)Mathf.Ceil(1.1f * boxesSpawnedInTimeCurve.Evaluate(totalTime)),

            minNumberOfDecals = (int)Mathf.Floor(0.9f * randomDecalsInTimeCurve.Evaluate(totalTime)),
            maxNumberOfDecals = (int)Mathf.Floor(1.1f * randomDecalsInTimeCurve.Evaluate(totalTime))
        };

        spawner.SpawnBoxes(waveInfo);
    }
}

[Serializable]
public class WaveInfo
{
    public int minNumberOfBoxes;
    public int maxNumberOfBoxes;

    public int minNumberOfDecals;
    public int maxNumberOfDecals;

    public float spawnAfterSeconds;

    public bool canSpawnGarbage = true;
    public bool canSpawnCrossedLabel = true;

    public string districtCode = null;
}
