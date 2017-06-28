using UnityEngine;

public class PlayerTransformEffectScript : MonoBehaviour
{
    public float lifetimeSeconds = 2.2f;

    // Use this for initialization
    void Start()
    {
        this.Delay(lifetimeSeconds, () =>
        {
            this.gameObject.SetActive(false);
        });
    }
}
