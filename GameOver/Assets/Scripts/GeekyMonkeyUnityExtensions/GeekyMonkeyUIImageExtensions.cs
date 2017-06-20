using System;
using UnityEngine;
using UnityEngine.UI;

public static class GeekyMonkeyUIImageExtensions
{
    public static void SetAlpha(this Image img, float alpha)
    {
        img.color = img.color.WithAlpha(alpha);
    }

    public static GmDelayPromise FadeAlpha(this Image img, MonoBehaviour mb, float fromAlpha, float toAlpha, float seconds)
    {
        float intervalSeconds = 0.1f;
        float startTime = Time.time;
        int fadeSteps = (int)Math.Ceiling(seconds / intervalSeconds);
        //Debug.Log("Fade Steps = " + fadeSteps);

        img.color = img.color.WithAlpha(fromAlpha);
        return mb.Repeat(intervalSeconds, fadeSteps, () =>
        {
            float timePercent = Mathf.Clamp((Time.time - startTime) / seconds, 0, 1);
            //Debug.Log("Fade % = " + timePercent);
            img.color = img.color.WithAlpha(Mathf.Lerp(fromAlpha, toAlpha, timePercent));
        });
    }

}
