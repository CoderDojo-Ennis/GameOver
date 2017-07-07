using UnityEngine;

public class SeaCameraScript : MonoBehaviour
{
    Vector3 NormalPosition;
    public static SeaCameraScript instance;
    GameObject FollowBullet;
    float LerpTime = 0;
    public float LerpSpeed = 7;
    public float LerpDirection = 1;
    public float BulletZoom = -4;
    public float BulletSlowdown = 0.1f;
    public float ZoomInSeconds = .5f;
    public float ZoomOutSeconds = .3f;
    //private BulletScript BulletScript;
    public Transform PlayerPosition;

    void Start()
    {
        NormalPosition = transform.position;
        instance = this;
    }

    void Update()
    {
        LerpTime += Time.deltaTime * LerpSpeed * LerpDirection;
        LerpTime = Mathf.Clamp(LerpTime, 0, 1);
        //Debug.Log(LerpTime);
        if (FollowBullet != null)
        {
            Vector3 BetweenBulletAndPlayer = Vector3.Lerp(FollowBullet.transform.position, PlayerPosition.position, 0.5f);
            transform.position = Vector3.Lerp(NormalPosition, new Vector3(BetweenBulletAndPlayer.x, BetweenBulletAndPlayer.y, BulletZoom), LerpTime);
        }
    }

    void ResetCamera()
    {
        transform.position = NormalPosition;
    }

    void ZoomOut()
    {
        LerpDirection = -(ZoomInSeconds / ZoomOutSeconds);
    }

    public void LerpToNewBullet(GameObject bullet)
    {
        LerpTime = 0;
        FollowBullet = bullet;
        GameManager.Instance.SetTimeScale(BulletSlowdown);
        GameManager.Instance.SetSoundTimeScale();
        LerpDirection = 1;
        //BulletScript = bullet.GetComponent<BulletScript>();
        //this.Delay(BulletScript.LiveTime, ResetCamera);
        this.Delay(ZoomInSeconds, () =>
        {
            ZoomOut();
        });
    }
}
