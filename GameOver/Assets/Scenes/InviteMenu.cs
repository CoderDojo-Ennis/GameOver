using TMPro;
using UnityEngine;

public class InviteMenu : BaseMenu
{
    [Header("Instructions")]
    public TextMeshPro InstructionText;
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
    public override void ShowMenu(float fadeSeconds)
    {
        Debug.Log("Show Invite Menu");
        base.ShowMenu(fadeSeconds);

        PlayMenuSound(PlayerLostSound);

        InstructionText.Type(this, TypingSeconds, true, () =>
        {
            AudioManager.Instance.PlayTypeCharacter();
        });
    }
}
