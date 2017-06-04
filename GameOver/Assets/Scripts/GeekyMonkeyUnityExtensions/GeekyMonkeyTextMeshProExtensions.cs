using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class GeekyMonkeyTextMeshProExtensions
{
    public static GmDelayPromise FadeAlpha(this TextMeshPro tmp, float fromAlpha, float toAlpha, float seconds)
    {
        float intervalSeconds = 0.1f;
        float startTime = Time.time;
        int fadeSteps = (int)Math.Ceiling(seconds / intervalSeconds);
        //Debug.Log("Fade Steps = " + fadeSteps);

        tmp.SetAlpha(fromAlpha);
        return tmp.Repeat(intervalSeconds, fadeSteps, () =>
        {
            float timePercent = Mathf.Clamp((Time.time - startTime) / seconds, 0, 1);
            //Debug.Log("Fade % = " + timePercent);
            tmp.SetAlpha(Mathf.Lerp(fromAlpha, toAlpha, timePercent));
        });
    }

    public static void SetAlpha(this TextMeshPro tmp, float alpha)
    {
        tmp.faceColor = tmp.faceColor.WithAlpha(alpha);
        tmp.outlineColor = tmp.outlineColor.WithAlpha(alpha);
    }
}
