using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class AngerBar : MonoBehaviour
{
    public Image chicken;
    public Sprite[] chickenSprites;
    public float concernedChickenValue;
    public float angryChickenValue;

    [Range(0.0f, 1.0f)]
    public float angerValue;

    [SerializeField]
    float maxHeight;
    RectTransform rectTransform;
    RectTransform chickenRectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        chickenRectTransform = chicken.GetComponent<RectTransform>();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (chickenRectTransform == null) { chickenRectTransform = chicken.GetComponent<RectTransform>(); }
#endif
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, angerValue * maxHeight);
        if (angerValue < concernedChickenValue)
        {
            chicken.sprite = chickenSprites[0];
        }
        else if (angerValue < angryChickenValue)
        {
            chicken.sprite = chickenSprites[1];
        }
        else
        {
            chicken.sprite = chickenSprites[2];
        }

        chickenRectTransform.anchoredPosition = new Vector2(
            chickenRectTransform.anchoredPosition.x,
            rectTransform.anchoredPosition.y + angerValue * maxHeight);
    }
}
