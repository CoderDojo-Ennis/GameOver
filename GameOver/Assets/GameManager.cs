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

    [Header("Kinect")]
    public GameObject KinectController;

    GameObject SceneActive;

    GameObject Menu;
    GameObject GameCamera;
    GameObject KinectCamera;

    // Preload / Show the next scene
    string PreloadedSceneName;
    AsyncOperation NextSceneAsync;

    private void Awake()
    {

        // Singleton that survives scene changes
        if (Instance != null)
        {
            Debug.Log("Game manager 2nd instance abort");
            Destroy(gameObject);
            return;
        }

        Debug.Log("Game manager awake");

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start() {

        Debug.Log("Game manager start");

        // Gather the stuff
        //videoDirector = GeekyMonkeyVideoDirector.Instance;

        // Start Kinect
        KinectController.SetActive(true);

        if (SceneManager.GetActiveScene().name == "Complete")
        {
            StartIntro();
        }
    }

    private void StartIntro()
    {
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
            ShowScene("WarScene");
        });

        // Allow for video fade-in, then load the next scene in the background
        this.Delay(1, () =>
        {
            GameCamera.SetActive(true);
            PreloadScene("WarScene", false);
            //SceneManager.LoadScene("WarScene");
        });
    }

    // Update is called once per frame
    void Update() {

    }

    /// <summary>
    /// Pre-Load the next scene
    /// </summary>
    /// <param name="sceneName">Name of the scene</param>
    /// <param name="showImmediately">Show immediately, or else it will be shown when ShowScene is called</param>
    public void PreloadScene(string sceneName, bool showImmediately)
    {
        if (PreloadedSceneName != sceneName)
        {
            Debug.Log("Preloading " + sceneName + " Show Immediately = " + showImmediately);
            NextSceneAsync = SceneManager.LoadSceneAsync(sceneName);
            NextSceneAsync.allowSceneActivation = showImmediately;
            if (!showImmediately)
            {
                PreloadedSceneName = sceneName;
            }
        }
    }

    /// <summary>
    /// Show the scene now. Will be faster if it's already pre-loaded
    /// </summary>
    /// <param name="sceneName">Name of the scene</param>
    public void ShowScene(string sceneName)
    {
        // Is it pre-loaded
        if (PreloadedSceneName == sceneName)
        {
            Debug.Log("Showing After Preloading " + sceneName);
            NextSceneAsync.allowSceneActivation = true;
        } else
        {
            Debug.Log("Showing Non Preloaded  " + sceneName);
            NextSceneAsync = SceneManager.LoadSceneAsync(sceneName);
            NextSceneAsync.allowSceneActivation = true;
        }
    }


    /*
    public void ShowNextScene()
    {
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
    */
}
