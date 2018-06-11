using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandCollectable : MonoBehaviour
{
    public float FinalHeight;
    public float RotateSpeed;
    public float BobDistance;
    public float BobSpeed;
    private float MaxYPos;
    private float MinYPos;
    private bool GoingUp;
    private float PositionTolerance = 0.1f;
    private float FallSpeed = 0;
    public Vector3 CollectedLocation;
    private bool Collected = false;
    private Vector3 MoveFrom;
    private float MoveLerp = 0;

    void Start ()
    {
        MaxYPos = transform.position.y + BobDistance;
        MinYPos = transform.position.y - BobDistance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !Collected)
        {
            Debug.Log("Collected");
            GetComponent<AudioSource>().Play();
            MoveFrom = transform.position;
            transform.rotation = Quaternion.Euler(Vector3.zero);
            LandScene.instance.CuttersCollected();
            Collected = true;
        }
    }

    void Update ()
    {
        if (Collected)
        {
            transform.position = Vector3.Lerp(MoveFrom, CollectedLocation, MoveLerp);
            MoveLerp += 4 * Time.deltaTime;
        }
        else
        {
            transform.Rotate(Vector3.up, RotateSpeed * Time.deltaTime, Space.Self);
            if (FallSpeed > 0)
            {
                transform.Translate(0, -FallSpeed, 0);
                FallSpeed += 0.07f;
                if (transform.position.y <= FinalHeight)
                {
                    FallSpeed = 0;
                    transform.position = new Vector3(transform.position.x, FinalHeight, transform.position.z);
                    MaxYPos = transform.position.y + BobDistance;
                    MinYPos = transform.position.y - BobDistance;
                }
            }
            else
            {
                if (GoingUp)
                {
                    transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, MaxYPos, BobSpeed), transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, MinYPos, BobSpeed), transform.position.z);
                }
                if (transform.position.y > MaxYPos - PositionTolerance)
                {
                    transform.position = new Vector3(transform.position.x, MaxYPos - PositionTolerance, transform.position.z);
                    GoingUp = false;
                }
                else if (transform.position.y < MinYPos + PositionTolerance)
                {
                    transform.position = new Vector3(transform.position.x, MinYPos + PositionTolerance, transform.position.z);
                    GoingUp = true;
                }
            }
        }
	}

    public void Fall()
    {
        FallSpeed = 0.01f;
    }
}
