using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaftScript : MonoBehaviour
{
    public float drownAngle;
    AudioSource splashSound;
    GameObject Player;
    SpriteRenderer PlayerRenderer;
    bool CanDie = true;
    Rigidbody r;

    void Start ()
    {
        Player = GetComponentInChildren<AvatarSea>().gameObject;
        PlayerRenderer = Player.GetComponent<SpriteRenderer>();
        splashSound = GetComponent<AudioSource>();
        r = GetComponent<Rigidbody>();
	}

	void Update ()
    {
        float angle = AngleConvert(transform.eulerAngles.z);
        if ((angle < -drownAngle || angle > drownAngle) && CanDie)
        {
            Drown();
        }
    }

    float AngleConvert(float f)
    {
        if (f > 180)
        {
            return (360 - f) * -1;
        }
        else
        {
            return f;
        }
    }

    void Drown()
    {
        CanDie = false;
        //Do some kind of death animation here later
        this.Delay(2, () =>
        {
            Respawn();
        });
        splashSound.Play();
        PlayerScript.Instance.Damage(1);
        PlayerRenderer.enabled = false;
    }

    void Respawn()
    {
        CanDie = true;
        transform.localRotation = Quaternion.identity;
        r.angularVelocity = Vector3.zero;
        BlinkPlayer(10);
    }

    void BlinkPlayer(int numBlinks)
    {
        StartCoroutine(DoBlinks(numBlinks, 0.1f));
    }

    IEnumerator DoBlinks(int numBlinks, float seconds)
    {
        for (int i = 0; i < numBlinks * 2; i++)
        {
            PlayerRenderer.enabled = !PlayerRenderer.enabled;
            yield return new WaitForSeconds(seconds);
        }
        PlayerRenderer.enabled = true;
    }
}
