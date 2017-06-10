using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    [Header("Video Playlists")]
    public GeekyMonkeyVideoPlaylist VideoPlaylist_Intro;
    public GeekyMonkeyVideoPlaylist VideoPlaylist_War;
    public GeekyMonkeyVideoPlaylist VideoPlaylist_Sea;
    public GeekyMonkeyVideoPlaylist VideoPlaylist_Land;

    [Header("Game Scenes")]
    public GameObject War;
    public GameObject Sea;
    GameObject SceneActive;

    //GeekyMonkeyVideoDirector videoDirector;

    GameObject Menu;
    GameObject GameCamera;
    GameObject KinectCamera;
    //GameObject VideoCamera;


	// Use this for initialization
	void Start () {

        // Singleton that survives scene changes
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Gather the stuff
        //videoDirector = GeekyMonkeyVideoDirector.Instance;

        // Disable everything but the intro video
        GameCamera = GameObject.Find("GameCamera");
        GameCamera.SetActive(false);
        KinectCamera = GameObject.Find("KinectCamera");
        KinectCamera.SetActive(false);

        // Start intro video
        //VideoCamera = GameObject.Find("VideoCamera");
        VideoPlaylist_Intro.PlayNext().Then(() =>
        {
            KinectCamera.SetActive(true);
            ShowScene(War);
        });

        // Allow for video fade-in, then load the next scene in the background
        this.Delay(3, () =>
        {
            GameCamera.SetActive(true);
            //SceneManager.LoadScene("WarScene");
        });
    }

    // Update is called once per frame
    void Update () {
		
	}

    // Show a specific game scene
    public void ShowScene(GameObject scene)
    {
        GameCamera.SetActive(false);
        if (SceneActive != null)
        {
            // Fade out the current scene
            SceneActive.GetComponent<BaseGameScene>().FadeOut().Then(() => {
                SceneActive = scene;
                SceneActive.SetActive(true);
                SceneActive.GetComponent<BaseGameScene>().FadeIn();
            });
        } else
        {
            SceneActive = scene;
            SceneActive.SetActive(true);
            SceneActive.GetComponent<BaseGameScene>().FadeIn();
        }

    }
}
