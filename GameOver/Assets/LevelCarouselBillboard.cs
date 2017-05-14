using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCarouselBillboard : MonoBehaviour {
    Vector3 rotation;

    private float RotationSpeed = 90;

	// Use this for initialization
	void Start () {
        rotation = new Vector3(90, 180, 0);

    }
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(rotation), Time.deltaTime * RotationSpeed);	
	}
}
