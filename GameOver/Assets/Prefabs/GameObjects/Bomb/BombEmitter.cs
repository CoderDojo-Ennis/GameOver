using UnityEngine;

public class BombEmitter : MonoBehaviour
{
    [Header("The Bomb")]
    public GameObject BombPrefab;
    public float BombDrag = 2;

    [Header("Frequency")]
    public float BombIntervalSeconds = 3;
    public float BombIntervalRandomSeconds = 0.5f;

    // Don't drop bombs on collectables
    [Header("Where")]
    public bool AvoidLeft = false;
    public bool AvoidRight = false;
    public float AvoidPercent = .25f;
    public Vector3? NextDropPoint = null;

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
            Vector3 position;

            // Aiming for something specific?
            if (NextDropPoint != null)
            {
                position = NextDropPoint.Value;
                NextDropPoint = null;
            }
            else
            {
                // Pick a point on the quad based on the scale
                Vector3 center = this.transform.position;
                Vector3 scale = this.transform.localScale;
                float rangePct = 1;
                if (AvoidLeft)
                {
                    rangePct -= AvoidPercent;
                    center.SetX(center.x + (scale.x * AvoidPercent));
                }
                if (AvoidRight)
                {
                    rangePct -= AvoidPercent;
                    center.SetX(center.x - (scale.x * AvoidPercent));
                }
                scale *= rangePct;
                Vector3 scaleHalf = scale * .5f;
                position = new Vector3(
                    center.x - Random.Range(-scaleHalf.x, scaleHalf.x),
                    center.y - Random.Range(-scaleHalf.y, scaleHalf.y),
                    center.z - Random.Range(-scaleHalf.z, scaleHalf.z));
            }
            Quaternion rotation = Quaternion.identity;

            // Spawn
            GameObject bomb = Instantiate(BombPrefab, position, rotation);
            bomb.GetComponent<Rigidbody2D>().drag = BombDrag;

            // Arís!
            ScheduleNextBomb();
        }
    }

    /// <summary>
    ///  Whenever the next bomb drops - it should hit this
    /// </summary>
    /// <param name="nextTarget">What it should hit</param>
    internal void DropNextBombFrom(Vector3 nextDropPoint)
    {
        NextDropPoint = nextDropPoint;
    }
}
