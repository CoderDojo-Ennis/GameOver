using UnityEngine;

public class SeaScene : BaseGameScene
{
    [Header("Objects")]
    public SeaWaves sea;
    public WaveFollow enemy;

    [Header("Variables")]
    public int TimerDuration = 30;
    public float StartingWaveStrength;
    public float EndingWaveStrength;
    public int EnemyIntroductionTime;
    public float EnemyFightingXPosition;
    public float EnemyStartingXPosition;
    private float EnemyTargetPos;
    public int EnemyLerpSpeed;


    public override void FirstUpdate()
    {
        base.FirstUpdate();

        EnemyTargetPos = EnemyStartingXPosition;
        GameManager.Instance.StartTimer(TimerDuration, EnemyIntroductionTime);
        GameManager.Instance.TimerEnded += TimerEnded;
        GameManager.Instance.TimerEvent += EnemyIntro;

        PlayerScript.Instance.ShowKinect(0);
    }

    /// <summary>
    /// Un-hook any events
    /// </summary>
    internal new void OnDestroy()
    {
        base.OnDestroy();
        GameManager.Instance.TimerEnded -= TimerEnded;
        GameManager.Instance.TimerEvent -= EnemyIntro;
    }

    new void Update()
    {
        base.Update();

        if (!Paused)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                FadeToScene("LandScene");
                //var warScene = GameManager.Instance.War;
                //GameManager.Instance.ShowScene(warScene);
            }
        }

        if (GameManager.Instance.TimerValue <= 3)
        {
            EnemyTargetPos = EnemyStartingXPosition;
        }

        //Use this for waves that get bigger up until the enemy arrives
        //sea.scale = Mathf.Lerp(EndingWaveStrength, StartingWaveStrength, ((float)GameManager.Instance.TimerValue - EnemyIntroductionTime) / ((float)TimerDuration - EnemyIntroductionTime));
        sea.scale = Mathf.Lerp(EndingWaveStrength, StartingWaveStrength, (float)GameManager.Instance.TimerValue / TimerDuration);
        if (enemy.RaftPosition.x < EnemyTargetPos - 0.5)
        {
            enemy.RaftPosition.x += Time.deltaTime * EnemyLerpSpeed;
        }
        else if (enemy.RaftPosition.x > EnemyTargetPos + 0.5)
        {
            enemy.RaftPosition.x -= Time.deltaTime * EnemyLerpSpeed;
        }
        //enemy.RaftPosition.x = Mathf.Lerp(EnemyFightingXPosition, EnemyStartingXPosition, (float)((GameManager.Instance.TimerValue + EnemyLerpSeconds) - EnemyIntroductionTime) / EnemyLerpSeconds);
    }

    void TimerEnded()
    {
        GameManager.Instance.HideTimer();
        FadeToScene("AfterSeaScene");
    }

    void EnemyIntro()
    {
        enemy.gameObject.SetActive(true);
        EnemyTargetPos = EnemyFightingXPosition;
    }
}
