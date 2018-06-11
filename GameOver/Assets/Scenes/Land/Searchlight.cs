using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Searchlight : MonoBehaviour
{
    public float SweepRange;
    public float SweepSpeed;
    private float InitialRotation;
    public int FramesVisibleForLoss;
    private int FrameCounter = 0;
    private bool PlayerVisible = true;
    public Color PlayerDark;
    public Color PlayerLight;
    private bool PlayerCollidedLastFrame = false;

	void Start ()
    {
        InitialRotation = transform.rotation.eulerAngles.y;
	}
	
	void Update ()
    {
        transform.Rotate(transform.up, SweepSpeed * Time.deltaTime);
        if (transform.rotation.eulerAngles.y > InitialRotation + SweepRange || transform.rotation.eulerAngles.y < InitialRotation - SweepRange)
        {
            SweepSpeed = -SweepSpeed;
        }
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
            int layers = ~(1 << LayerMask.NameToLayer("Player"));
            RaycastHit info;
            if (Physics.Linecast(transform.position, PlayerScript.Instance.transform.position, out info, layers))
            {
                //Debug.Log(info.collider.gameObject.name);
                PlayerHidden();
            }
            else
            {
                if (PlayerVisible == false)
                {
                    PlayerVisible = true;
                    PlayerScript.Instance.PlayerImage.SetColour(PlayerLight, 0.1f);
                }
                FrameCounter++;
                if (FrameCounter >= FramesVisibleForLoss)
                {
                    //Player has been seen for too long - lose
                }
            }
            PlayerCollidedLastFrame = false;
        }
        else
        {
            PlayerHidden();
        }
    }

    void PlayerHidden()
    {
        FrameCounter = 0;
        if (PlayerVisible == true)
        {
            PlayerVisible = false;
            PlayerScript.Instance.PlayerImage.SetColour(PlayerDark, 0.1f);
        }
    }
}
