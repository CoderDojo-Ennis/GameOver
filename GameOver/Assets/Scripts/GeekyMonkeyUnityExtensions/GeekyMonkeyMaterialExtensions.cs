using System;
using UnityEngine;

public static class GeekyMonkeyMaterialExtensions
{
    /// <summary>
    /// Fade the main color from one color to another over time
    /// </summary>
    /// <param name="m">Material</param>
    /// <param name="mb">MonoBehaviour used for events</param>
    /// <param name="fromColor">From Color</param>
    /// <param name="toColor">To Color</param>
    /// <param name="seconds">Seconds</param>
    public static GmDelayPromise Fade(this Material mat, MonoBehaviour mb, Color fromColor, Color toColor, float seconds)
    {
        float intervalSeconds = 0.1f;
        float startTime = Time.time;
        int fadeSteps = (int)Math.Ceiling(seconds / intervalSeconds);
        //Debug.Log("Fade Steps = " + fadeSteps);

        mat.SetColor("_Color", fromColor);
        return mb.Repeat(intervalSeconds, fadeSteps, () =>
        {
            float timePercent = Mathf.Clamp((Time.time - startTime) / seconds, 0, 1);
            //Debug.Log("Fade % = " + timePercent);
            mat.SetColor("_Color", Color.Lerp(fromColor, toColor, timePercent));
        });
    }
}
