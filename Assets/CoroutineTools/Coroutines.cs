using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class with a bunch of helpful coroutines.
/// </summary>
public class Coroutines
{
    /// <summary>
    /// Returns an action wrapped in a coroutine.
    /// </summary>
    /// <param name="action">Action to be wrapped.</param>
    /// <returns>The resulting coroutine.</returns>
    public static IEnumerator Wrap(Action action)
    {
        action.Invoke();
        yield return null;
    }

    /// <summary>
    /// Returns a coroutine that waits the specified number of seconds.
    /// </summary>
    /// <param name="seconds">Seconds to wait.</param>
    /// <returns>The resulting coroutine.</returns>
    public static IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    /// <summary>
    /// Returns a coroutine with the specified actions chained, in sequential order.
    /// </summary>
    /// <param name="actions"></param>
    /// <returns>The resulting coroutine.</returns>
    public static IEnumerator Chain(params IEnumerator[] actions)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            yield return actions[i].Run();
        }
    }

    /// <summary>
    /// Returns a coroutine that executes an action, then waits and then executes a second action.
    /// </summary>
    /// <param name="first">First action to be executed.</param>
    /// <param name="delay">Delay between the first and the second action.</param>
    /// <param name="then">Second action to be executed.</param>
    /// <returns>The resulting coroutine.</returns>
    public static IEnumerator FirstAndThen(Action first, float delay, Action then)
    {
        return Chain(Wrap(first), Wait(delay), Wrap(then));
    }

    /// <summary>
    /// Returns a coroutine composed by others coroutines. They will be executed in parallel.
    /// The length of the composed coroutine will be equal to the length of the last coroutine.
    /// </summary>
    /// <param name="actions">Array of coroutines to group.</param>
    /// <returns>The resulting coroutine.</returns>
    public static IEnumerator Join(params IEnumerator[] actions)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            if (i == actions.Length - 1)
            {
                yield return actions[i].Run();
            }
            else
            {
                actions[i].Run();
            }
        }
    }

    /// <summary>
    /// Returns a coroutine that fades the alpha of an image in a given time from 0 to 1.
    /// </summary>
    /// <param name="image">image to be faded.</param>
    /// <param name="duration">Fade duration.</param>
    /// <returns>The resulting coroutine.</returns>
    public static IEnumerator FadeAlpha01(Image image, float duration)
    {
        Color c = image.color;
        return FadeColor(image, duration, new Color(c.r, c.g, c.b, 0.0f), new Color(c.r, c.g, c.b, 1.0f));
    }

    /// <summary>
    /// Returns a coroutine that fades the alpha of an image in a given time from 0 to 1.
    /// </summary>
    /// <param name="image">image to be faded.</param>
    /// <param name="duration">Fade duration.</param>
    /// <returns>The resulting coroutine.</returns>
    public static IEnumerator FadeAlpha01(Text text, float duration)
    {
        Color c = text.color;
        return FadeColor(text, duration, new Color(c.r, c.g, c.b, 0.0f), new Color(c.r, c.g, c.b, 1.0f));
    }

    /// <summary>
    /// Returns a coroutine that fades the alpha of an image in a given time from 1 to 0.
    /// </summary>
    /// <param name="image">image to be faded.</param>
    /// <param name="duration">Fade duration.</param>
    /// <returns>The resulting coroutine.</returns>
    public static IEnumerator FadeAlpha10(Image image, float duration)
    {
        Color c = image.color;
        return FadeColor(image, duration, new Color(c.r, c.g, c.b, 1.0f), new Color(c.r, c.g, c.b, 0.0f));
    }

    public static IEnumerator FadeAlpha10(Text text, float duration)
    {
        Color c = text.color;
        return FadeColor(text, duration, new Color(c.r, c.g, c.b, 1.0f), new Color(c.r, c.g, c.b, 0.0f));
    }

    /// <summary>
    /// Returns a coroutine that fades the color of an image in a given time.
    /// </summary>
    /// <param name="image">image to be faded.</param>
    /// <param name="duration">Fade duration.</param>
    /// <param name="endColor">End color.</param>
    /// <returns>The resulting coroutine.</returns>
    public static IEnumerator FadeColor(Image image, float duration, Color endColor)
    {
        return FadeColor(image, duration, image.color, endColor);
    }

    /// <summary>
    /// Returns a coroutine that fades the color of an image in a given time.
    /// </summary>
    /// <param name="image">image to be faded.</param>
    /// <param name="duration">Fade duration.</param>
    /// <param name="startColor">Start color.</param>
    /// <param name="endColor">End color.</param>
    /// <returns>The resulting coroutine.</returns>
    public static IEnumerator FadeColor(Image image, float duration, Color startColor, Color endColor)
    {
        if (duration > 0.0f)
        {

            image.color = startColor;
            float t = 0.0f;

            while (t <= duration)
            {
                image.color = Color.Lerp(startColor, endColor, t / duration);

                t += Time.deltaTime;
                yield return null;
            }
        }

        image.color = endColor;
    }

    public static IEnumerator FadeColor(Material mat, string varName, float duration, Color startColor, Color endColor)
    {
        Color c = startColor;
        if (duration > 0.0f)
        {
            mat.SetColor(varName, c);

            float t = 0.0f;

            while (t <= duration)
            {
                c = Color.Lerp(startColor, endColor, t / duration);
                mat.SetColor(varName, c);

                t += Time.deltaTime;
                yield return null;
            }
        }
        c = endColor;
        mat.SetColor(varName, c);
    }

    public static IEnumerator FadeColor(Text text, float duration, Color startColor, Color endColor)
    {
        if (duration > 0.0f)
        {

            text.color = startColor;
            float t = 0.0f;

            while (t <= duration)
            {
                text.color = Color.Lerp(startColor, endColor, t / duration);

                t += Time.deltaTime;
                yield return null;
            }
        }

        text.color = endColor;
    }

    public static IEnumerator FadeColor(Light light, float duration, Color startColor, Color endColor)
    {
        if (duration > 0.0f)
        {

            light.color = startColor;
            float t = 0.0f;

            while (t <= duration)
            {
                light.color = Color.Lerp(startColor, endColor, t / duration);

                t += Time.deltaTime;
                yield return null;
            }
        }
        light.color = endColor;
    }

    /// <summary>
    /// Returns a coroutine that fades the volume of an audiosource in a given time.
    /// </summary>
    /// <param name="source">AudioSource to be faded.</param>
    /// <param name="duration">Fade duration.</param>
    /// <param name="endVolume">End volume.</param>
    /// <returns>The resulting coroutine.</returns>
    public static IEnumerator FadeVolume(AudioSource source, float duration, float endVolume)
    {
        return FadeVolume(source, duration, -1.0f, endVolume);
    }

    public static IEnumerator WaitFor(Predicate<object> predicate)
    {
        while (!predicate.Invoke(null))
        {
            yield return null;
        }
    }

    /// <summary>
    /// Returns a coroutine that fades the volume of an audiosource in a given time.
    /// </summary>
    /// <param name="source">AudioSource to be faded.</param>
    /// <param name="duration">Fade duration.</param>
    /// <param name=startVolume">Start volume.</param>
    /// <param name="endVolume">End volume.</param>
    /// <returns>The resulting coroutine.</returns>
    public static IEnumerator FadeVolume(AudioSource source, float duration, float startVolume, float endVolume)
    {
        if (startVolume < 0.0f)
        {
            startVolume = source.volume;
        }

        if (endVolume > 0.0f && !source.isPlaying)
        {
            source.Play();
        }

        if (duration > 0.0f)
        {
            source.volume = startVolume;

            float t = 0.0f;

            while (t <= duration)
            {
                source.volume = Mathf.Lerp(startVolume, endVolume, t / duration);

                t += Time.deltaTime;
                yield return null;
            }
        }

        source.volume = endVolume;
    }
}