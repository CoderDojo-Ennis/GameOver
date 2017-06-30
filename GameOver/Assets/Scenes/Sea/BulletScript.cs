using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float BulletSpeed;

	void Start () {

	}
	
	void Update ()
    {
        transform.Translate(-BulletSpeed * Time.deltaTime, 0, 0, Space.Self);
	}
}
