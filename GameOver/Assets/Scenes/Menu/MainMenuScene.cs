using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScene : BaseGameScene
{
    // Use this for initialization
    new void Start()
    {
        Debug.Log("Main Menu Start");
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (!IsShowingInstructions)
        {
        }
    }

    /// <summary>
    /// Instructions complete - begin gameplay
    /// </summary>
    public override void InstructionsComplete()
    {
        base.InstructionsComplete();
        PreloadScene("WarScene", false);
        PlayNextPlyalistVideo(VideoPlaylists.War).Then(() =>
        {
            FadeToScene("WarScene");
        });
    }
}
