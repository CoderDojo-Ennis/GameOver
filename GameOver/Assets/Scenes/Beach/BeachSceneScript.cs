using UnityEngine;

public class BeachSceneScript : BaseGameScene
{
    [Header("Sounds")]
    public AudioClip WelcomeSound;
    public AudioClip RejectedSound;

    // Cutscene sounds
    private AudioSource AudioSource;

    public new void Awake()
    {
        base.Awake();
        AudioSource = this.GetComponent<AudioSource>();
    }

    /// <summary>
    /// Start the cutscene
    /// </summary>
    public new void Start()
    {
        // Don't call base
        GameManager.Instance.ActiveGameScene = this;

        PlayerScript.Instance.HideKinect(0);

        FadeCameraIn();
        if (BackgroundMusic != null)
        {
            GameManager.Instance.PlayBackgroundMusic(BackgroundMusic);
        }

        // todo - change to proper scene
        this.Delay(5, () =>
        {
            FadeToScene("SeaScene");
        });
    }
}
