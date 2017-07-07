using UnityEngine;

public class LandInstructionsAnimation : MonoBehaviour
{
    public float SceneStartDelay = 1f;
    public SpriteRenderer LightColumn;

    // Use this for initialization
    void Start()
    {
        // Full health
        PlayerScript.Instance.SetInitialHealth();

        // Start animation
        this.Delay(SceneStartDelay, () =>
        {
            LightColumn.FadeAlpha(this, LightColumn.color.a, 0, 2f, false);
        });
    }
}
