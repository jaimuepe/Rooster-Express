using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StatisticCanvas : MonoBehaviour
{
    public GameObject playAgainButton;
    public AudioSource bgMusic;
    public Image fadePanel;

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(playAgainButton);
        }
    }

    public void PlayAgain()
    {
        StartCoroutine(
            Coroutines.Chain(
                Coroutines.Join(
                    Coroutines.FadeVolume(bgMusic, 1.0f, 0.0f),
                    Coroutines.FadeAlpha01(fadePanel, 1.0f)),
                Coroutines.Wrap(() => SceneManager.LoadScene("MapScene"))));
    }

    public void Exit()
    {
        Application.Quit();
    }
}
