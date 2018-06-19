using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardScript : MonoBehaviour
{
    public Transform Guard;
    public Transform Dog;
    private Transform GuardLeashPos;
    private Transform DogLeashPos;
    private LineRenderer Leash;
    public float WalkSpeed;
    public float RunSpeed;
    public Transform LeftWalkBoundary;
    public Transform RightWalkBoundary;
    public GameObject ExclamationMark;
    private AudioSource BarkSound;
    public int FramesVisibleForLoss;
    private int FrameCounter = 0;
    private bool PlayerCollidedLastFrame = false;

    void Start ()
    {
        Leash = GetComponent<LineRenderer>();
        GuardLeashPos = Guard.Find("LeashPosition");
        DogLeashPos = Dog.Find("LeashPosition");
        BarkSound = Dog.GetComponent<AudioSource>();
    }
	
	void Update ()
    {
        Leash.SetPosition(0, transform.InverseTransformPoint(GuardLeashPos.position));
        Leash.SetPosition(1, transform.InverseTransformPoint(DogLeashPos.position));
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
        if (PlayerCollidedLastFrame && !Bushes.instance.PlayerBehindBushes)
        {
            FrameCounter++;
            if (FrameCounter >= FramesVisibleForLoss)
            {
                FrameCounter = 0;
                SawSomething();
                LandScene.instance.Fail();
            }
            PlayerCollidedLastFrame = false;
        }
        else
        {
            FrameCounter = 0;
        }
    }


    public void SawSomething(string WhereToMove = null)
    {
        BarkSound.Play();
        ExclamationMark.SetActive(true);
        this.Delay(2, () => { ExclamationMark.SetActive(false); });
        if (WhereToMove != null)
        {
            bool direction = false;
            if (WhereToMove == "left")
            {
                direction = false;
            }
            else if (WhereToMove == "right")
            {
                direction = true;
            }

            foreach (EnemyWalk e in GetComponentsInChildren<EnemyWalk>())
            {
                e.RunOffScreen(false, direction);
            }
        }
        else
        {
            foreach (EnemyWalk e in GetComponentsInChildren<EnemyWalk>())
            {
                e.RunOffScreen(true, true);
            }
        }
    }
}
