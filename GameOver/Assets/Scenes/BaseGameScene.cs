using UnityEngine;
using UnityEngine.UI;

public class BaseGameScene : MonoBehaviour
{

    [Header("Background")]
    public Image BackgroundImage;

    [Header("Transition")]
    public float FadeSeconds = 2;

    public Light[] SceneLights;
    private float[] LightIntensities;

    [HideInInspector]
    public Camera SceneCamera;

    [HideInInspector]
    public bool IsShowingInstructions;

    // Use this for initialization
    public void Start()
    {
        Debug.Log("Base Start");

        GameManager.Instance.ActiveGameScene = this;

        // Hide the scene, and show instructions
        SceneCamera = this.gameObject.GetComponentInChildren<Camera>();
        if (SceneCamera != null)
        {
            SceneCamera.enabled = false;
        }
        if (this.BackgroundImage != null)
        {
            this.BackgroundImage.SetAlpha(0);
        }
        IsShowingInstructions = true;
        GameManager.Instance.ShowInstructions();

        // If no player found - start with invitation
        if (!GameManager.Instance.GameGestureListener.IsPlayerDetected)
        {
            GameManager.Instance.InviteGame();
        }
    }

    /// <summary>
    /// After instructions countdown
    /// </summary>
    public virtual void InstructionsComplete()
    {
        IsShowingInstructions = false;

        if (SceneLights != null)
        {
            LightIntensities = new float[SceneLights.Length];
            for (var i = 0; i < SceneLights.Length; i++)
            {
                LightIntensities[i] = SceneLights[i].intensity;
            }
        }

        if (SceneCamera != null)
        {
            SceneCamera.enabled = false;
            FadeIn();
        }
        //gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void Update()
    {

    }

    /// <summary>
    /// Fade in the lights in this scene
    /// </summary>
    /// <returns></returns>
    public virtual GmDelayPromise FadeIn()
    {
        Debug.Log("Fade In " + this.gameObject.name);

        if (SceneCamera != null)
        {
            SceneCamera.enabled = true;
        }
        GmDelayPromise promise = null;

        for (var i = 0; i < SceneLights.Length; i++)
        {
            promise = SceneLights[i].FadeIntensity(this, 0, LightIntensities[i], FadeSeconds);
        }

        if (this.BackgroundImage != null)
        {
            BackgroundImage.FadeAlpha(this, 0, 1, FadeSeconds);
        }

        return promise;
    }

    /// <summary>
    /// Fade out the lights in this scene
    /// </summary>
    /// <returns></returns>
    public virtual GmDelayPromise FadeOut()
    {
        Debug.Log("Fade Out " + this.gameObject.name);
        GmDelayPromise promise = null;

        if (SceneCamera != null)
        {
            SceneCamera.enabled = true;
        }

        if (SceneLights.Length > 0)
        {
            for (var i = 0; i < SceneLights.Length; i++)
            {
                promise = SceneLights[i].FadeIntensity(this, LightIntensities[i], 0, FadeSeconds);
            }
        }
        else
        {
            promise = this.Delay(0.1f);
        }

        promise.Then(() =>
        {
            if (SceneCamera != null)
            {
                SceneCamera.enabled = false;
            }
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
        PlayerScript.Instance.ScoreVisible = false;
        return GameManager.Instance.PlayNextPlyalistVideo(playlist);
    }

    /// <summary>
    /// The game has been paused
    /// </summary>
    public virtual void OnPause()
    {
    }

    /// <summary>
    /// The game has resumed
    /// </summary>
    public virtual void OnResume()
    {
    }
}
