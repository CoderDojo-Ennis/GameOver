using UnityEngine;

public class BeachSceneScript : BaseGameScene
{
    public GameObject ForgotThought;
    public GameObject ForgotPassport;
    public GameObject ForgotSuitcase;

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
        SceneRequiresPlayer = false;
    }

    /// <summary>
    /// Start the cutscene
    /// </summary>
    public override void FirstUpdate()
    {
        //base.FirstUpdate();

        // Don't call base
        GameManager.Instance.ActiveGameScene = this;

        PlayerScript.Instance.HideKinect(0);

        BoatGuard.sprite = BoatGuyIdle;
        CoinThought.SetActive(false);
        ForgotSuitcase.SetActive(false);
        ForgotPassport.SetActive(false);
        ForgotThought.SetActive(false);

        FadeCameraIn();
        if (BackgroundMusic != null)
        {
            GameManager.Instance.PlayBackgroundMusic(BackgroundMusic);
        }

        ShowCollectables();

        Avatar.SetAnimation("WalkRight");

        // Have belongings - Walk to boat guy
        if (HavePassport && HaveSuitcase)
        {
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
        else
        {
            // Forgot something
            Avatar.GlideX(-6.8f, -1f, 1.2f).Then(() =>
            {
                Avatar.SetAnimation("Idle");

                // stop & think
                this.Delay(1, () =>
                {
                    AudioSource.PlayOneShot(RejectedSound);
                    ForgotThought.SetActive(true);
                    if (HaveSuitcase)
                    {
                        ForgotPassport.SetActive(true);
                    }
                    else
                    {
                        ForgotSuitcase.SetActive(true);
                    }

                    this.Delay(4, () =>
                    {
                        ForgotThought.SetActive(false);

                        // Go back
                        Avatar.SetAnimation("WalkLeft");
                        Avatar.GlideX(-1f, -7f, 1.2f).Then(() =>
                        {
                            FadeToScene("WarScene");
                        });
                    });
                });
            });
        }
    }

    void GiveCoin()
    {
        CoinThought.SetActive(false);
        Coin.gameObject.transform.GlidePosition(this, Coin.transform.position, BoatGuardHand.position, 1f, false).Then(() =>
        {
            Coin.SetActive(false);

            BoatGuard.sprite = BoatGuyWelcome;
            AudioSource.PlayOneShot(WelcomeSound);
            this.Delay(1.5f, () =>
            {
                var AvatarInBoat = new Vector3(5f, Avatar.transform.localPosition.y, Avatar.transform.localPosition.z);
                Avatar.SetAnimation("WalkRight");
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
        AudioSource.PlayOneShot(RejectedSound);
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
        string PlayerPrefKey = "Collectable_" + collectable.gameObject.name;
        bool haveIt = PlayerPrefs.GetInt(PlayerPrefKey) == 1;
        Debug.Log("Have " + gameObject.name + " = " + haveIt);
        collectable.SetActive(haveIt);
        return haveIt;
    }
}
