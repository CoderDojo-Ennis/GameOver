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
    private PlayerJoints Joints;
    public float KinectTorqueScale = 1;

    void Start()
    {
        raft = GetComponentInParent<Rigidbody>();
        sp = GetComponent<SpriteRenderer>();
        Joints = PlayerScript.Instance.GetComponent<PlayerJoints>();
    }

    void Update()
    {
        float KinectLeanTorque = (Joints.head.transform.localPosition.x - Joints.spineBase.transform.localPosition.x) * KinectTorqueScale;
        sp.enabled = (KinectLeanTorque == 0);

        raft.AddTorque(new Vector3(0, 0, -KinectLeanTorque), ForceMode.Acceleration);

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

        // Move/rotate kinect image
        PlayerScript.Instance.transform.position = transform.position;
        PlayerScript.Instance.transform.rotation = transform.rotation;
    }
}
