using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GeekyMonkeyVideoDirector : MonoBehaviour {

	private VideoPlayer videoPlayer;
    private bool fadingOut;
    private Color visibleColor = new Color(1, 1, 1, 1);
    private MeshRenderer quadRenderer;
    private Material videoMaterial;
    private bool isPlaying;
    private GmDelayPromise playPromise;
    private Camera videoCamera;

    public static GeekyMonkeyVideoDirector Instance;

    [Header("Fade In")]
    public Color FadeInFrom;
    public float FadeInSeconds = 0;

    [Header("Fade Out")]
    public Color FadeOutTo;
    public float FadeOutSeconds = 0;

    // Use this for initialization
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        videoPlayer = this.GetComponent<VideoPlayer>();
        videoCamera = this.GetComponentInChildren<Camera>(true);
        quadRenderer = this.GetComponentInChildren<MeshRenderer>();
        videoMaterial = quadRenderer.material;

        videoCamera.enabled = false;

        videoPlayer.prepareCompleted += (sender) => {
            Debug.Log("Prepare completed");
            isPlaying = true;
            FadeIn();
            videoCamera.enabled = true;
            videoPlayer.Play();
        };
    }

    // Update is called once per frame
    void Update ()
    {
        if (isPlaying)
        {
            // Detect fade out time
            if (FadeOutSeconds > 0 && !fadingOut)
            {
                float secondsRemaining = ((float)(this.videoPlayer.frameCount - (ulong)this.videoPlayer.frame)) / this.videoPlayer.frameRate;
                if (secondsRemaining <= FadeOutSeconds)
                {
                    FadeOut();
                }
            }

            if (this.videoPlayer.frameCount == (ulong)this.videoPlayer.frame)
            {
                ClipComplete();
            }
        }
    }

    private void ClipComplete()
    {
        Debug.Log("Video End");
        isPlaying = false;
        fadingOut = false;
        videoPlayer.Stop();
        playPromise.Done();
        videoCamera.enabled = false;
    }

    private void FadeIn()
    {
        if (FadeInSeconds > 0)
        {
            videoMaterial.Fade(this, FadeInFrom, visibleColor, FadeInSeconds);
        } else
        {
            videoMaterial.SetColor("_Color", visibleColor);
        }
    }

    /// <summary>
    /// Begin Fade Out
    /// </summary>
    private void FadeOut()
    {
        this.fadingOut = true;
        videoMaterial.Fade(this, visibleColor, FadeOutTo, FadeInSeconds);
    }

    /// <summary>
    /// Play a video clip using the current settings
    /// </summary>
    /// <param name="clip">The video clip to play (if null, then current clip will play)</param>
    public GmDelayPromise PlayClip(VideoClip clip = null)
    {
        this.playPromise = new GmDelayPromise();

        // If no clip - play the one lareay dset
        if (clip != null)
        {
            videoPlayer.clip = clip;
        }

        Debug.Log("Prepare");
        videoPlayer.Prepare();

        fadingOut = false;
        videoPlayer.Play();

        return playPromise;
    }
}
