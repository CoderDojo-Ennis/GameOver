using UnityEngine;

public class AvatarLandInstructions : MonoBehaviour
{
    private AvatarScript Avatar;
    private AudioSource AudioSource;
    public InstructionsMenu InstructionsMenu;

    private void Awake()
    {
        this.Avatar = GetComponent<AvatarScript>();
        this.AudioSource = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {
        Avatar.SetAnimation("Idle");
        this.Delay(4f, () =>
        {
            Avatar.SetAnimation("WalkLeft");
            Avatar.GlideX(0, -8, 2).Then(() =>
            {
                InstructionsMenu.FadeToScene("GameOverScene");
            });
        });
    }
}
