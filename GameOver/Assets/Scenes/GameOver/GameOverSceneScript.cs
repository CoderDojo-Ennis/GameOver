using TMPro;
using UnityEngine;

public class GameOverSceneScript : BaseGameScene
{
    [Header("Transition")]
    public string NextScene = "Instructions_WarScene";
    public float Delay = 10f;

    [Header("Sounds")]
    public AudioClip GameOverSound;

    // Cutscene sounds
    private AudioSource AudioSource;

    private TextMeshPro Text;
    GmDelayPromise sceneDelay;

    public new void Awake()
    {
        base.Awake();
        AudioSource = this.GetComponent<AudioSource>();
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
        GameManager.Instance.DisableScoreCanvas();
        PlayerScript.Instance.HideKinect(0);

        FadeCameraIn();

        if (GameOverSound != null)
        {
            AudioSource.PlayOneShot(GameOverSound);
        }
        if (BackgroundMusic != null)
        {
            GameManager.Instance.PlayBackgroundMusic(BackgroundMusic);
        }

        this.Text = GetComponentInChildren<TMPro.TextMeshPro>();

        Text.Type(this, 0.1f, true, () =>
        {
            //AudioManager.Instance.PlayTypeCharacter();
        });

        GameManager.Instance.PauseBackroundMusic(Delay * .9f);
        sceneDelay = this.Delay(Delay, () =>
        {
            FadeToScene(NextScene);
        });
    }

    new void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            sceneDelay.Abort();
            FadeToScene(NextScene);
        }
    }
}
