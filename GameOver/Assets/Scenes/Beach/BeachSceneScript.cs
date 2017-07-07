using UnityEngine;

public class BeachSceneScript : BaseGameScene
{
    [Header("Sounds")]
    public AudioClip WelcomeSound;
    public AudioClip RejectedSound;

    [Header("Guard")]
    public SpriteRenderer BoatGuard;
    public Transform BoatGuardHand;
    public GameObject CoinThought;
    public Sprite BoatGuyIdle;
    public Sprite BoatGuyGimme;
    public Sprite BoatGuyReject;
    public Sprite BoatGuyWelcome;

    [Header("Collectables")]
    public GameObject Coin;
    public GameObject Passport;
    public GameObject Suitcase;
    public bool HaveCoin;
    public bool HavePassport;
    public bool HaveSuitcase;

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

        ShowCollectables();

        Avatar.SetAnimation("WalkRight");
        Avatar.GlideX(-6.8f, 1.4f, 2f).Then(() =>
        {
            Avatar.SetAnimation("IdleRight");

            // accept or reject
            this.Delay(1, () =>
            {
                if (HaveCoin)
                {
                    GiveCoin();
                }
                else
                {
                    Rejected();
                }
            });
        });

        this.Delay(2f, () =>
        {
            BoatGuard.sprite = BoatGuyGimme;
            CoinThought.SetActive(true);
        });
    }

    void GiveCoin()
    {
        CoinThought.SetActive(false);
        Coin.gameObject.transform.GlidePosition(this, Coin.transform.position, BoatGuardHand.position, 1f, false).Then(() =>
        {
            Coin.SetActive(false);

            BoatGuard.sprite = BoatGuyWelcome;
            this.Delay(1.5f, () =>
            {
                var AvatarInBoat = new Vector3(5f, Avatar.transform.localPosition.y, Avatar.transform.localPosition.z);
                Avatar.transform.GlideLocalPosition(this, Avatar.transform.localPosition, AvatarInBoat, 1f, false).Then(() =>
                {
                    Avatar.SetAnimation("Idle");
                    FadeToScene("Instructions_SeaScene");
                });
            });
        });
    }

    void Rejected()
    {
        BoatGuard.sprite = BoatGuyReject;
        this.Delay(2.5f, () =>
        {
            BoatGuard.sprite = BoatGuyIdle;
            CoinThought.SetActive(false);
        });
        this.Delay(1.5f, () =>
        {
            Avatar.SetAnimation("WalkLeft");
            Avatar.GlideX(1.4f, -7.2f, 2f).Then(() =>
            {
                FadeToScene("WarScene");
            });
        });
    }

    void ShowCollectables()
    {
        HavePassport = ShowCollectable(Passport);
        HaveCoin = ShowCollectable(Coin);
        HaveSuitcase = ShowCollectable(Suitcase);
    }

    bool ShowCollectable(GameObject collectable)
    {
        string PlayerPrefKey = "Collectable_" + this.gameObject.name;
        bool haveIt = PlayerPrefs.GetInt(PlayerPrefKey) == 1;
        collectable.SetActive(haveIt);
        return haveIt;
    }
}
