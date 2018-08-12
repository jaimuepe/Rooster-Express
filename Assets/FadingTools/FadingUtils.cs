
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadingUtils
{
    public static void FadeScreen(FadingScreenParams screenParams, FadingAudioParams audioParams = null)
    {
        if (audioParams == null) { audioParams = new FadingAudioParams(); }

        Image fadePanel = FadeCanvasSingleton.Instance.FadePanel;

        List<IEnumerator> actions = new List<IEnumerator>();

        // --- ENABLE FADE PANEL ---

        actions.Add(Coroutines.Wrap(() => fadePanel.gameObject.SetActive(true)));

        // --- FADING OUT ---

        float screenFadeOutTime = screenParams.fadeOutTime;

        IEnumerator fadeOutScreen = Coroutines.FadeAlpha01(fadePanel, screenFadeOutTime);

        if (audioParams.fadeOut)
        {
            float audioFadeOutTime = audioParams.fadeOutTime;

            if (screenFadeOutTime < 0.0f)
            {
                Debug.LogWarning("Negative screenFadeOutTime. Will default to 1.0f");
                screenFadeOutTime = 1.0f;
            }

            if (audioFadeOutTime < 0.0f)
            {
                Debug.Log("Negative audioFadeOutTime. Will default to screenFadeOutTime (" + screenFadeOutTime + ")");
                audioFadeOutTime = screenFadeOutTime;
            }

            /*
            IEnumerator fadeOutBgMusic = Coroutines.FadeVolume(
                AudioManagerSingleton.Instance.BackgroundMusicSource,
                audioFadeOutTime,
                0.0f);

            IEnumerator stopBgMusic = Coroutines.Wrap(() => AudioManagerSingleton.Instance.BackgroundMusicSource.Stop());

            // When joining coroutines the longest one should be the last one
            if (audioFadeOutTime > screenFadeOutTime)
            {
                actions.Add(Coroutines.Join(fadeOutScreen, Coroutines.Chain(fadeOutBgMusic, stopBgMusic)));
            }
            else
            {
                actions.Add(Coroutines.Join(Coroutines.Chain(fadeOutBgMusic, stopBgMusic), fadeOutScreen));
            }
            */
        }
        else
        {
            actions.Add(fadeOutScreen);
        }

        // --- BEFORE WAIT TIME CALLBACK ---

        if (screenParams.beforeWaitCallback != null)
        {
            actions.Add(Coroutines.Wrap(screenParams.beforeWaitCallback));
        }

        // --- WAIT TIME ---

        if (screenParams.waitTime != 0.0f)
        {
            actions.Add(Coroutines.Wait(screenParams.waitTime));
        }

        // --- AFTER WAIT TIME CALLBACK ---

        if (screenParams.afterWaitCallback != null)
        {
            actions.Add(Coroutines.Wrap(screenParams.afterWaitCallback));
        }

        // --- SWAP CLIP BEFORE FADING IN, IF NEEDED ---

        if (audioParams.newClip != null)
        {
           //  actions.Add(Coroutines.Wrap(() => AudioManagerSingleton.Instance.BackgroundMusicSource.clip = audioParams.newClip));
        }

        // --- FADE IN ---

        float screenFadeInTime = screenParams.fadeInTime;

        IEnumerator fadeInScreen = Coroutines.FadeAlpha10(fadePanel, screenFadeInTime);

        if (audioParams.fadeIn)
        {
            float audioFadeInTime = audioParams.fadeInTime;
            if (screenFadeInTime < 0.0f)
            {
                Debug.LogWarning("Negative screenFadeInTime. Will default to 1.0f");
                screenFadeInTime = 1.0f;
            }

            if (audioFadeInTime < 0.0f)
            {
                Debug.Log("Negative audioFadeInTime. Will default to screenFadeInTime (" + screenFadeInTime + ")");
                audioFadeInTime = screenFadeOutTime;
            }

            
            /*
            /IEnumerator fadeInBgMusic = Coroutines.FadeVolume(
                AudioManagerSingleton.Instance.BackgroundMusicSource,
                audioFadeInTime,
                AudioManagerSingleton.Instance.BackgroundMusicMaxVolume);

            // When joining coroutines the longest one should be the last one
            if (audioFadeInTime > screenFadeInTime)
            {
                actions.Add(Coroutines.Join(fadeInScreen, fadeInBgMusic));
            }
            else
            {
                actions.Add(Coroutines.Join(fadeInBgMusic, fadeInScreen));
            }
            */
        }
        else
        {
            actions.Add(fadeInScreen);
        }

        // --- END CALLBACK

        if (screenParams.endCallback != null)
        {
            actions.Add(Coroutines.Wrap(screenParams.endCallback));
        }

        // --- DISABLE FADE PANEL ---

        actions.Add(Coroutines.Wrap(() => fadePanel.gameObject.SetActive(false)));

        // Chain and run

        Coroutines.Chain(actions.ToArray()).Run();
    }
}
