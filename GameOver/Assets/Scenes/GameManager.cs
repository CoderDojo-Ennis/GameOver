using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
    //private KinectManager KinectManager;
    [HideInInspector]
    public GameGestureListener GameGestureListener;

    [Header("Menus")]
    public GameObject Instructions;
    public SpriteRenderer CameraMask;

    // Preload / Show the next scene
    string PreloadedSceneName;
    AsyncOperation NextSceneAsync;
    public Camera ActiveCamera;

    internal BaseMenu ActiveMenu;
    internal BaseMenu ActiveInstructions;

    private AudioSource BackgroundMusicSource;

    private string ActiveCameraName
    {
        get
        {
            if (ActiveCamera == null)
            {
                return null;
            }
            return ActiveCamera.name;
        }
    }

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
        BackgroundMusicSource = GameObject.Find("BackgroundMusicSource").GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Scene Started (after all objects awake)
    /// </summary>
    void Start()
    {
        Debug.Log("Game manager start");

        HideMenu();

        GetComponentInChildren<EventSystem>().enabled = true;

        // Start Kinect
        KinectController.SetActive(true);
        KinectCamera = GameObject.Find("KinectCamera");
        //KinectManager = KinectController.GetComponent<KinectManager>();
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
        //todo - show user resume menu - move this to invite script
        if (Paused)
        {
            Debug.Log("User Detected");
            Time.timeScale = 0;
            ShowMenu("Pause", 0);
        }
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
    void Update()
    {
        InputProcessKeyboard();
        InputProcessGamepad();
        InputProcessKinect();
    }

    /// <summary>
    /// Process inpt events for keyboard
    /// </summary>
    private void InputProcessKeyboard()
    {
        /*
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
        */
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
        // todo?
        /*
        if (!KinectManager || !GameGestureListener || !GameGestureListener.IsPlayerDetected)
        {
            Debug.Log("No kinect");
            return;
        }

        Debug.Log("todo: Check kinect");
        */
    }

    /// <summary>
    /// Invite the player
    /// </summary>
    public void InviteGame()
    {
        Paused = true;
        Time.timeScale = 0;
        ShowMenu("Invite", 1);
        PauseBackroundMusic();
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
        ShowMenu("Pause", 1);
        PauseBackroundMusic();
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
        // todo - could fade out
        Paused = false;
        HideMenu();
        Time.timeScale = 0;
        ResumeBackroundMusic();
        if (ActiveGameScene)
        {
            ActiveGameScene.OnResume();
        }
        this.Delay(0.25f, () =>
        {
            this.Repeat(0.2f, 10, () =>
            {
                Time.timeScale += .1f;
            }, true);
        }, true);
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

    /// <summary>
    /// Show a specific menu
    /// </summary>
    /// <param name="menuName">Name of the menu to show</param>
    public void ShowMenu(string menuName, float fadeSeconds)
    {
        if (ActiveMenu)
        {
            // Are we already showing the requested menu?
            if (ActiveMenu.gameObject.name == menuName)
            {
                return;
            }

            // Hide other menu
            HideMenu(true);
        }

        // Find the menu to show
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

        // Found it?
        if (baseMenu)
        {
            ActiveMenu = baseMenu;
            if (!IsVideoPlaying)
            {
                baseMenu.gameObject.SetActive(true);
                baseMenu.ShowMenu(fadeSeconds);
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
        bool justWatchedVideo = ActiveCameraName == "VideoCamera";
        float fadeSeconds = justWatchedVideo ? 0.1f : 1f;
        FadeCameraOut(fadeSeconds).Then(() =>
        {
            // Is it pre-loaded
            if (PreloadedSceneName == sceneName)
            {
                Debug.Log("Showing After Preloading " + sceneName);
                NextSceneAsync.allowSceneActivation = true;
            }
            else
            {
                Debug.Log("Showing Non Preloaded  " + sceneName);
                NextSceneAsync = SceneManager.LoadSceneAsync(sceneName);
                NextSceneAsync.allowSceneActivation = true;
            }
        });
    }

    /// <summary>
    /// Fade camera out and switch to another scene
    /// </summary>
    /// <param name="sceneName">Next scene name</param>
    /// <param name="fadeSeconds">Fade seconds</param>
    public void FadeToScene(string sceneName, float fadeSeconds)
    {
        PreloadScene(sceneName, false);
        FadeCameraOut(fadeSeconds).Then(() =>
        {
            ShowScene(sceneName);
        });
    }

    /// <summary>
    /// Play some background music
    /// </summary>
    /// <param name="music">The music to play</param>
    public void PlayBackgroundMusic(AudioClip music)
    {
        BackgroundMusicSource.loop = true;
        BackgroundMusicSource.Stop();
        BackgroundMusicSource.clip = music;
        BackgroundMusicSource.volume = 1;
        BackgroundMusicSource.Play();
    }

    /// <summary>
    /// Fade out and stop the background music
    /// </summary>
    public void StopBackroundMusic()
    {
        float fadeSeconds = 1;
        BackgroundMusicSource.Fade(this, BackgroundMusicSource.volume, 0, fadeSeconds, true);
        this.Delay(fadeSeconds, () =>
        {
            BackgroundMusicSource.Stop();
        });
    }

    /// <summary>
    /// Fade out and stop the background music
    /// </summary>
    public void PauseBackroundMusic()
    {
        Debug.Log("Pause background music");
        if (BackgroundMusicSource.clip != null)
        {
            float fadeSeconds = 0.5f;
            BackgroundMusicSource.Fade(this, BackgroundMusicSource.volume, 0, fadeSeconds, true);
            //BackgroundMusicSource.pitch = 1;
            //this.Repeat(.2f, 10, () =>
            //{
            //    BackgroundMusicSource.pitch -= .01f;
            //}, true);
            this.Delay(fadeSeconds, () =>
            {
                Debug.Log("Pause background music now");
                BackgroundMusicSource.Pause();
            }, true);
        }
    }

    /// <summary>
    /// Fade out and stop the background music
    /// </summary>
    public void ResumeBackroundMusic()
    {
        Debug.Log("Resume background music");
        if (BackgroundMusicSource.clip != null)
        {
            float fadeSeconds = 0.5f;
            BackgroundMusicSource.UnPause();
            BackgroundMusicSource.Fade(this, BackgroundMusicSource.volume, 1, fadeSeconds, true);
            //BackgroundMusicSource.pitch = 0.9f;
            //this.Repeat(.2f, 10, () =>
            //{
            //    BackgroundMusicSource.pitch += .01f;
            //}, true);
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
        StopBackroundMusic();

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
        videoDone.Then(() =>
        {
            IsVideoPlaying = false;
            // If a menu needs to be shown when the video is done
            if (ActiveMenu)
            {
                ShowMenu(ActiveMenu.gameObject.name, 0.5f);
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

    public GmDelayPromise FadeCameraOut(float seconds)
    {
        return CameraMask.FadeAlpha(this, CameraMask.color.a, 1, seconds, true);
    }

    public GmDelayPromise FadeCameraIn(float seconds, Camera camera)
    {
        ShowCamera(camera);
        return CameraMask.FadeAlpha(this, 1, 0, seconds, true);
    }

    /// <summary>
    /// Fade the current camera out, switch to another camera and fade it in.  Promise when complete
    /// </summary>
    /// <param name="camera"></param>
    /// <returns></returns>
    public GmDelayPromise FadeToCamera(Camera camera, float fadeSeconds)
    {
        GmDelayPromise fadeComplete = new GmDelayPromise();

        // Already faded out?
        if (CameraMask.color.a == 0)
        {
            FadeCameraIn(fadeSeconds, camera).Then(() =>
            {
                fadeComplete.Done();
            });
        }
        else
        {
            FadeCameraOut(fadeSeconds)
                .Then(() =>
                {
                    FadeCameraIn(fadeSeconds, camera).Then(() =>
                    {
                        fadeComplete.Done();
                    });
                });
        }

        return fadeComplete;
    }

    public void ShowCamera(Camera camera)
    {
        Debug.Log("ShowCamera " + camera.name);
        if (camera != null)
        {
            HideAllCameras(camera);
            camera.enabled = true;
        }
        ActiveCamera = camera;
    }

    public void HideAllCameras(Camera exceptCamera)
    {
        var activeCameras = GameObject.FindObjectsOfType<Camera>();
        foreach (var camera in activeCameras)
        {
            Debug.Log("  camera " + camera.name);
            if (camera.name != "KinectCamera" && camera != exceptCamera)
            {
                if (camera.enabled)
                {
                    Debug.Log("Disble camera: " + camera.name);
                    camera.enabled = false;
                }
            }
        }
    }
}
