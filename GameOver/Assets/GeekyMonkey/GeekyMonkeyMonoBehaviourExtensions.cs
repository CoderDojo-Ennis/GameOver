using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeekyMonkeyMonoBehaviourExtensions
{
    public static GmDelayPromise Delay(this MonoBehaviour mb, float delaySeconds, Action callback = null, bool realtime = false)
    {
        var promise = new GmDelayPromise { monobehaviour = mb };
        promise.coroutine = mb.StartCoroutine(WaitThenCallback(mb, delaySeconds, 1, callback, promise, realtime));
        return promise;
    }

    public static GmDelayPromise Repeat(this MonoBehaviour mb, float delaySeconds, int times, Action callback = null, bool realtime = false)
    {
        var promise = new GmDelayPromise { monobehaviour = mb };
        promise.coroutine = mb.StartCoroutine(WaitThenCallback(mb, delaySeconds, times, callback, promise, realtime));
        return promise;
    }

    public static GmDelayPromise Forever(this MonoBehaviour mb, float delaySeconds, Action callback = null, bool realtime = false)
    {
        var promise = new GmDelayPromise { monobehaviour = mb };
        promise.coroutine = mb.StartCoroutine(WaitThenCallback(mb, delaySeconds, int.MaxValue, callback, promise, realtime));
        return promise;
    }

    private static IEnumerator WaitThenCallback(MonoBehaviour mb, float seconds, int times, Action callback, GmDelayPromise promise, bool realTime)
    {
        for (var i = 0; i < times || times == int.MaxValue; i++)
        {
            if (realTime)
            {
                yield return new WaitForSecondsRealtime(seconds);
            }
            else
            {
                yield return new WaitForSeconds(seconds);
            }
            if (callback != null && mb != null && mb.gameObject != null && mb.isActiveAndEnabled)
            {
                try
                {
                    callback();
                }
                catch (Exception ex)
                {
                    Debug.LogError("Callback Error: " + ex.Message);
                }
            }
        }
        promise.Done();
    }

    public static GameObject[] GetChildrenWithTag(this MonoBehaviour mb, string tag, bool includeInactive)
    {
        var childTransforms = mb.GetComponentsInChildren<Transform>(includeInactive);
        List<GameObject> taggedChildren = new List<GameObject>();
        for (var i = 0; i < childTransforms.Length; i++)
        {
            if (childTransforms[i].CompareTag(tag))
            {
                taggedChildren.Add(childTransforms[i].gameObject);
            }
        }

        return taggedChildren.ToArray();
    }

    static public GameObject FindChildByName(this MonoBehaviour mb, string childName)
    {
        Transform[] ts = mb.transform.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (Transform t in ts)
        {
            if (t.gameObject.name == childName)
            {
                return t.gameObject;
            }
        }
        return null;
    }
}

public class GmDelayPromise
{
    internal Coroutine coroutine;
    internal MonoBehaviour monobehaviour;
    private Action then;
    bool done;

    // Protected constructor
    internal GmDelayPromise()
    {
    }

    public void Abort()
    {
        this.monobehaviour.StopCoroutine(this.coroutine);
    }

    public void Then(Action thenCallback)
    {
        if (done)
        {
            thenCallback();
        }
        else
        {
            this.then += thenCallback;
        }
    }

    internal void Done()
    {
        done = true;
        if (this.then != null)
        {
            this.then();
        }
    }
}
