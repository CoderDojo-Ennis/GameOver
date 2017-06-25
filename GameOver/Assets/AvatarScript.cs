using UnityEngine;

public class AvatarScript : MonoBehaviour
{
    private SpriteRenderer SpriteRenderer;

    public void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public GmDelayPromise GlideX(float startX, float targetX, float seconds)
    {
        int steps = 100;
        float step = 0;

        transform.localPosition = new Vector3(startX, transform.localPosition.y, transform.localPosition.z);

        return this.Repeat(seconds / steps, steps, () =>
        {
            step++;
            var x = Mathf.Lerp(startX, targetX, step / steps);
            Debug.Log("X=" + x + " @ step " + step);
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
            transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
        }, true);
    }

    public void SetSprite(Sprite sprite)
    {
        SpriteRenderer.sprite = sprite;
    }
}
