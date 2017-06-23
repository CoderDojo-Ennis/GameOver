﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : BaseGameScene
{
	// Use this for initialization
	new void Start () {
        // Don't call base. This one is unique
        // base.Start();

        GameManager.Instance.ActiveGameScene = this;
        GameManager.Instance.HideInstructions();

        StartIntro();
	}
	
	// Update is called once per frame
	new void Update () {
        base.Update();
	}

    /// <summary>
    /// Start the intro video, then transition to the war scene
    /// </summary>
    private void StartIntro()
    {
        PlayerScript.Instance.ScoreVisible = false;
        PreloadScene("MainMenuScene", false);
        PlayNextPlyalistVideo(VideoPlaylists.Intro).Then(() =>
        {
            ShowScene("MainMenuScene");
        });
    }
}