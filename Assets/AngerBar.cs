using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class AngerBar : MonoBehaviour
{
    public Image chicken;
    public Sprite[] chickenSprites;

    [SerializeField]
    float maxHeight;
    RectTransform rectTransform;
    RectTransform chickenRectTransform;

    GameManager gm;

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        rectTransform = GetComponent<RectTransform>();
        chickenRectTransform = chicken.GetComponent<RectTransform>();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (chickenRectTransform == null) { chickenRectTransform = chicken.GetComponent<RectTransform>(); }
        if (gm == null) { gm = FindObjectOfType<GameManager>(); }
#endif
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, gm.anger * maxHeight);
        if (gm.anger < gm.uncomfortableBossThreshold)
        {
            chicken.sprite = chickenSprites[0];
        }
        else if (gm.anger < gm.angryBossThreshold)
        {
            chicken.sprite = chickenSprites[1];
        }
        else
        {
            chicken.sprite = chickenSprites[2];
        }

        chickenRectTransform.anchoredPosition = new Vector2(
            chickenRectTransform.anchoredPosition.x,
            rectTransform.anchoredPosition.y + gm.anger * maxHeight);
    }
}
