using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneScript : MonoBehaviour
{
    public Transform DropPositions;
    private Vector3 DropPosLeft;
    private Vector3 DropPosRight;
    public float FinalHeight;
    public float RotateSpeed;
    public float BobDistance;
    public float BobSpeed;
    private float MaxYPos;
    private float MinYPos;
    private bool GoingUp;
    private float PositionTolerance = 0.1f;
    private float FallSpeed = 0;
    private bool Collected = false;

    private float YVelocity;
    public float ThrownRotateSpeed;

    void Start ()
    {
        DropPosLeft = DropPositions.Find("Left").position;
        DropPosRight = DropPositions.Find("Right").position;
        MaxYPos = transform.position.y + BobDistance;
        MinYPos = transform.position.y - BobDistance;
    }

    public void Restart()
    {
        FallSpeed = 0;
        Collected = false;
        transform.position = DropPosLeft;
        transform.rotation = Quaternion.identity;
        MaxYPos = transform.position.y + BobDistance;
        MinYPos = transform.position.y - BobDistance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !Collected)
        {
            Debug.Log("Collected Bone");
            GetComponent<AudioSource>().Play();
            transform.rotation = Quaternion.Euler(Vector3.zero);
            Collected = true;
            LandScene.instance.BoneCollected();
            YVelocity = 10;
        }
    }

    void Update()
    {
        if (Collected)
        {
            YVelocity -= 9.8f * Time.deltaTime;
            float xMovement = (Vector3.Distance(transform.position, DropPosLeft) > Vector3.Distance(transform.position, DropPosRight) ? 2 : -2) * Time.deltaTime;
            transform.Translate(xMovement, YVelocity * Time.deltaTime, 2 * Time.deltaTime, Space.World);
            transform.Rotate(0, 0, ThrownRotateSpeed * Time.deltaTime);
        }
        else
        {
            transform.Rotate(Vector3.up, RotateSpeed * Time.deltaTime, Space.Self);
            if (FallSpeed > 0)
            {
                transform.Translate(0, -FallSpeed, 0);
                FallSpeed += 0.07f;
                if (transform.localPosition.y <= FinalHeight)
                {
                    FallSpeed = 0;
                    transform.localPosition = new Vector3(transform.position.x, FinalHeight, transform.position.z);
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

    public string Fall()
    {
        string whereDropped = "";
        if (Vector3.Distance(PlayerScript.Instance.transform.position, DropPosLeft) > Vector3.Distance(PlayerScript.Instance.transform.position, DropPosRight))
        {
            transform.position = DropPosLeft;
            whereDropped = "left";
        }
        else
        {
            transform.position = DropPosRight;
            whereDropped = "right";
        }
        FallSpeed = 0.01f;
        return whereDropped;
    }
}
