using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpScript : MonoBehaviour
{
    private Animator Anim;
    private float movingSpeed = 0;

    public void Awake()
    {
        Anim = GetComponent<Animator>();
    }

    public GmDelayPromise GlideX(float startX, float targetX, float seconds)
    {
        int steps = 80;
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

    public void StartMoving(float speed)
    {
        movingSpeed = speed;
    }

    private void Update()
    {
        transform.Translate(movingSpeed * Time.deltaTime, 0, 0);
    }

    public void SetAnimation(string animation)
    {
        Anim.SetTrigger(Animator.StringToHash(animation));
    }
}
