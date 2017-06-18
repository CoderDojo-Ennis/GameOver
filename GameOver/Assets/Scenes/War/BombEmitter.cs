using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEmitter : MonoBehaviour {

    [Header("The Bomb")]
    public GameObject BombPrefab;

    [Header("Frequency")]
    public float BombIntervalSeconds = 3;
    public float BombIntervalRandomSeconds = 0.5f;

    private float NextBombSeconds;
    private bool IsBombing;

    /// <summary>
    /// Turn on the bombing emitter
    /// </summary>
    public void StartBombing()
    {
        if (!IsBombing)
        {
            IsBombing = true;
            ScheduleNextBomb();
        }
    }

    /// <summary>
    /// Stop the bombing emitter
    /// </summary>
    public void StopBombing()
    {
        IsBombing = false;
    }

    /// <summary>
    /// Schedule the next bomb to drop
    /// </summary>
    private void ScheduleNextBomb()
    {
        float delay = BombIntervalSeconds + Random.Range(-BombIntervalRandomSeconds, BombIntervalRandomSeconds);
        this.Delay(delay, SpawnBomb);
    }

    /// <summary>
    /// Spawn a bomb, and schedule the next
    /// </summary>
    private void SpawnBomb()
    {
        // Should we abort?
        if (IsBombing)
        {
            // Pick a point on the quad based on the scale
            Vector3 center = this.transform.position;
            Vector3 scale = this.transform.localScale;
            Vector3 scaleHalf = this.transform.localScale * .5f;
            Vector3 position = new Vector3(
                center.x - Random.Range(-scaleHalf.x, scaleHalf.x),
                center.y - Random.Range(-scaleHalf.y, scaleHalf.y),
                center.z - Random.Range(-scaleHalf.z, scaleHalf.z));
            Quaternion rotation = Quaternion.identity;
            GameObject.Instantiate(BombPrefab, position, rotation);

            ScheduleNextBomb();
        }
    }
}
