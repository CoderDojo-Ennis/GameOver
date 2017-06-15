using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InviteMenu : BaseMenu {

    [Header("Graphics")]
    public GameObject Background;

    [Header("Instructions")]
    public TextMeshProUGUI HeadingText;
    public float TypingSeconds = 0.05f;

    [Header("Sounds")]
    public AudioClip PlayerLostSound;

    /// <summary>
    /// Awake (before start)
    /// </summary>
    public void Start()
    {
    }

    /// <summary>
    /// Show the menu
    /// </summary>
    public override void ShowMenu()
    {
        Debug.Log("Show Invite Menu");
        base.ShowMenu();

        PlayMenuSound(PlayerLostSound);

        // todo - fade in
        //var backgroundImage = Background.GetComponent<Image>();
        //backgroundImage.GetComponent<CanvasRenderer>().FadeAlpha(this, 0, 1, 1);

        HeadingText.Type(this, TypingSeconds, true, () =>
        {
            AudioManager.Instance.PlayTypeCharacter();
        });
    }
}
