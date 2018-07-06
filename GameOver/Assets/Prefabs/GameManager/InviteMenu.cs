using TMPro;
using UnityEngine;

public class InviteMenu : BaseMenu
{
    [Header("Instructions")]
    public TextMeshPro InstructionText;
    public float TypingSeconds = 0.05f;

    [Header("Sounds")]
    public AudioClip PlayerLostSound;

    [Header("Avatar")]
    private AvatarScript Avatar;

    private TextMeshProUGUI CountdownText;

    /// <summary>
    /// Awake (before start)
    /// </summary>
    public void Awake()
    {
        Avatar = GetComponentInChildren<AvatarScript>(true);
        Avatar.gameObject.layer = this.gameObject.layer;
        CountdownText = GameObject.Find("CountDownText").GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Start (after awake)
    /// </summary>
    public void Start()
    {
    }

    /// <summary>
    /// Show the menu
    /// </summary>
    public override void ShowMenu(float fadeSeconds)
    {
        //Debug.Log("Show Invite Menu");
        base.ShowMenu(fadeSeconds);

        PlayMenuSound(PlayerLostSound);

        InstructionText.Type(this, TypingSeconds, true, () =>
        {
            AudioManager.Instance.PlayTypeCharacter();
        });

        Avatar.SetAnimation("WalkRight");
        Avatar.GlideX(-7, 0, 1.2f).Then(() =>
        {
            Avatar.SetAnimation("Idle");
            CountdownText.text = "";
        });
    }
}
