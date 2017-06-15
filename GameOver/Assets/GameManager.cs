using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    /// <summary>
    /// Singleton Game Manager
    /// </summary>
    public static GameManager Instance;

    [Header("Video Playlists")]
    public GeekyMonkeyVideoPlaylist VideoPlaylist_Intro;
    public GeekyMonkeyVideoPlaylist VideoPlaylist_War;
    public GeekyMonkeyVideoPlaylist VideoPlaylist_Sea;
    public GeekyMonkeyVideoPlaylist VideoPlaylist_Land;

    internal bool IsVideoPlaying;

    [Header("Kinect")]
    public GameObject KinectController;
    public GameObject KinectCamera;
    private KinectManager KinectManager;
    private GameGestureListener GameGestureListener;

    // Preload / Show the next scene
    string PreloadedSceneName;
    AsyncOperation NextSceneAsync;

    internal BaseMenu ActiveMenu;

    /// <summary>
    /// The active BaseScene
    /// </summary>
    internal BaseGameScene ActiveGameScene;

    /// <summary>
    /// Is the game paused
    /// </summary>
    internal bool Paused;

    /// <summary>
    /// Seen Awake (before start)
    /// </summary>
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

    /// <summary>
    /// Scene Started (after all objects awake)
    /// </summary>
    void Start() {

        Debug.Log("Game manager start");

        HideMenu();

        GetComponentInChildren<EventSystem>().enabled = true;

        // Start Kinect
        KinectController.SetActive(true);
        KinectCamera = GameObject.Find("KinectCamera");
        KinectManager = KinectController.GetComponent<KinectManager>();
        GameGestureListener = KinectController.GetComponent<GameGestureListener>();
        GameGestureListener.OnUserDetected += GameGestureListener_OnUserDetected;
        GameGestureListener.OnUserLost += GameGestureListener_OnUserLost;
        GameGestureListener.OnSwipeLeft += GameGestureListener_SwipeHorizontal;
    }

    /// <summary>
    /// Kinect user detected
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void GameGestureListener_OnUserDetected(object sender, System.EventArgs e)
    {
        //todo - show user resume menu
        Debug.Log("User Detected");
        PauseGame();
    }

    /// <summary>
    /// Kinect user lost
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void GameGestureListener_OnUserLost(object sender, System.EventArgs e)
    {
        Debug.Log("User Lost");
        InviteGame();
    }

    /// <summary>
    /// Player has swiped left or right
    /// </summary>
    private void GameGestureListener_SwipeHorizontal(object sender, System.EventArgs e)
    {
        // Abort any playing video
        if (GeekyMonkeyVideoDirector.Instance.IsPlaying)
        {
            GeekyMonkeyVideoDirector.Instance.Abort();
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update() {
        InputProcessKeyboard();
        InputProcessGamepad();
        InputProcessKinect();
    }

    /// <summary>
    /// Process inpt events for keyboard
    /// </summary>
    private void InputProcessKeyboard()
    {
        // Pause/Resume
        if (Input.GetKeyUp(KeyCode.P))
        {
            if (Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        // Simulate kinect events
        // Player IN
        if (Input.GetKeyUp(KeyCode.I))
        {
            this.GameGestureListener_OnUserDetected(null, null);
        }
        // Player OUT
        if (Input.GetKeyUp(KeyCode.O))
        {
            this.GameGestureListener_OnUserLost(null, null);
        }
    }

    /// <summary>
    /// Process input events for gamepad
    /// </summary>
    private void InputProcessGamepad()
    {
        //todo
    }

    /// <summary>
    /// Process input events for kinect
    /// </summary>
    private void InputProcessKinect()
    {
        if (!KinectManager || !GameGestureListener || !GameGestureListener.IsPlayerDetected)
        {
            Debug.Log("No kinect");
            return;
        }

        Debug.Log("todo: Check kinect");
    }

    /// <summary>
    /// Invite the player
    /// </summary>
    public void InviteGame()
    {
        Paused = true;
        Time.timeScale = 0;
        ShowMenu("Invite");
        if (ActiveGameScene)
        {
            ActiveGameScene.OnPause();
        }
    }

    /// <summary>
    /// Pause the game
    /// </summary>
    public void PauseGame()
    {
        Paused = true;
        Time.timeScale = 0;
        ShowMenu("Pause");
        if (ActiveGameScene)
        {
            ActiveGameScene.OnPause();
        }
    }

    /// <summary>
    /// Resume the paused game
    /// </summary>
    public void ResumeGame()
    {
        Paused = false;
        Time.timeScale = 1;
        HideMenu();
        if (ActiveGameScene)
        {
            ActiveGameScene.OnResume();
        }
    }

    public void HideMenu(bool deactivate = true)
    {
        var menus = GameObject.Find("Menus");
        foreach (var menuTransform in menus.GetComponentsInChildren<Transform>(true))
        {
            // Only immediate children
            if (menuTransform.parent == menus.transform && menuTransform.gameObject.activeInHierarchy)
            {
                Debug.Log("Deactivate " + menuTransform.gameObject.name);
                menuTransform.gameObject.SetActive(false);
            }
        }
        if (deactivate)
        {
            ActiveMenu = null;
        }
    }

    public void ShowMenu(string menuName)
    {
        HideMenu(true);
        var menus = GameObject.Find("Menus");
        BaseMenu baseMenu = null;
        foreach (var menuTransform in menus.GetComponentsInChildren<Transform>(true))
        {
            // Only immediate children
            if (menuTransform.parent == menus.transform)
            {
                if (menuTransform.gameObject.name == menuName)
                {
                    baseMenu = menuTransform.gameObject.GetComponent<BaseMenu>();
                }
            }
        }

        if (baseMenu)
        {
            ActiveMenu = baseMenu;
            if (!IsVideoPlaying)
            {
                baseMenu.gameObject.SetActive(true);
                baseMenu.ShowMenu();
            }
        }
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

    /// <summary>
    /// Play the next video for the given playlist
    /// </summary>
    /// <param name="Playlist">The playlist to choose from</param>
    /// <returns>Promise for video completion</returns>
    public GmDelayPromise PlayNextPlyalistVideo(VideoPlaylists Playlist)
    {
        // Disable everything but the video
        HideKinect();
        HideMenu(false);

        // Start intro video
        GeekyMonkeyVideoPlaylist playlist;
        switch (Playlist)
        {
            case VideoPlaylists.Intro:
            default:
                playlist = VideoPlaylist_Intro;
                break;
            case VideoPlaylists.War:
                playlist = VideoPlaylist_War;
                break;
            case VideoPlaylists.Sea:
                playlist = VideoPlaylist_Sea;
                break;
            case VideoPlaylists.Land:
                playlist = VideoPlaylist_Land;
                break;
        }

        IsVideoPlaying = true;
        var videoDone = playlist.PlayNext();
        videoDone.Then(() => {
            IsVideoPlaying = false;
            // If a menu needs to be shown when the video is done
            if (ActiveMenu)
            {
                ShowMenu(ActiveMenu.gameObject.name);
            }
        });
        return videoDone;
    }

    /// <summary>
    /// Show the kinect overlay
    /// </summary>
    public void ShowKinect()
    {
        if (KinectCamera)
        {
            KinectCamera.SetActive(true);
        }
    }

    /// <summary>
    /// Hide the kinect overlay
    /// </summary>
    public void HideKinect()
    {
        if (KinectCamera)
        {
            KinectCamera.SetActive(false);
        }
    }
}
