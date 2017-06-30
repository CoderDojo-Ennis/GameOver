﻿using UnityEngine;

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

    new void Start()
    {
        base.Start();
        EnemyTargetPos = EnemyStartingXPosition;
        GameManager.Instance.StartTimer(TimerDuration, EnemyIntroductionTime);
        GameManager.Instance.TimerEnded += TimerEnded;
        GameManager.Instance.TimerEvent += EnemyIntro;
    }

    new void Update()
    {
        base.Update();

        if (!Paused)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                FadeToScene("IntroScene");
                //var warScene = GameManager.Instance.War;
                //GameManager.Instance.ShowScene(warScene);
            }
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
        Debug.Log("End");
    }

    void EnemyIntro()
    {
        enemy.gameObject.SetActive(true);
        EnemyTargetPos = EnemyFightingXPosition;
    }
}
