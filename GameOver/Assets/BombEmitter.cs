using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEmitter : MonoBehaviour {

    public GameObject BombPrefab;
    public float BombIntervalSeconds = 3;
    public float BombIntervalRandomSeconds = 0.5f;

    private float NextBombSeconds;

	void Start () {
    }

    public void StartBombing()
    {
        Debug.Log("Bomb emitter start");
        ScheduleNextBomb();
    }

    private void ScheduleNextBomb()
    {
        float delay = BombIntervalSeconds + Random.Range(-BombIntervalRandomSeconds, BombIntervalRandomSeconds);
        Debug.Log("Next bomb in " + delay);
        this.Delay(delay, SpawnBomb);
    }

    private void SpawnBomb()
    {
        Debug.Log("Drop Bomb");
        Vector3 position = this.transform.position; // todo - extents
        Quaternion rotation = Quaternion.identity;
        GameObject.Instantiate(BombPrefab, position, rotation);

        ScheduleNextBomb();
    }
}
