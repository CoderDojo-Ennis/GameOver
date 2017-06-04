using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGameScene : MonoBehaviour {

    [Header("Transition")]
    public float FadeSeconds = 2;

    public Light[] SceneLights;
    private float[] LightIntensities;

    [HideInInspector]
    public Camera SceneCamera;

	// Use this for initialization
	public void Start () {
        LightIntensities = new float[SceneLights.Length];
        for (var i = 0; i < SceneLights.Length; i++)
        {
            LightIntensities[i] = SceneLights[i].intensity;
        }

        SceneCamera = this.gameObject.GetComponentInChildren<Camera>();
        SceneCamera.enabled = false;
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	public void Update () {
		
	}

    /// <summary>
    /// Fade in the lights in this scene
    /// </summary>
    /// <returns></returns>
    public GmDelayPromise FadeIn()
    {
        SceneCamera.enabled = true;
        GmDelayPromise promise = null;

        for (var i = 0; i < SceneLights.Length; i++)
        {
            promise = SceneLights[i].FadeIntensity(this, 0, LightIntensities[i], FadeSeconds);
        }

        return promise;
    }

    /// <summary>
    /// Fade out the lights in this scene
    /// </summary>
    /// <returns></returns>
    public GmDelayPromise FadeOut()
    {
        SceneCamera.enabled = true;
        GmDelayPromise promise = null;

        for (var i = 0; i < SceneLights.Length; i++)
        {
            promise = SceneLights[i].FadeIntensity(this, LightIntensities[i], 0, FadeSeconds);
        }

        promise.Then(() =>
        {
            SceneCamera.enabled = false;
            gameObject.SetActive(false);
        });

        return promise;
    }
}
