using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsManager : MonoBehaviour
{
    public Light directionalLight;
    public Light[] pointLights;

    public Light treadmillLightA;
    public Light treadmillLightB;
    public Light treadmillLightC;
    public Light treadmillLightD;
    public Light treadmillLightShreder;

    Dictionary<int, Color> defaultColors = new Dictionary<int, Color>();

    private void Start()
    {
        defaultColors[directionalLight.GetInstanceID()] = directionalLight.color;

        for (int i = 0; i < pointLights.Length; i++)
        {
            defaultColors[pointLights[i].GetInstanceID()] = pointLights[i].color;
        }

        defaultColors[treadmillLightA.GetInstanceID()] = treadmillLightA.color;
        defaultColors[treadmillLightB.GetInstanceID()] = treadmillLightB.color;
        defaultColors[treadmillLightC.GetInstanceID()] = treadmillLightC.color;
        defaultColors[treadmillLightD.GetInstanceID()] = treadmillLightD.color;
        defaultColors[treadmillLightShreder.GetInstanceID()] = treadmillLightShreder.color;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TurnRed();
        }
    }

    public void TurnRed()
    {
        IEnumerator wait = Coroutines.Wait(1.0f);
        IEnumerator fadeDirectionalLightBack = Coroutines.FadeColor(directionalLight, 0.5f, Color.red, GetDefaultColor(directionalLight));

        List<IEnumerator> fadeToRedCoroutines = new List<IEnumerator>();
        List<IEnumerator> fadeBackFromRedCoroutines = new List<IEnumerator>();

        fadeToRedCoroutines.Add(
            Coroutines.FadeColor(directionalLight, 0.5f, directionalLight.color, Color.red));

        fadeBackFromRedCoroutines.Add(
            Coroutines.FadeColor(directionalLight, 0.5f, Color.red, GetDefaultColor(directionalLight)));

        for (int i = 0; i < pointLights.Length; i++)
        {
            fadeToRedCoroutines.Add(
                Coroutines.FadeColor(pointLights[i], 0.5f, pointLights[i].color, Color.red));

            fadeBackFromRedCoroutines.Add(
            Coroutines.FadeColor(pointLights[i], 0.5f, Color.red, GetDefaultColor(pointLights[i])));
        }

        Renderer r = treadmillLightA.transform.parent.GetComponent<Renderer>();

        fadeToRedCoroutines.Add(
            Coroutines.Join(
                Coroutines.FadeColor(treadmillLightA, 0.5f, treadmillLightA.color, Color.red),
               // Coroutines.FadeColor(r.material, "_Color", 0.5f, Color.white, new Color(255, 89, 89) / 255.0f),
                Coroutines.FadeColor(r.material, "_EmissionColor", 0.5f, Color.black, Color.red)));

        fadeBackFromRedCoroutines.Add(
            Coroutines.Join(
                Coroutines.FadeColor(treadmillLightA, 0.5f, Color.red, GetDefaultColor(treadmillLightA)),
               // Coroutines.FadeColor(r.material, "_Color", 0.5f, new Color(255, 89, 89) / 255.0f, Color.white),
                Coroutines.FadeColor(r.material, "_EmissionColor", 0.5f, Color.red, Color.black)));

        CoroutineSingleton.Instance.StartCoroutine(Coroutines.Chain(
            Coroutines.Join(fadeToRedCoroutines.ToArray()),
            wait,
            Coroutines.Join(fadeBackFromRedCoroutines.ToArray())));
    }

    private Color GetDefaultColor(Light light)
    {
        return defaultColors[light.GetInstanceID()];
    }
}
