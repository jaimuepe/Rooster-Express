
using UnityEngine;

/// <summary>
/// Audio-fading parameters.
/// </summary>
public class FadingAudioParams
{
    public bool fadeOut;
    public bool fadeIn;

    public float fadeOutTime = -1.0f;
    public float fadeInTime = -1.0f;

    public AudioClip newClip;
}