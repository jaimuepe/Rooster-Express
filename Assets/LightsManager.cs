using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsManager : MonoBehaviour
{
    public Light directionalLight;
    public Light[] pointLights;

    public Light[] threadmillLights;

    Dictionary<int, Color> defaultColors = new Dictionary<int, Color>();

    public AudioClip buzzerClip;

    private void Start()
    {
        defaultColors[directionalLight.GetInstanceID()] = directionalLight.color;

        for (int i = 0; i < pointLights.Length; i++)
        {
            defaultColors[pointLights[i].GetInstanceID()] = pointLights[i].color;
        }

        for (int i = 0; i < threadmillLights.Length; i ++)
        {
            defaultColors[threadmillLights[i].GetInstanceID()] = threadmillLights[i].color;
        }

        treadmillLightsCoroutine = new Coroutine[5];
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                TurnGreen(0);
            }
            else
            {
                TurnRed(0);
            }
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                TurnGreen(1);
            }
            else
            {
                TurnRed(1);
            }
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                TurnGreen(2);
            }
            else
            {
                TurnRed(2);
            }
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                TurnGreen(3);
            }
            else
            {
                TurnRed(3);
            }
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                TurnGreen(4);
            }
            else
            {
                TurnRed(4);
            }
        }
    }
#endif

    Coroutine globalLightsCoroutine;
    Coroutine[] treadmillLightsCoroutine;

    public void TurnGreen(int light)
    {
        Light threadmillLight = threadmillLights[light];

        Renderer r = threadmillLight.transform.parent.GetComponent<Renderer>();

        if (treadmillLightsCoroutine[light] != null)
        {
            StopCoroutine(treadmillLightsCoroutine[light]);
        }

        treadmillLightsCoroutine[light] = StartCoroutine(Coroutines.Chain(
            Coroutines.Join(
                Coroutines.FadeColor(threadmillLight, 0.5f, threadmillLight.color, Color.green),
                // Coroutines.FadeColor(r.material, "_Color", 0.5f, Color.white, new Color(255, 89, 89) / 255.0f),
                Coroutines.FadeColor(r.material, "_EmissionColor", 0.5f, r.material.GetColor("_EmissionColor"), Color.green)),
            Coroutines.Wait(1.0f),
            Coroutines.Join(
                Coroutines.FadeColor(threadmillLight, 0.5f, Color.green, GetDefaultColor(threadmillLight)),
                // Coroutines.FadeColor(r.material, "_Color", 0.5f, new Color(255, 89, 89) / 255.0f, Color.white),
                Coroutines.FadeColor(r.material, "_EmissionColor", 0.5f, Color.green, Color.black))));
    }

    public void TurnRed(int light)
    {
        // GLOBAL LIGHTS

        if (globalLightsCoroutine != null)
        {
            StopCoroutine(globalLightsCoroutine);
        }

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

        globalLightsCoroutine = CoroutineSingleton.Instance.StartCoroutine(Coroutines.Chain(
            Coroutines.Join(fadeToRedCoroutines.ToArray()),
            Coroutines.Wait(1.0f),
            Coroutines.Join(fadeBackFromRedCoroutines.ToArray())));

        // TREADMILL LIGHT

        Light threadmillLight = threadmillLights[light];

        if (treadmillLightsCoroutine[light] != null)
        {
            StopCoroutine(treadmillLightsCoroutine[light]);
        }

        Renderer r = threadmillLight.transform.parent.GetComponent<Renderer>();

        treadmillLightsCoroutine[light] = StartCoroutine(Coroutines.Chain(
            Coroutines.Join(
                Coroutines.FadeColor(threadmillLight, 0.5f, threadmillLight.color, Color.red),
                // Coroutines.FadeColor(r.material, "_Color", 0.5f, Color.white, new Color(255, 89, 89) / 255.0f),
                Coroutines.FadeColor(r.material, "_EmissionColor", 0.5f, Color.black, Color.red)),
            Coroutines.Wait(1.0f),
            Coroutines.Join(
                Coroutines.FadeColor(threadmillLight, 0.5f, Color.red, GetDefaultColor(threadmillLight)),
                // Coroutines.FadeColor(r.material, "_Color", 0.5f, new Color(255, 89, 89) / 255.0f, Color.white),
                Coroutines.FadeColor(r.material, "_EmissionColor", 0.5f, Color.red, Color.black))));

        AudioUtils.PlayClip2D(buzzerClip, 1.0f);
    }

    private Color GetDefaultColor(Light light)
    {
        return defaultColors[light.GetInstanceID()];
    }
}
