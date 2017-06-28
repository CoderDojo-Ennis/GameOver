using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaftScript : MonoBehaviour
{
    public float drownAngle;
    AudioSource splashSound;

    void Start ()
    {
        splashSound = GetComponent<AudioSource>();
	}

	void Update ()
    {
        if (transform.eulerAngles.z < -drownAngle || transform.eulerAngles.z > drownAngle)
        {
            Debug.Log(transform.eulerAngles.z);
            Drown();
        }
    }

    void Drown()
    {
        splashSound.Play();
    }
}
