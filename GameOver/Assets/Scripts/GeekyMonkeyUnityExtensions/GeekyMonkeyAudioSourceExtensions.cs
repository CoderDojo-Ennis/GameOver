﻿using System;
using UnityEngine;

public static class GeekyMonkeyAudioSourceExtensions
{
    /// <summary>
    /// Fade the audio source volume over time
    /// </summary>
    /// <param name="m">Material</param>
    /// <param name="mb">MonoBehaviour used for events</param>
    /// <param name="fromVolume">From Volume (0-1)</param>
    /// <param name="toVolume">To Volume (0-1)</param>
    /// <param name="seconds">Seconds</param>
    public static GmDelayPromise Fade(this AudioSource source, MonoBehaviour mb, float fromVolume, float toVolume, float seconds)
    {
        float intervalSeconds = 0.1f;
        float startTime = Time.time;
        int fadeSteps = (int)Math.Ceiling(seconds / intervalSeconds);
        //Debug.Log("Fade Steps = " + fadeSteps);

        source.volume = fromVolume;
        return mb.Repeat(intervalSeconds, fadeSteps, () =>
        {
            float timePercent = Mathf.Clamp((Time.time - startTime) / seconds, 0, 1);
            //Debug.Log("Fade % = " + timePercent);
            source.volume = Mathf.Lerp(fromVolume, toVolume, timePercent);
        });
    }
}
