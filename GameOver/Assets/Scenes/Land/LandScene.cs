using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandScene : BaseGameScene
{
    public static LandScene instance;
    public LandCollectable BoltCutters;
    public float DelayBeforeCutters;
    public GameObject CutterInfo;

	new void Start ()
    {
        base.Start();
        instance = this;
        Invoke("DropCutters", DelayBeforeCutters);
        CutterInfo.SetActive(false);
	}
	
    void DropCutters()
    {
        BoltCutters.Fall();
    }

    public void CuttersCollected()
    {
        CutterInfo.SetActive(true);
    }

    public void Win()
    {

    }

	public override void FirstUpdate ()
    {
        base.FirstUpdate();
	}

    new void Update()
    {
        base.Update();
    }
}
