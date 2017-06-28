using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public int InitialHealth = 60;
    public int Health = 60;
    //public float ImageScale = 1;
    //public float MoveScale = 1;
    public float JointScale = 10f;
    public Vector3 JointOffset;
    public float TravelScaleX = 1;
    public bool NaturalX = true;

    public Image[] Hearts;
    public Sprite HeartFull;
    public Sprite HeartHalf;
    public Sprite HeartEmpty;
    public Vector3 AspectScale = new Vector3(1, 1, 1);

    public static PlayerScript Instance;
    public PlayerImageScript PlayerImage;

    private Canvas ScoreCanvasCanvas;

    public void Awake()
    {
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
