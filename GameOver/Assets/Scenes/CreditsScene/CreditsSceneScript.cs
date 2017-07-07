using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreditsSceneScript : BaseGameScene {
    [Header("Transition")]
    public string NextScene = "Instructions_WarScene";
    public float Delay = 10f;

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

        Text = GetComponentInChildren<TextMeshPro>();

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