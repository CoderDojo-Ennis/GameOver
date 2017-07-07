using System.Collections.Generic;
using UnityEngine;

public static class GeekyMonkeyGameobjectExtensions
{
    public static void DeleteAllChildren(this GameObject go)
    {
        foreach (Transform t in go.transform)
        {
            GameObject.Destroy(t.gameObject);
        }
    }

    public static T[] GetComponentsInChildrenWithTag<T>(this GameObject gameObject, string tag)
        where T : Component
    {
        var results = new List<T>();

        if (gameObject.CompareTag(tag))
        {
            results.Add(gameObject.GetComponent<T>());
        }

        foreach (Transform t in gameObject.transform)
        {
            results.AddRange(t.gameObject.GetComponentsInChildrenWithTag<T>(tag));
        }

        return results.ToArray();
    }

    public static T GetComponentInParents<T>(this GameObject gameObject)
        where T : Component
    {
        for (Transform t = gameObject.transform; t != null; t = t.parent)
        {
            T result = t.GetComponent<T>();
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    public static T[] GetComponentsInParents<T>(this GameObject gameObject)
        where T : Component
    {
        var results = new List<T>();
        for (Transform t = gameObject.transform; t != null; t = t.parent)
        {
            T result = t.GetComponent<T>();
            if (result != null)
            {
                results.Add(result);
            }
        }

        return results.ToArray();
    }

    /// <summary>
    /// the set of layers that GameObject can collide against.
    /// </summary>
    /// <param name="gameObject">The game object</param>
    /// <param name="layer">If omitted, it uses the layer of the calling GameObject, which is the most common/intuitive case. But you can specify a layer and it’ll hand you the collision mask for that layer instead.</param>
    /// <returns></returns>
    public static int GetCollisionMask(this GameObject gameObject, int layer = -1)
    {
        if (layer == -1)
        {
            layer = gameObject.layer;
        }

        int mask = 0;
        for (int i = 0; i < 32; i++)
        {
            mask |= (Physics.GetIgnoreLayerCollision(layer, i) ? 0 : 1) << i;
        }

        return mask;
    }

    public static GmDelayPromise GlidePosition(this Transform transform, MonoBehaviour mb, Vector3 startPos, Vector3 targetPos, float seconds, bool realtime)
    {
        int steps = 40;
        float step = 0;

        transform.position = startPos;

        return mb.Repeat(seconds / steps, steps, () =>
        {
            step++;
            transform.position = Vector3.Lerp(startPos, targetPos, step / steps);
        }, realtime);
    }

    public static GmDelayPromise GlideLocalPosition(this Transform transform, MonoBehaviour mb, Vector3 startPos, Vector3 targetPos, float seconds, bool realtime)
    {
        int steps = 40;
        float step = 0;

        transform.localPosition = startPos;

        return mb.Repeat(seconds / steps, steps, () =>
        {
            step++;
            transform.localPosition = Vector3.Lerp(startPos, targetPos, step / steps);
        }, realtime);
    }

    /*
    /// <summary>
    /// Sets the game object active/inactive and returns a promise that is satisfied on the next frame
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="active"></param>
    /// <returns>Promise</returns>
    public static GmDelayPromise SetActiveAsync(this GameObject gameObject, MonoBehaviour mb, bool active)
    {
        gameObject.SetActive(active);
        var promise = new GmDelayPromise();
        mb.Delay(0.1f, () => { promise.Done(); });
        return promise;
    }
    */
}
