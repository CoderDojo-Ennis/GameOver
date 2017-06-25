using System;
using TMPro;
using UnityEngine;

public static class GeekyMonkeyTextMeshProExtensions
{
    public static GmDelayPromise FadeAlpha(this TextMeshPro tmp, float fromAlpha, float toAlpha, float seconds, bool realtime)
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
        return tmp.Repeat(intervalSeconds, fadeSteps, () =>
        {
            step++;
            float timePercent = Mathf.Clamp(step / fadeSteps, 0, 1);
            //Debug.Log("Fade % = " + timePercent);
            tmp.SetAlpha(Mathf.Lerp(fromAlpha, toAlpha, timePercent));
        }, true);
    }

    public static void SetAlpha(this TextMeshPro tmp, float alpha)
    {
        tmp.color = tmp.color.WithAlpha(alpha);
        tmp.faceColor = tmp.faceColor.WithAlpha(alpha);
        tmp.outlineColor = tmp.outlineColor.WithAlpha(alpha);
    }

    /// <summary>
    /// Type out the characters one at a time
    /// </summary>
    /// <param name="tmp"></param>
    /// <param name="mb"></param>
    /// <param name="characterSeconds"></param>
    /// <param name="characterShown"></param>
    /// <returns></returns>
    public static GmDelayPromise Type(this TextMeshPro tmp, MonoBehaviour mb, float characterSeconds, bool realtime = false, Action characterShown = null)
    {
        if (tmp == null || mb == null)
        {
            return null;
        }

        GmDelayPromise finishPromise;

        TMP_TextInfo textInfo = tmp.textInfo;

        // Get # of Visible Character in text object
        tmp.maxVisibleCharacters = 999999;
        int charCount = tmp.text.Length; //  textInfo.characterCount; 
        Debug.Log("Typing " + charCount + " characters:" + tmp.text);

        tmp.maxVisibleCharacters = 0;

        finishPromise = mb.Repeat(characterSeconds, charCount, () =>
        {
            //Debug.Log("Typing character");
            tmp.maxVisibleCharacters += 1;
            if (characterShown != null)
            {
                characterShown();
            }
        }, realtime);

        return finishPromise;
    }

    /// <summary>
    /// Type out the characters one at a time
    /// </summary>
    /// <param name="tmp"></param>
    /// <param name="mb"></param>
    /// <param name="characterSeconds"></param>
    /// <param name="characterShown"></param>
    /// <returns></returns>
    public static GmDelayPromise Type(this TextMeshProUGUI tmp, MonoBehaviour mb, float characterSeconds, bool realtime = false, Action characterShown = null)
    {
        if (tmp == null || mb == null)
        {
            return null;
        }

        GmDelayPromise finishPromise;

        TMP_TextInfo textInfo = tmp.textInfo;

        // Get # of Visible Character in text object
        tmp.maxVisibleCharacters = 999999;
        int charCount = tmp.text.Length; //  textInfo.characterCount; 
        Debug.Log("Typing " + charCount + " characters:" + tmp.text);

        tmp.maxVisibleCharacters = 0;

        finishPromise = mb.Repeat(characterSeconds, charCount, () =>
        {
            //Debug.Log("Typing character");
            tmp.maxVisibleCharacters += 1;
            if (characterShown != null)
            {
                characterShown();
            }
        }, realtime);

        return finishPromise;
    }
}
