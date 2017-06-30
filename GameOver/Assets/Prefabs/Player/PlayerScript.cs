using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript Instance;

    public int InitialHealth = 60;
    public int Health = 60;
    //public float ImageScale = 1;
    //public float MoveScale = 1;
    public float JointScale = 10f;
    public Vector3 JointOffset;
    public float TravelScaleX = 1;
    public bool NaturalX = true;
    public Vector3 AspectScale = new Vector3(1, 1, 1);

    [Header("Health")]
    public Image[] Hearts;
    public Sprite HeartFull;
    public Sprite HeartHalf;
    public Sprite HeartEmpty;
    public AudioClip InjureSound;
    public AudioClip DeathSound;
    public event EventHandler OnDeath;

    [Header("Avatar")]
    public GameObject AvatarPrefab;
    public GameObject TransformEffect;

    private AudioSource AudioSource;

    internal void HideKinect(float fadeSeconds)
    {
        PlayerImage.FadeOut(fadeSeconds);
    }

    internal void ShowKinect(float fadeSeconds)
    {
        PlayerImage.FadeIn(fadeSeconds);
    }

    public PlayerImageScript PlayerImage;

    private Canvas ScoreCanvasCanvas;

    public void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
        PlayerImage = GetComponentInChildren<PlayerImageScript>();
        ScoreCanvasCanvas = GameObject.Find("ScoreCanvas").GetComponent<Canvas>();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Start (After Awake)
    /// </summary>
    public void Start()
    {
        SetInitialHealth();

        /*
        this.transform.localScale = new Vector3(ImageScale, ImageScale);
        if (MoveScale == 1)
        {
            MoveScale = 1 / ImageScale;
        }
        */
    }

    public AvatarScript ChangeToAvatar()
    {
        if (TransformEffect != null)
        {
            // It auto disables
            TransformEffect.SetActive(true);
        }
        float fadeSeconds = 0.5f;
        HideKinect(fadeSeconds);
        var avatar = GameObject.Instantiate(AvatarPrefab).GetComponent<AvatarScript>();
        avatar.gameObject.layer = LayerMask.NameToLayer("Default");
        avatar.transform.position = this.transform.position;
        avatar.FadeIn(fadeSeconds);
        return avatar;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha0))
        {
            HideKinect(1);
        }
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            ShowKinect(1);
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            ChangeToAvatar();
        }
    }

    /// <summary>
    /// Should the score/health be shown
    /// </summary>
    public bool ScoreVisible
    {
        set
        {
            ScoreCanvasCanvas.enabled = value;
        }
    }

    /// <summary>
    /// Set full health at the beginning of the game
    /// </summary>
    public void SetInitialHealth()
    {
        Health = InitialHealth;
        DisplayHealth();
    }

    /// <summary>
    /// Take damage
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage)
    {
        Health -= damage;
        DisplayHealth();
        PlayerImage.ShowDamaged();
        if (Health > 0)
        {
            AudioSource.PlayOneShot(InjureSound);
        }
        else
        {
            AudioSource.PlayOneShot(DeathSound);
            if (OnDeath != null)
            {
                OnDeath(this, null);
            }
        }
    }

    private void DisplayHealth()
    {
        var hp = Health;
        for (var h = 0; h < Hearts.Length; h++)
        {
            Sprite heartSprite = HeartEmpty;
            if (hp >= 20)
            {
                heartSprite = HeartFull;
            }
            else if (hp >= 10)
            {
                heartSprite = HeartHalf;
            }
            Hearts[h].sprite = heartSprite;
            hp -= 20;
        }
    }

    public void SetX(float x)
    {
        transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
    }
}
