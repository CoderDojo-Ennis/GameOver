using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : BaseMenu {

    public GameObject Background;
    public TextMeshProUGUI HeadingText;
    public float TypingSeconds = 0.05f;

    public override void ShowMenu()
    {
        Debug.Log("Show Pause Menu");
        base.ShowMenu();

        // todo - fade in
        //var backgroundImage = Background.GetComponent<Image>();
        //backgroundImage.GetComponent<CanvasRenderer>().FadeAlpha(this, 0, 1, 1);

        HeadingText.Type(this, TypingSeconds, true, () =>
        {
            AudioManager.Instance.PlayTypeCharacter();
        });
    }
}
