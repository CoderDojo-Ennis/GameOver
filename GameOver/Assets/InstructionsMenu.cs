using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsMenu : BaseMenu {

    public GameObject Background;

    [Header("Instructions")]
    public TextMeshProUGUI HeadingText;
    public float TypingSeconds = 0.05f;

    [Header("Countdown")]
    public TextMeshProUGUI CountdownText;
    public int CountdownSeconds = 3;
    private int SecondsRemaining;

    [Header("Sounds")]
    public AudioClip CountdownSecondSound;
    public AudioClip CountdownGoSound;

    // Menu sounds
    private AudioSource AudioSource;

    /// <summary>
    /// Start the pause menu
    /// </summary>
    public void Start()
    {
        AudioSource = this.GetComponent<AudioSource>();

        GameManager.Instance.GameGestureListener.OnSwipeLeft += KinectSwipeHorizontal;
        GameManager.Instance.GameGestureListener.OnSwipeRight += KinectSwipeHorizontal;
    }

    /// <summary>
    /// Show the instructions
    /// </summary>
    public void ShowInstructions()
    {
        StartCountdown();
    }

    /// <summary>
    /// Swipe gesture detected
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void KinectSwipeHorizontal(object sender, EventArgs e)
    {
        //todo: Skip countdown
    }

    public override void ShowMenu()
    {
        Debug.Log("Show Instructions");
        base.ShowMenu();

        // todo - fade in
        //var backgroundImage = Background.GetComponent<Image>();
        //backgroundImage.GetComponent<CanvasRenderer>().FadeAlpha(this, 0, 1, 1);

        HeadingText.Type(this, TypingSeconds, false, () =>
        {
            AudioManager.Instance.PlayTypeCharacter();
        });
    }

    /// <summary>
    /// Start the countdown timer
    /// </summary>
    public void StartCountdown()
    {
        this.CountdownText.text = "";
        SecondsRemaining = CountdownSeconds;

        // Pre-countdown delay
        this.Delay(3f, () => {
            // Number of seconds
            this.Repeat(1f, this.CountdownSeconds + 1, () =>
            {
                if (SecondsRemaining == 0)
                {
                    this.CountdownText.text = "Go!";
                    AudioSource.PlayOneShot(CountdownGoSound);

                    // Instructions complete
                    this.Delay(1f, () => {
                        GameManager.Instance.HideInstructions();
                        GameManager.Instance.ActiveGameScene.InstructionsComplete();
                    });
                } else
                {
                    this.CountdownText.text = SecondsRemaining.ToString();
                    AudioSource.PlayOneShot(CountdownSecondSound);
                }
                SecondsRemaining--;

                // Scale to designed size over one second
                float scale2 = 1.6f;
                float scale = scale2;
                int scaleSteps = 4;
                this.CountdownText.rectTransform.localScale = new Vector3(scale,scale);
                this.Repeat(1 / (scaleSteps + 1), scaleSteps, () =>
                {
                    scale -= (scale2 - 1) / (float)scaleSteps;
                    this.CountdownText.rectTransform.localScale = new Vector3(scale, scale);
                });
            });
        });
    }
}
