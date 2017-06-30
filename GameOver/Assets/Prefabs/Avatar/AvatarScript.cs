using UnityEngine;

public class AvatarScript : MonoBehaviour
{
    private SpriteRenderer SpriteRenderer;
    private Animator Anim;

    public void Awake()
    {
        Anim = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public GmDelayPromise GlideX(float startX, float targetX, float seconds)
    {
        int steps = 40;
        float step = 0;

        transform.localPosition = new Vector3(startX, transform.localPosition.y, transform.localPosition.z);

        return this.Repeat(seconds / steps, steps, () =>
        {
            step++;
            var x = Mathf.Lerp(startX, targetX, step / steps);
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
            transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
        }, true);
    }

    public void SetSprite(Sprite sprite)
    {
        SpriteRenderer.sprite = sprite;
    }

    public void SetAnimation(string animation)
    {
        Anim.SetTrigger(Animator.StringToHash(animation + "Trigger"));
    }

    internal GmDelayPromise FadeIn(float fadeSeconds)
    {
        return SpriteRenderer.FadeAlpha(this, 0, 1, fadeSeconds, true);
    }
    internal GmDelayPromise FadeOut(float fadeSeconds)
    {
        return SpriteRenderer.FadeAlpha(this, 1, 0, fadeSeconds, true);
    }
}
