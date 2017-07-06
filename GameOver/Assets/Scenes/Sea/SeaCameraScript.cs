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
    public float ZoomOutSeconcs = .3f;
    private BulletScript BulletScript;

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
            transform.position = Vector3.Lerp(NormalPosition, new Vector3(FollowBullet.transform.position.x, FollowBullet.transform.position.y, BulletZoom), LerpTime);
        }
    }

    void ResetCamera()
    {
        transform.position = NormalPosition;
    }

    void ZoomOut()
    {
        LerpDirection = -(ZoomInSeconds / ZoomOutSeconcs);
    }

    public void LerpToNewBullet(GameObject bullet)
    {
        LerpTime = 0;
        FollowBullet = bullet;
        Time.timeScale = BulletSlowdown;
        GameManager.Instance.SetSoundTimeScale();
        LerpDirection = 1;
        BulletScript = bullet.GetComponent<BulletScript>();
        //this.Delay(BulletScript.LiveTime, ResetCamera);
        this.Delay(ZoomInSeconds, () =>
        {
            ZoomOut();
        });
    }
}
