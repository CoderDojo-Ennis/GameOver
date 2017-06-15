using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : BaseMenu {

    [Header("Graphics")]
    public GameObject Background;

    [Header("Instructions")]
    public TextMeshProUGUI HeadingText;
    public float TypingSeconds = 0.05f;

    [Header("Sounds")]
    public AudioClip PlayerFoundSound;
    public AudioClip ResumeSound;

    /// <summary>
    /// Awake (before start)
    /// </summary>
    public void Awake()
    {
    }

    /// <summary>
    /// Start the pause menu
    /// </summary>
    public void Start()
    {
        GameManager.Instance.GameGestureListener.OnSwipeLeft += KinectSwipeHorizontal;
        GameManager.Instance.GameGestureListener.OnSwipeRight += KinectSwipeHorizontal;
    }

    /// <summary>
    /// Swipe gesture detected
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void KinectSwipeHorizontal(object sender, EventArgs e)
    {
        PlayMenuSound(ResumeSound);
        GameManager.Instance.ResumeGame();
    }

    /// <summary>
    /// Show the menu
    /// </summary>
    public override void ShowMenu()
    {
        Debug.Log("Show Pause Menu");
        base.ShowMenu();

        PlayMenuSound(PlayerFoundSound);

        // todo - fade in
        //var backgroundImage = Background.GetComponent<Image>();
        //backgroundImage.GetComponent<CanvasRenderer>().FadeAlpha(this, 0, 1, 1);

        HeadingText.Type(this, TypingSeconds, true, () =>
        {
            AudioManager.Instance.PlayTypeCharacter();
        });
    }
}
