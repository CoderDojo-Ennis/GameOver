using UnityEngine;

public class WaveFollow : MonoBehaviour
{
    public float YOffset;
    public Vector3 RaftPosition;
    public bool Rotate = true;

    void Update()
    {
        transform.position = RaftPosition;
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        int layerMask = ~(1 << LayerMask.NameToLayer("Player")); //Don't collide with the player layer
        if (Physics.Raycast(ray, out hit, 10000, layerMask))
        {
            // Debug.Log(hit.collider.gameObject.name);
            transform.position = hit.point;
            if (Rotate)
            {
                transform.localRotation = Quaternion.FromToRotation(Vector3.up, new Vector3(hit.normal.x, hit.normal.y, hit.normal.z));
            }
            transform.position = new Vector3(transform.position.x, transform.position.y + YOffset, transform.position.z);
            //rb.AddTorque(new Vector3(0, 0, hit.normal.x * rotSpeed * -1), ForceMode.Acceleration);
            //transform.Rotate(Vector3.forward, hit.normal.x * rotSpeed * -1);
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
            //raftWaves.rotation = Quaternion.FromToRotation(Vector3.up, new Vector3(hit.normal.x, hit.normal.y, hit.normal.z));
            //raftWaves.rotation = Quaternion.Euler(0, 0, raftWaves.rotation.eulerAngles.z);
        }
    }
}
