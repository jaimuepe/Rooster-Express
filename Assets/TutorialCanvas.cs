using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCanvas : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    public TextMeshProUGUI commandtext;

    // public Image tutorialPanel;
    public Image commandPanel;

    public float textFadeTime;
    public GameObject skipTutorialHintText;

    BossScreen screen;

    private void Start()
    {
        screen = FindObjectOfType<BossScreen>();
    }

    public void CleanState()
    {
        screen.Relaxed();
        commandtext.color = new Color(commandtext.color.r, commandtext.color.g, commandtext.color.b, 0.0f);
        tutorialText.color = new Color(tutorialText.color.r, tutorialText.color.g, tutorialText.color.b, 0.0f);
        skipTutorialHintText.gameObject.SetActive(false);
    }

    public IEnumerator DisplayCommand(string text)
    {
        return Coroutines.Chain(
         Coroutines.Join(
             Coroutines.Wrap(() => commandtext.text = text),
             Coroutines.FadeAlpha01(commandtext, textFadeTime)
             ));
    }

    public IEnumerator HideCommand()
    {
        return Coroutines.Join(
            Coroutines.FadeAlpha10(commandtext, textFadeTime)
            );
    }

    public IEnumerator DisplayText(string text, float length)
    {
        return Coroutines.Chain(
            Coroutines.Join(
                Coroutines.Wrap(() => screen.Talk()),
                Coroutines.Wrap(() => tutorialText.text = text),
                Coroutines.FadeAlpha01(tutorialText, textFadeTime)
                ),
            Coroutines.Wait(length),
            Coroutines.Join(
                Coroutines.FadeAlpha10(tutorialText, textFadeTime),
                Coroutines.Wrap(() => screen.Relaxed()))
        );
    }
}
