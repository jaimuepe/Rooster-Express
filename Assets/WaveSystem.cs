using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    public WaveInfo[] waves;
    private BoxSpawner spawner;

    private void Start()
    {
        spawner = FindObjectOfType<BoxSpawner>();
#if UNITY_EDITOR
        BeginWaves();
#endif
    }

    public void BeginWaves()
    {
        StartCoroutine(WaveSpawner());
    }

    IEnumerator WaveSpawner()
    {
        for (int i = 0; i < waves.Length; i++)
        {
            yield return new WaitForSeconds(waves[i].spawnAfterSeconds);
            spawner.SpawnBoxes(waves[i]);
        }
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
}
