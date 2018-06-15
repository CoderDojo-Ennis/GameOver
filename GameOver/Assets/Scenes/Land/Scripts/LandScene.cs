using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandScene : BaseGameScene
{
    public static LandScene instance;
    public CutterScript BoltCutters;
    public BoneScript Bone;
    public float DelayBeforeCutters;
    public float DelayBeforeBone;
    public GameObject CutterInfo;
    public Searchlight searchlight;
    public GameObject Phase2Objects;
    private AudioSource LoseSound;
    private bool CanFail = true;
    private int CurrentPhase; //Phase 1 = searchlight      Phase 2 = dog and guard

	new void Start ()
    {
        base.Start();
        instance = this;
        Invoke("DropCutters", DelayBeforeCutters);
        CutterInfo.SetActive(false);
        LoseSound = GetComponent<AudioSource>();
        CurrentPhase = 1;
        //Phase2Objects.SetActive(false);
	}
	
    void DropCutters()
    {
        BoltCutters.Fall();
    }

    void DropBone()
    {
        Bone.Fall();
    }

    public void CuttersCollected()
    {
        CutterInfo.SetActive(true);
        if (CurrentPhase == 2)
        {
            Invoke("DropBone", DelayBeforeBone);
        }
    }

    public void BoneCollected()
    {
    }

    public void Win()
    {
        if (CanFail)   //if the player is in the process of losing, don't win
        {
            CanFail = false;
            CurrentPhase++;
            if (CurrentPhase == 3)
            {
                // todo-winning scene
            }
            else  //start phase 2
            {
                GameManager.Instance.FadeCameraOut(1).Then(() =>
                {
                    GameManager.Instance.FadeCameraIn(1, Camera.main);
                    searchlight.gameObject.SetActive(false);
                    Phase2Objects.SetActive(true);
                    BoltCutters.Restart();
                    Invoke("DropCutters", DelayBeforeCutters);
                    CanFail = true;
                    try
                    {
                        CutterInfo.GetComponent<FenceCut>().ProgressSlider.value = CutterInfo.GetComponent<FenceCut>().ProgressSlider.maxValue;
                    }
                    catch { }
                    CutterInfo.SetActive(false);
                });
            }
        }
    }

    public void Fail()
    {
        if (CanFail)
        {
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
                    BoltCutters.Restart();
                    Invoke("DropCutters", DelayBeforeCutters);
                    CanFail = true;
                    try
                    {
                        CutterInfo.GetComponent<FenceCut>().ProgressSlider.value = CutterInfo.GetComponent<FenceCut>().ProgressSlider.maxValue;
                    } catch { }
                    CutterInfo.SetActive(false);
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
