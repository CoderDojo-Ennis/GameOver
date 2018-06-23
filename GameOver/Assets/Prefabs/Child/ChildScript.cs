using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildScript : MonoBehaviour
{
    private Animator Anim;
    private Transform Following = null;
    private float FollowTolerance = 0.8f;
    private float FollowSpeed = 1.5f;
    private float XScale;

    private void Start()
    {
        Anim = GetComponent<Animator>();
        XScale = transform.localScale.x;
    }

    private void Update()
    {
        if (Following != null)
        {
            if (transform.position.x < Following.position.x - FollowTolerance)
            {
                transform.localScale = new Vector3(XScale, transform.localScale.y, transform.localScale.z);
                transform.Translate(FollowSpeed * Time.deltaTime, 0, 0);
                SetAnimation("ChildWalk");
            }
            else if (transform.position.x > Following.position.x + FollowTolerance)
            {
                transform.localScale = new Vector3(-XScale, transform.localScale.y, transform.localScale.z);
                transform.Translate(-FollowSpeed * Time.deltaTime, 0, 0);
                SetAnimation("ChildWalk");
            }
            else
            {
                SetAnimation("ChildIdle");
            }
        }
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

    public void SetAnimation(string animation)
    {
        Anim.SetTrigger(Animator.StringToHash(animation));
    }

    public void StartFollowing(Transform ToFollow)
    {
        Following = ToFollow;
    }
}
