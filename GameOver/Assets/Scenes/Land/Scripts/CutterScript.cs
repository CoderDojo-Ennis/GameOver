using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutterScript : MonoBehaviour
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
    public Vector3 CollectedLocation;
    private bool Collected = false;
    private Vector3 MoveFrom;
    private float MoveLerp = 0;
    public GameObject mesh;
    public GameObject sprite;

    void Start ()
    {
        DropPosLeft = DropPositions.Find("Left").position;
        DropPosRight = DropPositions.Find("Right").position;
        MaxYPos = transform.position.y + BobDistance;
        MinYPos = transform.position.y - BobDistance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !Collected)
        {
            Debug.Log("Collected Cutters");
            GetComponent<AudioSource>().Play();
            MoveFrom = transform.position;
            transform.rotation = Quaternion.Euler(Vector3.zero);
            LandScene.instance.CuttersCollected();
            Collected = true;
        }
    }

    public void Restart()
    {
        MoveLerp = 0;
        FallSpeed = 0;
        Collected = false;
        transform.position = DropPosLeft;
        transform.rotation = Quaternion.identity;
        try
        {
            mesh.SetActive(true);
            sprite.SetActive(false);
        }
        catch (System.NullReferenceException) { }
    }

    void Update ()
    {
        if (Collected)
        {
            if (MoveLerp < 0.95f)
            {
                transform.position = Vector3.Lerp(MoveFrom, CollectedLocation, MoveLerp);
                MoveLerp += 4 * Time.deltaTime;
            }
            try
            {
                mesh.SetActive(false);
                sprite.SetActive(true);
            }
            catch (System.NullReferenceException) { } 
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
        if (Vector3.Distance(PlayerScript.Instance.transform.position, DropPosLeft) > Vector3.Distance(PlayerScript.Instance.transform.position, DropPosRight))
        {
            transform.position = DropPosLeft;
        }
        else
        {
            transform.position = DropPosRight;
        }
        FallSpeed = 0.01f;
    }
}
