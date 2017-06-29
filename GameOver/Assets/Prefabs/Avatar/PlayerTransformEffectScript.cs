using UnityEngine;

public class PlayerTransformEffectScript : MonoBehaviour
{
    private float age = 0f;
    public float lifetimeSeconds = 2.2f;
    private Vector3 StartPosition;

    // Use this for initialization
    void Start()
    {
        StartPosition = this.transform.position;
        age = 0;
    }

    private void Update()
    {
        age += Time.deltaTime;
        float agePct = age / lifetimeSeconds;
        if (agePct > 1)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            transform.localScale = Vector3.one * Mathf.Sin(Mathf.Deg2Rad * agePct * 180);
        }
        transform.position = this.StartPosition;
    }
}
