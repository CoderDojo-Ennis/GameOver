using System;
using TMPro;
using UnityEngine;

public class PauseMenu : BaseMenu
{
    [Header("Instructions")]
    public TextMeshPro PauseText;
    public float TypingSeconds = 0.05f;

    [Header("Sounds")]
    public AudioClip PlayerFoundSound;
    public AudioClip ResumeSound;

    private AvatarScript Avatar;

    /// <summary>
    /// Awake (before start)
    /// </summary>
    public void Awake()
    {
        Avatar = GetComponentInChildren<AvatarScript>(true);
        Avatar.gameObject.layer = this.gameObject.layer;
    }

    /// <summary>
    /// Start the pause menu
    /// </summary>
    public void Start()
    {
        GameGestureListener gameGestureListener = GameManager.Instance.GameGestureListener;
        gameGestureListener.OnSwipeLeft += KinectSwipeHorizontal;
        gameGestureListener.OnSwipeRight += KinectSwipeHorizontal;
        gameGestureListener.OnSwipeUp += Resume;
        gameGestureListener.OnOneHandUp += Resume;
    }

    private void Resume(object sender, EventArgs e)
    {
        if (GameManager.Instance.Paused)
        {
            PlayMenuSound(ResumeSound);
            GameManager.Instance.ResumeGame();
        }
    }

    /// <summary>
    /// Swipe gesture detected
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void KinectSwipeHorizontal(object sender, EventArgs e)
    {
        if (GameManager.Instance.Paused)
        {
            PlayMenuSound(ResumeSound);
            GameManager.Instance.ResumeGame();
        }
    }

    /// <summary>
    /// Show the menu
    /// </summary>
    public override void ShowMenu(float fadeSeconds)
    {
        Debug.Log("Show Pause Menu");
        base.ShowMenu(fadeSeconds);

        PlayMenuSound(PlayerFoundSound);

        PauseText.Type(this, TypingSeconds, true, () =>
        {
            AudioManager.Instance.PlayTypeCharacter();
        });
    }
}
