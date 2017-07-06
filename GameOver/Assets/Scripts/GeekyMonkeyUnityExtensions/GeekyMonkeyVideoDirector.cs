using System;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class GeekyMonkeyVideoDirector : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private bool fadingOutAudio;
    private bool fadingOutVideo;
    private bool fadingIn;
    private Color visibleColor = new Color(1, 1, 1, 1);
    private MeshRenderer quadRenderer;
    private Material videoMaterial;
    private bool isPlaying;
    private GmDelayPromise playPromise;
    private Camera videoCamera;
    private AudioSource audioSource;
    private TextMeshPro headingText;

    public static GeekyMonkeyVideoDirector Instance;

    [Header("Fade In")]
    public Color FadeInFrom;
    public float FadeInSeconds = 0;
    public float FadeInAudioSeconds = 0;

    [Header("Fade Out")]
    public Color FadeOutTo;
    public float FadeOutSeconds = 0;
    public float FadeOutAudioSeconds = 0;

    [Header("Limits")]
    public float MaxClipSeconds = 30;

    /// <summary>
    /// Is a video currently playing
    /// </summary>
    public bool IsPlaying
    {
        get
        {
            return isPlaying;
        }
    }

    /// <summary>
    /// Awake (before start)
    /// </summary>
    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Start (after awake)
    /// </summary>
    void Start()
    {
        Debug.Log("VideoDirector Start");

        audioSource = this.GetComponent<AudioSource>();
        videoPlayer = this.GetComponent<VideoPlayer>();
        videoCamera = this.GetComponentInChildren<Camera>(true);
        quadRenderer = this.GetComponentInChildren<MeshRenderer>();
        headingText = GameObject.Find("VideoHeading").GetComponent<TextMeshPro>();
        videoMaterial = quadRenderer.material;

        // Start faded out
        audioSource.volume = 0;
        videoCamera.enabled = false;
        headingText.SetAlpha(0);

        // Video prepare completed event
        videoPlayer.prepareCompleted += (sender) =>
        {
            Debug.Log("Prepare completed");
            isPlaying = true;
            FadeInVideo();
            FadeInAudio();
            videoPlayer.Play();
        };

        // ControlEvents
        GameGestureListener gameGestureListener = GameManager.Instance.GameGestureListener;
        if (gameGestureListener != null)
        {
            gameGestureListener.OnSwipeLeft += KinectSwipeHorizontal;
            gameGestureListener.OnSwipeRight += KinectSwipeHorizontal;
        }
    }

    /// <summary>
    /// un-hook events
    /// </summary>
    private void OnDestroy()
    {
        GameGestureListener gameGestureListener = GameManager.Instance.GameGestureListener;
        if (gameGestureListener != null)
        {
            gameGestureListener.OnSwipeLeft -= KinectSwipeHorizontal;
            gameGestureListener.OnSwipeRight -= KinectSwipeHorizontal;
        }
    }

    /// <summary>
    /// Kinect swipe left or right
    /// </summary>
    private void KinectSwipeHorizontal(object sender, EventArgs e)
    {
        if (isPlaying)
        {
            Abort();
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
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

            // Limit?
            if (this.videoPlayer.time >= this.MaxClipSeconds)
            {
                Abort();
            }

            // Are we there yet?
            if (this.videoPlayer.frameCount == (ulong)this.videoPlayer.frame)
            {
                ClipComplete();
            }
        }

        ProcessKeyboardInput();
    }

    /// <summary>
    /// Process keyboard input for the video director
    /// </summary>
    void ProcessKeyboardInput()
    {
        //if (isPlaying)
        //{
        // Abort
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Abort();
        }
        //}
    }

    /// <summary>
    /// Abort the video playback. Do fadeout if configured. Then trigger the complete event.
    /// </summary>
    public void Abort()
    {
        Debug.Log("Video abort request. isPlaying=" + isPlaying);

        // Is a video playing
        if (isPlaying)
        {
            if (fadingIn)
            {
                float secondsPlayed = (float)(this.videoPlayer.frameCount) / this.videoPlayer.frameRate;
                this.Delay(FadeInSeconds - secondsPlayed + .1f, Abort, true);
                return;
            }

            // Are we already fading out both the audio and video
            if (!fadingOutAudio || !fadingOutVideo)
            {
                float fadeOutSeconds = 0;

                if (!fadingOutAudio)
                {
                    fadeOutSeconds = FadeOutAudioSeconds;
                    FadeOutAudio();
                }

                if (!fadingOutVideo)
                {
                    fadeOutSeconds = Mathf.Max(fadeOutSeconds, FadeOutSeconds);
                    FadeOutVideo();
                }

                // After fade-out - stop the clip
                this.Delay(fadeOutSeconds, ClipComplete, true);
            }
        }
    }

    private void ClipComplete()
    {
        Debug.Log("Video End");
        isPlaying = false;
        videoMaterial.SetColor("_Color", visibleColor.WithAlpha(0));
        headingText.SetAlpha(0);
        fadingOutAudio = false;
        fadingOutVideo = false;
        videoPlayer.Stop();
        playPromise.Done();
        //videoCamera.enabled = false;
    }

    /// <summary>
    /// Begin Fade In Audio
    /// </summary>
    private void FadeInAudio()
    {
        if (FadeInAudioSeconds > 0)
        {
            audioSource.Fade(this, 0, 1, FadeInAudioSeconds, true);
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
        GameManager.Instance.ShowCamera(this.videoCamera);

        if (FadeInSeconds > 0)
        {
            fadingIn = true;
            videoMaterial.Fade(this, FadeInFrom, visibleColor, FadeInSeconds, true).Then(() =>
            {
                if (headingText != null)
                {
                    headingText.FadeAlpha(0, 1, FadeInSeconds, true).Then(() =>
                    {
                        fadingIn = false;
                    });
                }
                else
                {
                    fadingIn = false;
                }
            });
        }
        else
        {
            fadingIn = false;
            videoMaterial.SetColor("_Color", visibleColor);
            headingText.SetAlpha(1);
        }
    }

    /// <summary>
    /// Begin Fade Out of Video
    /// </summary>
    private void FadeOutVideo()
    {
        this.fadingOutVideo = true;
        if (FadeOutSeconds > 0)
        {
            headingText.FadeAlpha(1, 0, FadeOutSeconds / 2, true);
            videoMaterial.Fade(this, visibleColor, FadeOutTo, FadeOutSeconds, true);
        }
        else
        {
            videoMaterial.SetColor("_Color", visibleColor.WithAlpha(0));
            headingText.SetAlpha(0);
        }
    }

    /// <summary>
    /// Begin Fade Out of Audio
    /// </summary>
    private void FadeOutAudio()
    {
        this.fadingOutAudio = true;
        audioSource.Fade(this, 1, 0, FadeOutAudioSeconds, true);
    }

    /// <summary>
    /// Play a video clip using the current settings
    /// </summary>
    /// <param name="clip">The video clip to play (if null, then current clip will play)</param>
    public GmDelayPromise PlayClip(VideoClip clip = null, string heading = null)
    {
        // Heading
        if (headingText != null)
        {
            headingText.SetText(heading ?? "");
        }

        videoMaterial.SetColor("_Color", FadeInFrom);
        GameManager.Instance.ShowCamera(videoCamera);

        this.playPromise = new GmDelayPromise();

        // If no clip - play the one already set
        if (clip != null)
        {
            videoPlayer.clip = clip;
        }

        Debug.Log("Prepare");
        videoPlayer.Prepare();
        isPlaying = true;

        fadingIn = false;
        fadingOutAudio = false;
        fadingOutVideo = false;

        return playPromise;
    }
}
