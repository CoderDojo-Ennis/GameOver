﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarScene : BaseGameScene {

	// Use this for initialization
	new void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	new void Update () {
        base.Update();
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            var seaScene = GameManager.Instance.Sea;
            GameManager.Instance.ShowScene(seaScene);
        }
    }
}