using System;
using UnityEngine;

public static class GeekyMonkeyCanvasRendererExtensions
{
    public static GmDelayPromise FadeAlpha(this CanvasRenderer tmp, MonoBehaviour mb, float fromAlpha, float toAlpha, float seconds, bool realtime)
    {
        if (seconds == 0)
        {
            tmp.SetAlpha(toAlpha);
            var done = new GmDelayPromise();
            done.Done();
            return done;
        }

        float intervalSeconds = 0.1f;
        float step = 0;
        int fadeSteps = (int)Math.Ceiling(seconds / intervalSeconds);
        //Debug.Log("Fade Steps = " + fadeSteps);

        tmp.SetAlpha(fromAlpha);
        return mb.Repeat(intervalSeconds, fadeSteps, () =>
        {
            step++;
            float timePercent = Mathf.Clamp(step / fadeSteps, 0, 1);
            //Debug.Log("Fade % = " + timePercent);
            tmp.SetAlpha(Mathf.Lerp(fromAlpha, toAlpha, timePercent));
        });
    }

}
