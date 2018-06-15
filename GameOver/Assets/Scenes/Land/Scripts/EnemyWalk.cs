using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalk : MonoBehaviour
{
    private bool WalkingRight = true;   //if false - walking left
    private GuardScript guardScript;

    void Start ()
    {
        guardScript = GetComponentInParent<GuardScript>();
	}
	
	void Update ()
    {
        float moveDistance = (WalkingRight ? guardScript.WalkSpeed : -guardScript.WalkSpeed) * Time.deltaTime;
        transform.Translate(moveDistance, 0, 0);
        if (WalkingRight)
        {
            if (transform.position.x > guardScript.RightWalkBoundary.position.x)
            {
                WalkingRight = false;
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
            if (transform.position.x < guardScript.LeftWalkBoundary.position.x)
            {
                WalkingRight = true;
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
    }
}
