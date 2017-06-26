using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaftScript : MonoBehaviour
{
    public float YOffset;
    public Vector3 RaftPosition;
	void Start () {
		
	}

	void Update () {
        transform.position = RaftPosition;
        RaycastHit hit;
        Ray r = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(r, out hit))
        {
            transform.position = hit.point;
            //transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + YOffset, transform.localPosition.z);
        }
    }
}
