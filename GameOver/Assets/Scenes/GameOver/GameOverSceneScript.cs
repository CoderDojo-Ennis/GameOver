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

    public new void Awake()
    {
        base.Awake();
        AudioSource = this.GetComponent<AudioSource>();
    }

    /// <summary>
    /// Start the cutscene
    /// </summary>
    public override void FirstUpdate()
    {
        base.FirstUpdate();

        // Don't call base
        GameManager.Instance.ActiveGameScene = this;

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
        this.Delay(Delay, () =>
        {
            FadeToScene(NextScene);
        });
    }
}
