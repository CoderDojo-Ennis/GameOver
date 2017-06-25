using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaftScript : MonoBehaviour
{
    public float YOffset;
	void Start () {
		
	}

	void Update () {
        transform.position = new Vector3(0, 10, 0);
        RaycastHit hit;
        Ray r = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(r, out hit))
        {
            transform.position = hit.point;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + YOffset, transform.localPosition.z);
        }
    }
}
