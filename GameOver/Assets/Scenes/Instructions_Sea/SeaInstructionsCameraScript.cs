using UnityEngine;

public class SeaInstructionsCameraScript : MonoBehaviour
{
    Vector3 NormalPosition;
    public static SeaInstructionsCameraScript instance;
    GameObject FollowBullet;
    float LerpTime = 0;
    public float LerpSpeed = 7;
    public float BulletZoom = -4;
    public float BulletSlowdown = 0.1f;
    public Transform PlayerPosition;

    void Start()
    {
        NormalPosition = transform.position;
        instance = this;
    }

    void Update()
    {
        LerpTime += Time.deltaTime * LerpSpeed;
        if (FollowBullet != null)
        {
            Vector3 BetweenBulletAndPlayer = Vector3.Lerp(FollowBullet.transform.position, PlayerPosition.position, 0.5f);
            transform.position = Vector3.Lerp(NormalPosition, new Vector3(BetweenBulletAndPlayer.x, BetweenBulletAndPlayer.y, BulletZoom), LerpTime);
        }
        else
        {
            transform.position = NormalPosition;
        }
    }

    public void LerpToNewBullet(GameObject bullet)
    {
        LerpTime = 0;
        FollowBullet = bullet;
    }
}
