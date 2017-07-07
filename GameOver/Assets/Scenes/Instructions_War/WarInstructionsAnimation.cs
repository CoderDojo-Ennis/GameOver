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
        // Full health + Nothing collected
        PlayerScript.Instance.SetInitialHealth();
        ResetCollectables();

        // Start animation
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

    void ResetCollectables()
    {
        foreach (var collectableName in new string[] { "Coin", "Passport", "Suitcase" })
        {
            string PlayerPrefKey = "Collectable_" + collectableName;
            PlayerPrefs.SetInt(PlayerPrefKey, 0);
        }
    }
}
