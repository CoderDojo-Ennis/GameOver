using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct DifficultySetting
{
    [Header("War")]
    public float BombDrag; //Speed of the bombs decreases when this increases. Higher is easier.
    public float BombInterval; //Time interval between each bomb. Higher is easier.
    public float CollectableLifeSeconds; //Time after a collectable is spawned before a bomb is dropped on it. Higher is easier.
    [Header("Sea")]
    public float StartingWaveSize; //Size of the waves at the start. Lower is easier.
    public float EndingWaveSize; //Size of the waves by the end. Lower is easier.
    public float ShootDelay; //Delay between enemy's shots. Higher is easier.
    [Header("Land")]
    public int SearchlightTime; //the amount of frames you must be in the light to lose. Higher is easier.
    public int GuardTime; //the amount of frames you must be in front of the guard to lose. Higher is easier.
    public float GuardSpeed; //the speed at which the guard walks. Higher is easier? Maybe. Not sure.
    public float SearchlightSpeed; //the speed at which the searchlight rotates. Higher is easier? Again, maybe.
    public float CutFenceSpeed; //the speed that the player can cut the fence. Higher is easier.
}

public class DifficultyManager : MonoBehaviour
{
    [Header("Text")]
    public Color[] DifficultyTextColours;
    public float TextScreenTime; //How long the text stays on screen after changing it
    private Text DiffText;

    [Header("Difficulty")]
    public int HealthPerPress = 10;
    public DifficultySetting[] difficultySettings;
    private int currentDifficulty = 1; //0 is easy, 1 is normal, 2 is hard

    private GmDelayPromise CurrentTextDisappearDelay;

	void OnEnable ()
    {
        DiffText = GetComponentInChildren<Text>();
		if (difficultySettings.Length != 4 || DifficultyTextColours.Length != 4)
        {
            Debug.LogError("There are " + difficultySettings.Length + "difficulty settings / colours! There should only be 4!");
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
        ShowText();
	}

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update ()
    {
		if (Input.GetButtonDown("DifficultyUp") && currentDifficulty < 3)
        {
            currentDifficulty++;
            SetDifficulty();
        }
        if (Input.GetButtonDown("DifficultyDown") && currentDifficulty > 0)
        {
            currentDifficulty--;
            SetDifficulty();
        }
        if (Input.GetButtonDown("ExtraLife"))
        {
            PlayerScript.Instance.GiveHealth(HealthPerPress);
        }
        if (Input.GetButtonDown("ShowDiffText"))
        {
            ShowText();
        }
	}

    private void SetDifficulty()
    {
        DifficultySetting difficulty = difficultySettings[currentDifficulty];
        switch (SceneManager.GetActiveScene().name)
        {
            case "WarScene":
                FindObjectOfType<WarScene>().CollectableLifeSeconds = difficulty.CollectableLifeSeconds;
                BombEmitter bombEmitter = FindObjectOfType<BombEmitter>();
                bombEmitter.BombIntervalSeconds = difficulty.BombInterval;
                bombEmitter.BombDrag = difficulty.BombDrag;
                foreach (BombScript bomb in FindObjectsOfType<BombScript>())
                {
                    bomb.GetComponent<Rigidbody2D>().drag = difficulty.BombDrag;
                }
                break;
            case "SeaScene":
                SeaScene sea = FindObjectOfType<SeaScene>();
                sea.StartingWaveStrength = difficulty.StartingWaveSize;
                sea.EndingWaveStrength = difficulty.EndingWaveSize;
                LandScene.FindObjectsOfTypeAll<EnemyGun>()[0].ShootDelay = difficulty.ShootDelay;
                break;
            case "LandScene":
                Searchlight light = LandScene.FindObjectsOfTypeAll<Searchlight>()[0]; //Find it even if it is disabled
                light.SweepSpeed = difficulty.SearchlightSpeed;
                light.FramesVisibleForLoss = difficulty.SearchlightTime;
                GuardScript guard = LandScene.FindObjectsOfTypeAll<GuardScript>()[0];
                guard.FramesVisibleForLoss = difficulty.GuardTime;
                guard.WalkSpeed = difficulty.GuardSpeed;
                LandScene.FindObjectsOfTypeAll<FenceCut>()[0].CutSpeed = difficulty.CutFenceSpeed;
                break;
            default:
                Debug.Log("Can't change difficulty in this scene");
                break;
        }

        ShowText();
        DiffText.color = DifficultyTextColours[currentDifficulty];
        switch (currentDifficulty)
        {
            case 0:
                DiffText.text = "E";
                break;
            case 1:
                DiffText.text = "N";
                break;
            case 2:
                DiffText.text = "H";
                break;
            case 3:
                DiffText.text = "EX";
                break;
            default:
                Debug.LogError("...What just happened?");
                break;
        }
    }

    private void ShowText()
    {
        if (CurrentTextDisappearDelay != null)
        {
            CurrentTextDisappearDelay.Abort();
        }
        DiffText.gameObject.SetActive(true);
        CurrentTextDisappearDelay = this.Delay(TextScreenTime, () => { DiffText.gameObject.SetActive(false); });
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetDifficulty();
    }
}
