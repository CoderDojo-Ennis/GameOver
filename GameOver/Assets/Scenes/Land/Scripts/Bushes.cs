using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bushes : MonoBehaviour
{
    public static Bushes instance;
    public bool PlayerBehindBushes = false;
    private bool PlayerCollidedLastFrame = false;

	void Start ()
    {
        instance = this;
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCollidedLastFrame = true;
        }
    }

    private void FixedUpdate()
    {
        if (PlayerCollidedLastFrame)
        {
            PlayerBehindBushes = true;
            PlayerCollidedLastFrame = false;
        }
        else
        {
            PlayerBehindBushes = false;
        }
    }
}
