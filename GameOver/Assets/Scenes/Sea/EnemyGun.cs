using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    SpriteRenderer sr;
    [Header("Objects")]
    public GameObject BulletPrefab;
    public Transform FiringPoint;
    public Sprite normal;
    public Sprite firing;
    public Transform LookTowards;
    public GameObject Arm;
    [Header("Variables")]
    public float ShootDelay;
    public float SprayAngle;
    public float FireSpriteTime;
    public float RecoilDistance;
    private Vector3 OriginalPos;
    private float GunHeight;

    void Start()
    {
        OriginalPos = transform.localPosition;
        sr = GetComponent<SpriteRenderer>();
        InvokeRepeating("Shoot", ShootDelay, ShootDelay);
        GunHeight = FiringPoint.position.y - transform.position.y;
    }

    void Shoot()
    {
        sr.sprite = firing;
        transform.Translate(RecoilDistance, 0, 0, Space.Self);
        GameObject b;
        b = Instantiate(BulletPrefab, FiringPoint.position, transform.rotation);
        //b.transform.eulerAngles = new Vector3(b.transform.eulerAngles.x, b.transform.eulerAngles.y, b.transform.eulerAngles.z + Random.Range(-SprayAngle, SprayAngle));
        this.Delay(FireSpriteTime, ResetSprite);
    }

    void ResetSprite()
    {
        transform.localPosition = OriginalPos;
        sr.sprite = normal;
    }

    void Update()
    {
        Vector3 aimTowards = new Vector3(LookTowards.position.x, LookTowards.position.y - GunHeight, LookTowards.position.z);
        Vector3 targetDir = aimTowards - transform.position;
        float angle = (Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg) + 180;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Arm.transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z * -1);
    }
}
