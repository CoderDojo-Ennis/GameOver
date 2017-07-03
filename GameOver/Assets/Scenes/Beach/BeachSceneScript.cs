using UnityEngine;

public class BeachSceneScript : BaseGameScene
{
    [Header("Sounds")]
    public AudioClip WelcomeSound;
    public AudioClip RejectedSound;

    [Header("Guard")]
    public SpriteRenderer BoatGuard;
    public GameObject CoinThought;
    public Sprite BoatGuyIdle;
    public Sprite BoatGuyGimme;

    private AvatarScript Avatar;

    // Cutscene sounds
    private AudioSource AudioSource;

    public new void Awake()
    {
        base.Awake();
        AudioSource = this.GetComponent<AudioSource>();
        Avatar = GetComponentInChildren<AvatarScript>();
    }

    /// <summary>
    /// Start the cutscene
    /// </summary>
    public new void Start()
    {
        // Don't call base
        GameManager.Instance.ActiveGameScene = this;

        PlayerScript.Instance.HideKinect(0);

        BoatGuard.sprite = BoatGuyIdle;
        CoinThought.SetActive(false);

        FadeCameraIn();
        if (BackgroundMusic != null)
        {
            GameManager.Instance.PlayBackgroundMusic(BackgroundMusic);
        }

        Avatar.SetAnimation("WalkRight");
        Avatar.GlideX(-6.8f, 1.4f, 2f).Then(() =>
        {
            Avatar.SetAnimation("Idle");

            // todo - accept or reject
        });

        this.Delay(1.9f, () =>
        {
            BoatGuard.sprite = BoatGuyGimme;
            CoinThought.SetActive(true);
        });

        // todo - change to proper scene
        this.Delay(8, () =>
        {
            FadeToScene("Instructions_SeaScene");
        });
    }
}
