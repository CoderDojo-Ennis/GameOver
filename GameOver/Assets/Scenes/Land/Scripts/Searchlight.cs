using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Searchlight : MonoBehaviour
{
    public float SweepRange;
    public float SweepSpeed;
    private float LocalSweepSpeed;
    private float ReferenceRotation;
    public int FramesVisibleForLoss;
    private int FrameCounter = 0;
    private bool PlayerVisible = true;
    public Color PlayerDark;
    public Color PlayerLight;
    private bool PlayerCollidedLastFrame = false;
    public bool Sweeping = true;

	void Start ()
    {
        ReferenceRotation = transform.rotation.eulerAngles.y;
        LocalSweepSpeed = SweepSpeed;
	}
	
	void Update ()
    {
        if (Sweeping)
        {
            transform.Rotate(0, LocalSweepSpeed * Time.deltaTime, 0, Space.World);
            //transform.Rotate(Vector3.up, LocalSweepSpeed * Time.deltaTime);
        }
        if (transform.rotation.eulerAngles.y > ReferenceRotation + SweepRange)
        {
            LocalSweepSpeed = -SweepSpeed;
        }
        else if (transform.rotation.eulerAngles.y < ReferenceRotation - SweepRange)
        {
            LocalSweepSpeed = SweepSpeed;
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
                if (FrameCounter >= FramesVisibleForLoss && Sweeping)
                {
                    FrameCounter = 0;
                    Sweeping = false;
                    LandScene.instance.Fail();
                }
            }
            PlayerCollidedLastFrame = false;
        }
        else
        {
            PlayerHidden();
        }
    }

    public void Restart()
    {
        FrameCounter = 0;
        Sweeping = true;
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
