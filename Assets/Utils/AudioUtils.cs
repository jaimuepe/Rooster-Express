using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioUtils : MonoBehaviour
{
    public static void PlayClip2D(AudioClip clip, float volume)
    {
        GameObject g = new GameObject("[OneShotAudio]");
        AudioSource source = g.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.Play();
        source.reverbZoneMix = 0.0f;
        Destroy(g, clip.length);
    }
}
