using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A canvas+panel used for fading effects.
/// </summary>
public class FadeCanvas : MonoBehaviour
{
    private Canvas canvas;
    public Image FadePanel { get; private set; }

    private void Awake()
    {
        canvas = gameObject.AddComponent<Canvas>();

        // This is important so the canvas stays on top of everything.
        canvas.sortingOrder = 16000;
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler cs = gameObject.AddComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(1920, 1080);
        cs.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        cs.matchWidthOrHeight = 1.0f;

        GraphicRaycaster gr = gameObject.AddComponent<GraphicRaycaster>();
        gr.ignoreReversedGraphics = false;
        gr.blockingObjects = GraphicRaycaster.BlockingObjects.None;

        GameObject panelObj = new GameObject("FadePanel");
        FadePanel = panelObj.AddComponent<Image>();

        RectTransform rt = panelObj.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.0f, 0.0f);
        rt.anchorMax = new Vector2(1.0f, 1.0f);

        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, -1.0f, cs.referenceResolution.y + 2);
        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, -1.0f, cs.referenceResolution.x + 2);
        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 1.0f, cs.referenceResolution.y + 2);

        FadePanel.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        panelObj.transform.SetParent(transform, false);
    }
}