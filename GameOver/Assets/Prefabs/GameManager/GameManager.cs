using TMPro;
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

    [Header("Sound")]
    public float MusicTimeScale = .05f;

    private AudioSource BackgroundMusicSource;
    private GmDelayPromise PlayerReadyPromise;

    [Header("Timer")]
    public TextMeshProUGUI Timer;
    public int TimerValue;
    public int PulseTime;
    internal int MidTimerEventTime;
    private AudioSource TimerPulseSound;
    public delegate void TimerEndedHandler();
    public event TimerEndedHandler TimerEnded;
    public delegate void TimerEventHandler();
    public event TimerEventHandler TimerEvent;
    private GmDelayPromise TimerPromise;
    [Header("ScoreCanvas")]
    public GameObject ScoreCanvas;

    [Header("Quality")]
    public int TargetFrameRate = 25;

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
    public bool Paused;

    /// <summary>
    /// Seen Awake (before start)
    /// </summary>
    private void Awake()
    {
        // Singleton that survives scene changes
        if (Instance != null)
        {
            //Debug.Log("Game manager 2nd instance abort");
            Destroy(gameObject);
            return;
        }

        //Debug.Log("Game manager awake");

        Instance = this;
        BackgroundMusicSource = GameObject.Find("BackgroundMusicSource").GetComponent<AudioSource>();
        TimerPulseSound = GetComponent<AudioSource>();
        GameGestureListener = KinectController.GetComponent<GameGestureListener>();
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Scene Started (after all objects awake)
    /// </summary>
    void Start()
    {
        //Debug.Log("Game manager start");

        HideMenu();
        FadeCameraOut(0);
        Cursor.visible = false;

        // Warm up
        GetComponentInChildren<EventSystem>().enabled = true;
        Timer.text = "";

        // Sync game engine with videos
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = TargetFrameRate;
        //Application.targetFrameRate = (int)GetComponent<UnityEngine.Video.VideoPlayer>().frameRate;

        // Start Kinect
        KinectController.SetActive(true);
        gameObject.GetComponentInChildren<KinectManager>(true).enabled = true;
        GameGestureListener = KinectController.GetComponent<GameGestureListener>();

        GameGestureListener.OnUserDetected += GameGestureListener_OnUserDetected;
        GameGestureListener.OnUserLost += GameGestureListener_OnUserLost;
        GameGestureListener.OnSwipeLeft += GameGestureListener_SwipeHorizontal;
    }

    internal void SetSoundTimeScale()
    {
        float newPitch = 1 - ((1 - Time.timeScale) * MusicTimeScale);
        this.BackgroundMusicSource.FadePitch(this, this.BackgroundMusicSource.pitch, newPitch, 0.5f, true);
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
            GameManager.Instance.SetTimeScale(0);
            ShowMenu("Pause", 0);
        }
        else
        {
            //PauseGame();
        }
    }

    /// <summary>
    /// Kinect user lost
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void GameGestureListener_OnUserLost(object sender, System.EventArgs e)
    {
        //Debug.Log("User Lost");
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
    public GmDelayPromise InviteGame()
    {
        Debug.Log("InviteGame");
        Paused = true;
        GameManager.Instance.SetTimeScale(0);
        if (ActiveGameScene)
        {
            ActiveGameScene.OnPause();
        }
        ShowMenu("Invite", 1);
        PauseBackroundMusic();
        PlayerReadyPromise = new GmDelayPromise();
        return PlayerReadyPromise;
    }

    /// <summary>
    /// Pause the game
    /// </summary>
    public void PauseGame()
    {
        if (!Paused)
        {
            if (ActiveGameScene)
            {
                ActiveGameScene.OnPause();
            }
            Paused = true;
        }
        Debug.Log("PauseGame");
        GameManager.Instance.SetTimeScale(0);
        ShowMenu("Pause", 1);
        PauseBackroundMusic();
    }

    /// <summary>
    /// Resume the paused game
    /// </summary>
    public void ResumeGame()
    {
        Debug.Log("ResumeGame");
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
            float timeScale = 0;
            this.Repeat(0.2f, 10, () =>
            {
                timeScale += .1f;
                Time.timeScale = timeScale;
            }, true);
        }, true);

        if (PlayerReadyPromise != null)
        {
            PlayerReadyPromise.Done();
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
                //Debug.Log("Deactivate " + menuTransform.gameObject.name);
                menuTransform.gameObject.SetActive(false);
            }
        }
        if (deactivate)
        {
            ActiveMenu = null;
        }
    }

    public void SetTimeScale(float newTimeScale)
    {
        Time.timeScale = newTimeScale;
        Debug.Log("TimeScale=" + newTimeScale);
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
        /*
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
        */
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
            //if (PreloadedSceneName == sceneName)
            //{
            //    Debug.Log("Showing After Preloading " + sceneName);
            //    NextSceneAsync.allowSceneActivation = true;
            //}
            //else
            //{
            //Debug.Log("Showing Non Preloaded  " + sceneName);
            //NextSceneAsync = SceneManager.LoadSceneAsync(sceneName);
            //NextSceneAsync.allowSceneActivation = true;
            //}
            this.Delay(0.5f, () =>
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            });
        });
    }

    /// <summary>
    /// Fade camera out and switch to another scene
    /// </summary>
    /// <param name="sceneName">Next scene name</param>
    /// <param name="fadeSeconds">Fade seconds</param>
    public void FadeToScene(string sceneName, float fadeSeconds)
    {
        PauseBackroundMusic();
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
    public void PauseBackroundMusic(float fadeSeconds = 0.5f)
    {
        //Debug.Log("Pause background music");
        if (BackgroundMusicSource.clip != null)
        {
            BackgroundMusicSource.Fade(this, BackgroundMusicSource.volume, 0, fadeSeconds, true);
            //BackgroundMusicSource.pitch = 1;
            //this.Repeat(.2f, 10, () =>
            //{
            //    BackgroundMusicSource.pitch -= .01f;
            //}, true);
            this.Delay(fadeSeconds, () =>
            {
                //Debug.Log("Pause background music now");
                BackgroundMusicSource.Pause();
            }, true);
        }
    }

    /// <summary>
    /// Fade out and stop the background music
    /// </summary>
    public void ResumeBackroundMusic()
    {
        //Debug.Log("Resume background music");
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
        PlayerScript.Instance.transform.position = Vector3.zero;
        PlayerScript.Instance.transform.rotation = Quaternion.identity;
        PlayerScript.Instance.ShowKinect(1);
    }

    /// <summary>
    /// Hide the kinect overlay
    /// </summary>
    public void HideKinect()
    {
        PlayerScript.Instance.HideKinect(1);
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
        //Debug.Log("ShowCamera " + camera.name);
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
            if (camera != exceptCamera)
            {
                if (camera.enabled)
                {
                    //Debug.Log("Disble camera: " + camera.name);
                    camera.enabled = false;
                }
            }
        }
    }

    public void HideTimer()
    {
        Timer.text = "";
    }

    public void StopTimer()
    {
        HideTimer();
        if (TimerPromise != null)
        {
            TimerPromise.Abort();
        }
    }

    public void StartTimer(int Duration, int EventTime)
    {
        TimerValue = Duration;
        Timer.text = TimerValue.ToString();
        MidTimerEventTime = EventTime;
        TimerPromise = this.Delay(1, TimerTick);
    }

    void TimerTick()
    {
        TimerValue -= 1;
        Timer.text = TimerValue.ToString();
        if (TimerValue == MidTimerEventTime)
        {
            TimerEvent();
        }
        if (TimerValue <= PulseTime)
        {
            TimerPulseSound.PlayOneShot(TimerPulseSound.clip);
            // Scale to designed size over one second
            float scale2 = 1.6f;
            float scale = scale2;
            int scaleSteps = 4;
            this.Timer.rectTransform.localScale = new Vector3(scale, scale);
            this.Repeat(1 / (scaleSteps + 1), scaleSteps, () =>
            {
                // Aborted
                //if (SecondsRemaining < 0)
                //{
                //    this.Timer.text = "";
                //    return;
                //}
                scale -= (scale2 - 1) / (float)scaleSteps;
                this.Timer.rectTransform.localScale = new Vector3(scale, scale);
            });
        }
        if (TimerValue > 0)
        {
            TimerPromise = this.Delay(1, TimerTick);
        }
        else
        {
            TimerEnded();
            Timer.enabled = false;
        }
    }

    public void DisableScoreCanvas()
    {
        ScoreCanvas.SetActive(false);
    }

    public void EnableScoreCanvas()
    {
        ScoreCanvas.SetActive(true);
    }
}
