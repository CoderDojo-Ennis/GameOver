using UnityEngine;

public class BaseGameScene : MonoBehaviour
{
    [Header("Background")]
    public AudioClip BackgroundMusic;

    [Header("Player")]
    public bool NaturalX = true;
    public float TravelScaleX = 10;
    private PlayerScript Player;
    public float PlayerScale = 1;

    [Header("Transition")]
    public float FadeSeconds = 1f;

    //[HideInInspector]
    public Camera SceneCamera;

    internal bool Paused;

    public void Awake()
    {
        Player = PlayerScript.Instance; //  GameObject.FindObjectOfType<PlayerScript>();
        SceneCamera = gameObject.GetComponentInChildren<Camera>(true);
    }

    // Use this for initialization
    public void Start()
    {
        Debug.Log("Base Start");

        GameManager.Instance.ActiveGameScene = this;

        this.Delay(0.2f, () =>
        {
            Player.TravelScaleX = this.TravelScaleX;
            Player.NaturalX = this.NaturalX;
            Player.transform.localScale = Vector3.one * PlayerScale;
        });

        if (BackgroundMusic != null)
        {
            GameManager.Instance.PlayBackgroundMusic(BackgroundMusic);
        }

        // If no player found - start with invitation
        if (!GameManager.Instance.GameGestureListener.IsPlayerDetected)
        {
            GameManager.Instance.InviteGame();
        }
        else
        {
            FadeCameraIn();
        }

        // When the player dies
        PlayerScript.Instance.OnDeath += (sender, args) =>
        {
            GameManager.Instance.HideKinect();
            FadeToScene("GameOverScene");
        };
    }

    // Update is called once per frame
    public void Update()
    {

    }

    /// <summary>
    /// Fade in the scene camera
    /// </summary>
    public virtual GmDelayPromise FadeCameraIn()
    {
        return GameManager.Instance.FadeCameraIn(this.FadeSeconds, this.SceneCamera);
    }

    /// <summary>
    /// Fade out this scene, then switch to another scene
    /// </summary>
    /// <param name="sceneName"></param>
    public void FadeToScene(string sceneName)
    {
        // Preload the next scene
        GameManager.Instance.PreloadScene(sceneName, false);

        // Fade out
        GameManager.Instance.FadeCameraOut(FadeSeconds).Then(() =>
        {
            GameManager.Instance.ShowScene(sceneName);
        });
    }

    // Shortcuts to game manager
    public void PreloadScene(string sceneName, bool showImmediately)
    {
        GameManager.Instance.PreloadScene(sceneName, showImmediately);
    }
    public void ShowScene(string sceneName)
    {
        GameManager.Instance.ShowScene(sceneName);
    }
    public GmDelayPromise PlayNextPlyalistVideo(VideoPlaylists playlist)
    {
        PlayerScript.Instance.ScoreVisible = false;
        return GameManager.Instance.PlayNextPlyalistVideo(playlist);
    }

    /// <summary>
    /// The game has been paused
    /// </summary>
    public virtual void OnPause()
    {
        Paused = true;
    }

    /// <summary>
    /// The game has resumed
    /// </summary>
    public virtual void OnResume()
    {
        Paused = false;
        FadeCameraIn();
    }
}
