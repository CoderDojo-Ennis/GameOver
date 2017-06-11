using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaScene : BaseGameScene {

	// Use this for initialization
	new void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	new void Update () {
        base.Update();
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            FadeToScene("IntroScene");
            //var warScene = GameManager.Instance.War;
            //GameManager.Instance.ShowScene(warScene);
        }
    }
}
