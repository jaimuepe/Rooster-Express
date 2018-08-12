using System;

/// <summary>
/// Screen-fading parameters.
/// </summary>
public class FadingScreenParams
{
    public float fadeOutTime;
    public float fadeInTime;
    public float waitTime;

    public Action beforeWaitCallback;
    public Action afterWaitCallback;
    public Action endCallback;

}
