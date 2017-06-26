using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaftScript : MonoBehaviour
{
    public float YOffset;
    public Vector3 RaftPosition;
    public float rotSpeed;
    public Rigidbody rb;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}

	void Update () {
        transform.position = RaftPosition;
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out hit))
        {
            transform.position = hit.point;
            //transform.localRotation = Quaternion.FromToRotation(Vector3.up, new Vector3(hit.normal.x, hit.normal.y, hit.normal.z));
            transform.position = new Vector3(transform.position.x, transform.position.y + YOffset, transform.position.z);
            rb.AddTorque(new Vector3(0, 0, hit.normal.x * rotSpeed * -1), ForceMode.Acceleration);
            Debug.Log(hit.normal.x * rotSpeed * -1);
            //transform.Rotate(Vector3.forward, hit.normal.x * rotSpeed * -1);
            //transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
        }
    }
}
