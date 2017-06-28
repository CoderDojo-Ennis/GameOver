using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSea : MonoBehaviour
{
    //public float speed = 30;
    public float LeanTorque;
    public Sprite Left;
    public Sprite Right;
    public Sprite Idle;
    SpriteRenderer sp;
    Rigidbody raft;

	void Start ()
    {
        raft = GetComponentInParent<Rigidbody>();
        sp = GetComponent<SpriteRenderer>();
	}
	
	void Update ()
    {
        if (Input.GetKey("left"))
        {
            raft.AddTorque(new Vector3(0, 0, LeanTorque), ForceMode.Acceleration);
            sp.sprite = Left;
            //transform.Rotate(0, 0, Time.deltaTime * speed);
        }
        else if (Input.GetKey("right"))
        {
            raft.AddTorque(new Vector3(0, 0, -LeanTorque), ForceMode.Acceleration);
            sp.sprite = Right;
            //transform.Rotate(0, 0, Time.deltaTime * speed * -1);
        }
        else
        {
            sp.sprite = Idle;
        }
    }
}
