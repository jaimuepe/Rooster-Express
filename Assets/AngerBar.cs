using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class AngerBar : MonoBehaviour
{
    public Image[] chickenImages;
    public float concernedChickenValue;
    public float angryChickenValue;

    [Range(0.0f, 1.0f)]
    public float angerValue;

    void Start()
    {

    }

    void Update()
    {

    }
}
