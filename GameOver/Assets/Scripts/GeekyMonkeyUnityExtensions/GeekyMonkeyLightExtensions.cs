using System;
using UnityEngine;

public static class GeekyMonkeyLightExtensions
{
    /// <summary>
    /// Fade the light intensity over time
    /// </summary>
    /// <param name="m">Material</param>
    /// <param name="mb">MonoBehaviour used for events</param>
    /// <param name="fromVolume">From Volume (0-1)</param>
    /// <param name="toVolume">To Volume (0-1)</param>
    /// <param name="seconds">Seconds</param>
    public static GmDelayPromise FadeIntensity(this Light light, MonoBehaviour mb, float fromIntensity, float toIntensity, float seconds)
    {
        float intervalSeconds = 0.1f;
        float startTime = Time.time;
        int fadeSteps = (int)Math.Ceiling(seconds / intervalSeconds);
        //Debug.Log("Fade Steps = " + fadeSteps);

        light.intensity = fromIntensity;
        return mb.Repeat(intervalSeconds, fadeSteps, () =>
        {
            float timePercent = Mathf.Clamp((Time.time - startTime) / seconds, 0, 1);
            //Debug.Log("Fade % = " + timePercent);
            light.intensity = Mathf.Lerp(fromIntensity, toIntensity, timePercent);
        });
    }
}
