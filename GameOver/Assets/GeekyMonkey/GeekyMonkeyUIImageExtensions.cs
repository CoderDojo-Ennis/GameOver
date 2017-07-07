using System;
using UnityEngine;
using UnityEngine.UI;

public static class GeekyMonkeyUIImageExtensions
{
    public static void SetAlpha(this Image img, float alpha)
    {
        img.color = img.color.WithAlpha(alpha);
    }

    public static GmDelayPromise FadeAlpha(this Image img, MonoBehaviour mb, float fromAlpha, float toAlpha, float seconds, bool realtime)
    {
        if (seconds == 0)
        {
            img.SetAlpha(toAlpha);
            var done = new GmDelayPromise();
            done.Done();
            return done;
        }

        float intervalSeconds = 0.1f;
        float step = 0;
        int fadeSteps = (int)Math.Ceiling(seconds / intervalSeconds);
        //Debug.Log("Fade Steps = " + fadeSteps);

        img.color = img.color.WithAlpha(fromAlpha);
        return mb.Repeat(intervalSeconds, fadeSteps, () =>
        {
            step++;
            float timePercent = Mathf.Clamp(step / fadeSteps, 0, 1);
            //Debug.Log("Fade % = " + timePercent);
            img.SetAlpha(Mathf.Lerp(fromAlpha, toAlpha, timePercent));
        });
    }

}
