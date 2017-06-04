using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GeekyMonkeyVideoDirector : MonoBehaviour {

	private VideoPlayer videoPlayer;
    private bool fadingOutAudio;
    private bool fadingOutVideo;
    private Color visibleColor = new Color(1, 1, 1, 1);
    private MeshRenderer quadRenderer;
    private Material videoMaterial;
    private bool isPlaying;
    private GmDelayPromise playPromise;
    private Camera videoCamera;
    private AudioSource audioSource;

    public static GeekyMonkeyVideoDirector Instance;

    [Header("Fade In")]
    public Color FadeInFrom;
    public float FadeInSeconds = 0;
    public float FadeInAudioSeconds = 0;

    [Header("Fade Out")]
    public Color FadeOutTo;
    public float FadeOutSeconds = 0;
    public float FadeOutAudioSeconds = 0;

    // Use this for initialization
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        videoPlayer = this.GetComponent<VideoPlayer>();
        videoCamera = this.GetComponentInChildren<Camera>(true);
        quadRenderer = this.GetComponentInChildren<MeshRenderer>();
        videoMaterial = quadRenderer.material;

        audioSource.volume = 0;
        videoCamera.enabled = false;

        videoPlayer.prepareCompleted += (sender) => {
            Debug.Log("Prepare completed");
            isPlaying = true;
            FadeInVideo();
            FadeInAudio();
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
            if (FadeOutSeconds > 0 && !fadingOutVideo)
            {
                float secondsRemaining = ((float)(this.videoPlayer.frameCount - (ulong)this.videoPlayer.frame)) / this.videoPlayer.frameRate;
                if (secondsRemaining <= FadeOutSeconds)
                {
                    FadeOutVideo();
                }
            }
            if (FadeOutAudioSeconds > 0 && !fadingOutAudio)
            {
                float secondsRemaining = ((float)(this.videoPlayer.frameCount - (ulong)this.videoPlayer.frame)) / this.videoPlayer.frameRate;
                if (secondsRemaining <= FadeOutAudioSeconds)
                {
                    FadeOutAudio();
                }
            }

            if (this.videoPlayer.frameCount == (ulong)this.videoPlayer.frame)
            {
                ClipComplete();
            }

            // Abort
            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (!fadingOutAudio || !fadingOutVideo) {
                    if (!fadingOutAudio)
                    {
                        FadeOutAudio();
                    }

                    if (!fadingOutVideo)
                    {
                        FadeOutVideo();
                    }

                    // After fade-out - stop the clip
                    this.Delay(Mathf.Max(FadeOutAudioSeconds, FadeOutSeconds), ClipComplete);
                }
            }
        }
    }

    private void ClipComplete()
    {
        Debug.Log("Video End");
        isPlaying = false;
        fadingOutAudio = false;
        fadingOutVideo = false;
        videoPlayer.Stop();
        playPromise.Done();
        videoCamera.enabled = false;
    }

    /// <summary>
    /// Begin Fade In Audio
    /// </summary>
    private void FadeInAudio()
    {
        if (FadeInAudioSeconds > 0)
        {
            audioSource.Fade(this, 0, 1, FadeInAudioSeconds);
        }
        else
        {
            this.audioSource.volume = 1;
        }
    }

    /// <summary>
    /// Begin Fade In Video
    /// </summary>
    private void FadeInVideo()
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
    /// Begin Fade Out of Video
    /// </summary>
    private void FadeOutVideo()
    {
        this.fadingOutVideo = true;
        videoMaterial.Fade(this, visibleColor, FadeOutTo, FadeOutSeconds);
    }

    /// <summary>
    /// Begin Fade Out of Audio
    /// </summary>
    private void FadeOutAudio()
    {
        this.fadingOutAudio = true;
        audioSource.Fade(this, 1, 0, FadeOutAudioSeconds);
    }

    /// <summary>
    /// Play a video clip using the current settings
    /// </summary>
    /// <param name="clip">The video clip to play (if null, then current clip will play)</param>
    public GmDelayPromise PlayClip(VideoClip clip = null)
    {
        videoMaterial.SetColor("_Color", FadeInFrom);
        videoCamera.enabled = true;

        this.playPromise = new GmDelayPromise();

        // If no clip - play the one already set
        if (clip != null)
        {
            videoPlayer.clip = clip;
        }

        Debug.Log("Prepare");
        videoPlayer.Prepare();

        fadingOutAudio = false;
        fadingOutVideo = false;

        return playPromise;
    }
}
