using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandScene : BaseGameScene
{
    public static LandScene instance;
    public LandCollectable BoltCutters;
    public float DelayBeforeCutters;
    public GameObject CutterInfo;
    public Searchlight searchlight;
    private AudioSource LoseSound;
    private bool CanFail = true;
    private int CurrentPhase; //Phase 1 = searchlight      Phase 2 = searchlight + dog and guard

	new void Start ()
    {
        base.Start();
        instance = this;
        Invoke("DropCutters", DelayBeforeCutters);
        CutterInfo.SetActive(false);
        LoseSound = GetComponent<AudioSource>();
        CurrentPhase = 1;
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
        CurrentPhase++;
        if (CurrentPhase == 3)
        {
            // todo-winning scene
        }
    }

    public void Fail()
    {
        if (CanFail)
        {
            CanFail = false;
            LoseSound.Play();
            PlayerScript.Instance.Damage(10, false, false);
            this.Delay(2, () =>
            {
                Camera c = Camera.main;
                GameManager.Instance.FadeCameraOut(1).Then(() =>
                {
                    GameManager.Instance.FadeCameraIn(1, c);
                    searchlight.transform.Rotate(0, 180, 0, Space.World);
                    searchlight.Restart();
                    CanFail = true;
                });
            });
        }
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
