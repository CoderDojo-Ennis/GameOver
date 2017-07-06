using System.Collections;
using UnityEngine;

public class RaftScript : MonoBehaviour
{
    public static RaftScript instance;
    public float drownAngle;
    AudioSource splashSound;
    GameObject Player;
    SpriteRenderer PlayerRenderer;
    bool CanDie = true;
    bool RespawnWithAvatar;
    Rigidbody r;
    public float WaveTorqueScale = .1f;

    void Start()
    {
        instance = this;
        Player = GetComponentInChildren<AvatarSea>().gameObject;
        PlayerRenderer = Player.GetComponent<SpriteRenderer>();
        splashSound = GetComponent<AudioSource>();
        r = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float angle = AngleConvert(transform.eulerAngles.z);
        if ((angle < -drownAngle || angle > drownAngle) && CanDie)
        {
            Drown();
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            this.Respawn();
        }

        float waveTilt = AngleConvert(transform.parent.transform.rotation.eulerAngles.z);
        Debug.Log(waveTilt);
        r.AddTorque(new Vector3(0, 0, waveTilt * WaveTorqueScale), ForceMode.Acceleration);
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

    public void Drown()
    {
        CanDie = false;
        //Do some kind of death animation here later
        this.Delay(2, () =>
        {
            Respawn();
        });
        splashSound.Play();
        PlayerScript.Instance.Damage(10);
        RespawnWithAvatar = PlayerRenderer.enabled;
        if (RespawnWithAvatar)
        {
            PlayerRenderer.enabled = false;
        }
        else
        {
            PlayerScript.Instance.HideKinect(0.5f);
        }
    }

    void Respawn()
    {
        CanDie = true;
        transform.localRotation = Quaternion.identity;
        r.angularVelocity = Vector3.zero;
        if (RespawnWithAvatar)
        {
            BlinkPlayer(10);
        }
        else
        {
            this.Repeat(0.15f, 8, () =>
            {
                PlayerScript.Instance.HideKinect(0).Then(() =>
                {
                    this.Delay(0.07f, () =>
                    {
                        PlayerScript.Instance.ShowKinect(0);
                    });
                });
            });
        }
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
