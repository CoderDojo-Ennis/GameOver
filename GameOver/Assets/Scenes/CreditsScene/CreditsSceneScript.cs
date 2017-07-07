using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreditsSceneScript : BaseGameScene {
    public SpriteRenderer[] Characters;
    public float CharacterIntroDelay;
    public float CharacterDelay;
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
    public override void FirstUpdate()
    {
        // Don't call base
        GameManager.Instance.ActiveGameScene = this;
        GameManager.Instance.DisableScoreCanvas();
        PlayerScript.Instance.HideKinect(0);

        FadeCameraIn();
        if (BackgroundMusic != null)
        {
            GameManager.Instance.PlayBackgroundMusic(BackgroundMusic);
        }

        Text = GetComponentInChildren<TextMeshPro>();

        Text.Type(this, 0.05f, true, () =>
        {
            //AudioManager.Instance.PlayTypeCharacter();
        });
        this.Delay(CharacterIntroDelay, () =>
        {
            FadeCharacters(0);
        });
        GameManager.Instance.PauseBackroundMusic(Delay * .9f);
        this.Delay(Delay, () =>
        {
            GameManager.Instance.EnableScoreCanvas();
            FadeToScene(NextScene);
        });
    }

    void FadeCharacters(int index)
    {
        Characters[index].FadeAlpha(this, 0, 1, 1, true);
        Debug.Log(Characters.Length + " " + index);
        if (index < Characters.Length - 1)
        {
            this.Delay(CharacterDelay, () => {
                FadeCharacters(index + 1);
            });
        }
    }
}