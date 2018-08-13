using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject playGameObject;
    public AudioSource bgMusic;
    public Image fadePanel;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(playGameObject);
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(playGameObject);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
    
    public void StartGame()
    {
        StartCoroutine(
            Coroutines.Chain(
                Coroutines.Join(
                    Coroutines.FadeVolume(bgMusic, 1.0f, 0.0f),
                    Coroutines.FadeAlpha01(fadePanel, 1.0f)),
                Coroutines.Wrap(() => SceneManager.LoadScene("MapScene"))));
    }
}
