using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AuraAPI;

public class LandScene : BaseGameScene
{
    public static LandScene instance;
    public CutterScript BoltCutters;
    public BoneScript Bone;
    public float DelayBeforeCutters;
    public float DelayBeforeBone;
    public GameObject CutterInfo;
    public GameObject searchlight;
    public GameObject Phase2Objects;
    private AudioSource LoseSound;
    private bool CanFail = true;
    private int CurrentPhase; //Phase 1 = searchlight      Phase 2 = dog and guard
    private string WhereBoneDropped;

    public static List<T> FindObjectsOfTypeAll<T>() //useful extension method
    {
        List<T> results = new List<T>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var s = SceneManager.GetSceneAt(i);
            if (s.isLoaded)
            {
                var allGameObjects = s.GetRootGameObjects();
                for (int j = 0; j < allGameObjects.Length; j++)
                {
                    var go = allGameObjects[j];
                    results.AddRange(go.GetComponentsInChildren<T>(true));
                }
            }
        }
        return results;
    }

    new void Start ()
    {
        base.Start();
        instance = this;
        Invoke("DropCutters", DelayBeforeCutters);
        CutterInfo.SetActive(false);
        Phase2Objects.SetActive(false);
        LoseSound = GetComponent<AudioSource>();
        CurrentPhase = 1;
        //Phase2Objects.SetActive(false);
        foreach(AuraLight light in FindObjectsOfTypeAll<AuraLight>())
        {
            light.enabled = true; //sometimes the lights just randomly disable for some reason
        }
	}
	
    void DropCutters()
    {
        BoltCutters.Fall();
    }

    void DropBone()
    {
        WhereBoneDropped = Bone.Fall();
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
        this.Delay(0.5f, () => { Phase2Objects.GetComponentInChildren<GuardScript>().SawSomething(WhereBoneDropped); });
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
                    searchlight.SetActive(false);
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
            CanFail = false;
            LoseSound.Play();
            PlayerScript.Instance.Damage(10, false, false);
            this.Delay(2, () =>
            {
                GameManager.Instance.FadeToScene(SceneManager.GetActiveScene().name, 1);
                /*
                Camera c = Camera.main;
                GameManager.Instance.FadeCameraOut(1).Then(() =>
                {
                    GameManager.Instance.FadeCameraIn(1, c);
                    searchlight.transform.Rotate(0, 180, 0, Space.World);
                    searchlight.Restart();
                    BoltCutters.Restart();
                    Bone.Restart();
                    Invoke("DropCutters", DelayBeforeCutters);
                    Phase2Objects.SetActive(false);
                    CanFail = true;
                    try
                    {
                        CutterInfo.GetComponent<FenceCut>().ProgressSlider.value = CutterInfo.GetComponent<FenceCut>().ProgressSlider.maxValue;
                    } catch { }
                    CutterInfo.SetActive(false);
                });
                */
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
