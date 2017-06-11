﻿using System.Collections;
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
        Debug.Log("Base Start");

        if (SceneLights != null)
        {
            LightIntensities = new float[SceneLights.Length];
            for (var i = 0; i < SceneLights.Length; i++)
            {
                LightIntensities[i] = SceneLights[i].intensity;
            }
        }

        SceneCamera = this.gameObject.GetComponentInChildren<Camera>();
        if (SceneCamera != null)
        {
            SceneCamera.enabled = false;
            FadeIn();
        }
        //gameObject.SetActive(false);
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
        Debug.Log("Fade In " + this.gameObject.name);

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
        Debug.Log("Fade Out " + this.gameObject.name);
        GmDelayPromise promise = null;

        SceneCamera.enabled = true;

        if (SceneLights.Length > 0)
        {
            for (var i = 0; i < SceneLights.Length; i++)
            {
                promise = SceneLights[i].FadeIntensity(this, LightIntensities[i], 0, FadeSeconds);
            }
        } else
        {
            promise = this.Delay(0.1f);
        }

        promise.Then(() =>
        {
            SceneCamera.enabled = false;
            //gameObject.SetActive(false);
        });

        return promise;
    }

    /// <summary>
    /// Fade out this scene, then switch to another scene
    /// </summary>
    /// <param name="sceneName"></param>
    public void FadeToScene(string sceneName)
    {
        // Preload the next scene
        GameManager.Instance.PreloadScene(sceneName, false);

        // Fade out
        this.FadeOut().Then(() =>
        {
            GameManager.Instance.ShowScene(sceneName);
        });
    }

    // Shortcuts to game manager
    public void PreloadScene(string sceneName, bool showImmediately)
    {
        GameManager.Instance.PreloadScene(sceneName, showImmediately);
    }
    public void ShowScene(string sceneName)
    {
        GameManager.Instance.ShowScene(sceneName);
    }
    public GmDelayPromise PlayNextPlyalistVideo(VideoPlaylists playlist)
    {
        return GameManager.Instance.PlayNextPlyalistVideo(playlist);
    }
}
