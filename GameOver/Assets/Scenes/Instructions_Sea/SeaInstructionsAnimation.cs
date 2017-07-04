using UnityEngine;

public class SeaInstructionsAnimation : MonoBehaviour
{
    [Header("Bombs")]
    public float SceneStartDelay = 1f;

    // Use this for initialization
    void Start()
    {
        // Full health
        PlayerScript.Instance.SetInitialHealth();

        // Start animation
        this.Delay(SceneStartDelay, () =>
        {
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
