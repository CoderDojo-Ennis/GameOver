﻿using System;
using TMPro;
using UnityEngine;

public class InstructionsMenu : BaseGameScene
{
    [Header("Instructions")]
    public TextMeshPro InstructionText;
    public float TypingSeconds = 0.05f;

    [Header("Transition To")]
    public VideoPlaylists VideoPlaylist;
    public string GameSceneName = "WarScene";

    [Header("Countdown")]
    private TextMeshProUGUI CountdownText;
    public float PreCountdownSeconds = 4;
    public int CountdownSeconds = 3;
    private int SecondsRemaining;

    [Header("Sounds")]
    public AudioClip CountdownSecondSound;
    public AudioClip CountdownGoSound;

    // Menu sounds
    private AudioSource AudioSource;

    public new void Awake()
    {
        base.Awake();
        AudioSource = this.GetComponent<AudioSource>();
        this.CountdownText = GameObject.Find("CountDownText").GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Start the pause menu
    /// </summary>
    public new void Start()
    {
        // Don't call base
        Debug.Log("Instructions Start");
        GameManager.Instance.ActiveGameScene = this;

        this.Delay(.01f, () =>
        {
            if (VideoPlaylist != VideoPlaylists.None)
            {
                PlayNextPlyalistVideo(VideoPlaylist).Then(() =>
                {
                    InviteAndShowinstructions();
                });
            }
            else
            {
                InviteAndShowinstructions();
            }
        });
    }

    public void InviteAndShowinstructions()
    {
        if (!GameManager.Instance.GameGestureListener.IsPlayerDetected)
        {
            GameManager.Instance.InviteGame().Then(ShowInstructions);
        }
        else
        {
            ShowInstructions();
        }
    }

    /// <summary>
    /// Show the instructions
    /// </summary>
    public void ShowInstructions()
    {
        Debug.Log("Instructions Show");

        FadeCameraIn();
        PlayerScript.Instance.ScoreVisible = true;
        this.CountdownText.text = "";
        GameManager.Instance.GameGestureListener.OnSwipeLeft += KinectSwipeHorizontal;
        GameManager.Instance.GameGestureListener.OnSwipeRight += KinectSwipeHorizontal;

        InstructionText.Type(this, TypingSeconds, true, () =>
        {
            AudioManager.Instance.PlayTypeCharacter();
        });

        StartCountdown();
    }

    /// <summary>
    /// Swipe gesture detected
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void KinectSwipeHorizontal(object sender, EventArgs e)
    {
        Debug.Log("Instructions Swipe");

        // Is counting down
        if (this.CountdownText.text != "")
        {
            // Skip countdown
            if (SecondsRemaining > 0)
            {
                SecondsRemaining = -1;
                AudioSource.PlayOneShot(CountdownGoSound);
                GameManager.Instance.FadeToScene(GameSceneName, FadeSeconds);
            }
        }
    }

    /// <summary>
    /// Show the instructions
    /// </summary>
    public void ShowMenu()
    {
        Debug.Log("ShowMenu()");

        /*
        // todo - fade in
        //var backgroundImage = Background.GetComponent<Image>();
        //backgroundImage.GetComponent<CanvasRenderer>().FadeAlpha(this, 0, 1, 1);

        // Which instructions?
        string activeSceneName = GameManager.Instance.ActiveGameScene.name;
        Debug.Log("Show instructions for " + activeSceneName);
        string instructionText;
        switch (activeSceneName)
        {
            case "MainMenu":
                instructionText = "Survive:\n\n*War\n*Sea\n*Land";
                break;
            case "War":
                instructionText = "War!\n\n* Dodge Bombs\n* Collect\n  Necessities";
                break;
            default:
                instructionText = "(missing instructions: " + activeSceneName + ")";
                break;
        }

        HeadingText.text = instructionText;

        // todo - use swipe to abort typing
        HeadingText.Type(this, TypingSeconds, false, () =>
        {
            AudioManager.Instance.PlayTypeCharacter();
        });
        */
    }

    /// <summary>
    /// Start the countdown timer
    /// </summary>
    public void StartCountdown()
    {
        Debug.Log("Instructions StartCountdown");

        GameManager.Instance.PreloadScene("WarScene", false);

        this.CountdownText.text = "";
        SecondsRemaining = CountdownSeconds;

        // Pre-countdown delay
        this.Delay(PreCountdownSeconds, () =>
        {
            Debug.Log("Pre-Countdown complete");

            // Number of seconds
            this.Repeat(1f, this.CountdownSeconds + 1, () =>
            {
                Debug.Log("Countdown seconds remaining: " + SecondsRemaining);

                if (SecondsRemaining < 0)
                {
                    // Aborted
                    this.CountdownText.text = "";
                    return;
                }
                if (SecondsRemaining == 0)
                {
                    this.CountdownText.text = "Go!";
                    AudioSource.PlayOneShot(CountdownGoSound);

                    // Instructions complete
                    this.Delay(1f, () =>
                    {
                        this.CountdownText.text = "";
                        GameManager.Instance.FadeToScene(GameSceneName, FadeSeconds);
                    });
                }
                else
                {
                    this.CountdownText.text = SecondsRemaining.ToString();
                    AudioSource.PlayOneShot(CountdownSecondSound);
                    SecondsRemaining--;
                }

                // Scale to designed size over one second
                float scale2 = 1.6f;
                float scale = scale2;
                int scaleSteps = 4;
                this.CountdownText.rectTransform.localScale = new Vector3(scale, scale);
                this.Repeat(1 / (scaleSteps + 1), scaleSteps, () =>
                {
                    // Aborted
                    if (SecondsRemaining < 0)
                    {
                        this.CountdownText.text = "";
                        return;
                    }
                    scale -= (scale2 - 1) / (float)scaleSteps;
                    this.CountdownText.rectTransform.localScale = new Vector3(scale, scale);
                });
            });
        });
    }
}
