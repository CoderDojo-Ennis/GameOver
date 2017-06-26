using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSea : MonoBehaviour
{
    public float speed = 1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("left"))
        {
            transform.Rotate(0, 0, Time.deltaTime * speed);
        }
        if (Input.GetKey("right"))
        {
            transform.Rotate(0, 0, Time.deltaTime * speed * -1);
        }
    }
}
