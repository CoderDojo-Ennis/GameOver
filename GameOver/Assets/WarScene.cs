using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarScene : BaseGameScene {

    [Header("Bombs")]
    public GameObject BombEmitter;

	// Use this for initialization
	new void Start () {
        Debug.Log("War Start");
        base.Start();
	}
	
	// Update is called once per frame
	new void Update () {
        base.Update();

        if (!IsShowingInstructions)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                FadeToScene("SeaScene");

                //var seaScene = GameManager.Instance.Sea;
                //GameManager.Instance.ShowScene(seaScene);
            }
        }
    }

    /// <summary>
    /// Instructions complete - begin gameplay
    /// </summary>
    public override void InstructionsComplete()
    {
        base.InstructionsComplete();
        BombEmitter.GetComponent<BombEmitter>().StartBombing();
    }
}
