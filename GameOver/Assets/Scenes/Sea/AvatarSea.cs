using UnityEngine;

public class AvatarSea : MonoBehaviour
{
    //public float speed = 30;
    public float LeanTorque;
    public Sprite Left;
    public Sprite Right;
    public Sprite Idle;
    public Sprite Duck;
    SpriteRenderer sp;
    BoxCollider2D boxColider;
    Rigidbody raft;
    private PlayerJoints Joints;
    public float KinectTorqueScale = 1;
    public float KinectPositionTorqueScale = 0.5f;

    void Start()
    {
        raft = GetComponentInParent<Rigidbody>();
        sp = GetComponent<SpriteRenderer>();
        boxColider = GetComponent<BoxCollider2D>();
        Joints = PlayerScript.Instance.GetComponent<PlayerJoints>();
    }

    void FixedUpdate()
    {
        float KinectLeanTorque = (Joints.head.transform.localPosition.x - Joints.spineBase.transform.localPosition.x) * KinectTorqueScale;
        float KinectPositionTorque = Joints.spineBase.transform.localPosition.x * KinectPositionTorqueScale;
        raft.AddTorque(new Vector3(0, 0, -KinectLeanTorque - KinectPositionTorque), ForceMode.Acceleration);

        // If no kinect - disable avatar collider
        boxColider.enabled = sp.enabled = (KinectLeanTorque == 0);

        if (Input.GetKey(KeyCode.DownArrow))
        {
            boxColider.size = new Vector2(boxColider.size.x, 0.2f);
            sp.sprite = Duck;
        }
        else
        {
            boxColider.size = new Vector2(boxColider.size.x, 0.37f);
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                raft.AddTorque(new Vector3(0, 0, LeanTorque), ForceMode.Acceleration);
                sp.sprite = Left;
                //transform.Rotate(0, 0, Time.deltaTime * speed);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
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

    private void Update()
    {
        // Move/rotate kinect image
        PlayerScript.Instance.transform.position = transform.position;
        PlayerScript.Instance.transform.rotation = transform.rotation;
    }
}
