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
    public Transform LeftWalkBoundary;
    public Transform RightWalkBoundary;

    void Start ()
    {
        Leash = GetComponent<LineRenderer>();
        GuardLeashPos = Guard.Find("LeashPosition");
        DogLeashPos = Dog.Find("LeashPosition");
    }
	
	void Update ()
    {
        Leash.SetPosition(0, transform.InverseTransformPoint(GuardLeashPos.position));
        Leash.SetPosition(1, transform.InverseTransformPoint(DogLeashPos.position));
    }
}
