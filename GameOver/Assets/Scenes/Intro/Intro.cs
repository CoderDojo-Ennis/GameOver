public class Intro : BaseGameScene
{
    // Use this for initialization
    new void Start()
    {
        // Don't call base. This one is unique
        // base.Start();

        GameManager.Instance.ActiveGameScene = this;

        StartIntro();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    /// <summary>
    /// Start the intro video, then transition to the war scene
    /// </summary>
    private void StartIntro()
    {
        string firstScene = "Instructions_WarScene";
        PlayerScript.Instance.ScoreVisible = false;
        PreloadScene(firstScene, false);
        PlayNextPlyalistVideo(VideoPlaylists.Intro).Then(() =>
        {
            ShowScene(firstScene);
        });
    }
}
