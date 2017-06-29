using UnityEngine;

public class WarInstructionsAnimation : MonoBehaviour
{
    [Header("Bombs")]
    public Transform[] BombSpawnPoints;
    public GameObject BombPrefab;
    public float BombStartDelay = 1f;
    public float BombInterval = 0.5f;

    // Use this for initialization
    void Start()
    {
        this.Delay(BombStartDelay - BombInterval, () =>
        {
            int bombIndex = 0;
            this.Repeat(BombInterval, BombSpawnPoints.Length, () =>
            {
                var spawnPoint = BombSpawnPoints[bombIndex];
                var bomb = GameObject.Instantiate(BombPrefab, spawnPoint);
                bomb.layer = spawnPoint.gameObject.layer;
                bombIndex++;
            });
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
