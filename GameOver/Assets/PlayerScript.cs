using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public int InitialHealth = 60;
    public int Health = 60;

    public Image[] Hearts;
    public Sprite HeartFull;
    public Sprite HeartHalf;
    public Sprite HeartEmpty;

    public static PlayerScript Instance;

    private Canvas ScoreCanvasCanvas;

    public void Awake()
    {
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

    public void Start()
    {
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
}
