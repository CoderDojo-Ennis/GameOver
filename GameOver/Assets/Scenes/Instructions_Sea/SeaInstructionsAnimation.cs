using UnityEngine;

public class SeaInstructionsAnimation : MonoBehaviour
{
    [Header("Transitions")]
    public float SceneStartDelay = 1f;

    [Header("Boat")]
    public GameObject Boat;

    [Header("Enemy")]
    public GameObject EnemyBoat;
    public float EnemyInitialX = -5;
    public float EnemySpeed = 2;
    public Sprite BoatSprite;
    public Sprite BoatShootSprite;
    public GameObject Bullet;
    public float ShootTime = 5;
    private SpriteRenderer BoatRenderer;
    public Transform BulletStart;
    public GameObject BulletInstance;
    private BulletScript BulletScript;

    [Header("Avatar")]
    public AvatarScript Avatar;
    public float LeanAtAngle = 25;
    public float DuckDistance = 0.5f;

    [Header("Waves")]
    public float WaveMaxAngle = 45;
    public float WaveSpeed = 2;
    public float WaveHeight = 2;

    private float BoatStartY;
    private float EnemyBoatStartX;
    private float EnemyBoatStartY;
    public float BoatAngle;
    private float SceneTime = 0;

    // Use this for initialization
    void Start()
    {
        // Designed positions
        BoatStartY = Boat.transform.localPosition.y;
        EnemyBoatStartX = EnemyBoat.transform.localPosition.x;
        EnemyBoatStartY = EnemyBoat.transform.localPosition.y;
        EnemyBoat.transform.localPosition = new Vector3(EnemyInitialX, EnemyBoat.transform.localPosition.y, EnemyBoat.transform.localPosition.z);

        BoatRenderer = EnemyBoat.GetComponent<SpriteRenderer>();

        // Start animation
        this.Delay(SceneStartDelay, () =>
        {
        });

        this.Delay(ShootTime, () =>
        {
            Shoot();
        });
    }

    // Todo - make this an extension method
    float AngleConvert(float f)
    {
        if (f > 180)
        {
            return (360 - f) * -1;
        }
        else
        {
            return f;
        }
    }

    private void Shoot()
    {
        Debug.Log("Shoot", this);
        BoatRenderer.sprite = BoatShootSprite;
        this.Delay(0.1f, () =>
        {
            BoatRenderer.sprite = BoatSprite;
        }, true);

        BulletInstance = Instantiate(Bullet, BulletStart.position, BulletStart.rotation, this.transform);
        BulletInstance.layer = this.gameObject.layer;
        GameManager.Instance.SetTimeScale(.1f);

        BulletScript = BulletInstance.GetComponent<BulletScript>();
        BulletScript.ZoomTime = 0.1f;
        BulletScript.SlowTime = 0.5f;
        BulletScript.LiveTime = 0.6f;
    }

    // Update is called once per frame
    void Update()
    {
        SceneTime += Time.deltaTime;

        // Move the boat
        BoatAngle = WaveMaxAngle * Mathf.Sin(SceneTime * WaveSpeed);
        Boat.transform.localEulerAngles = new Vector3(0, 0, BoatAngle);
        Boat.transform.localPosition = new Vector3(Boat.transform.localPosition.x, BoatStartY - WaveHeight * Mathf.Sin(SceneTime * WaveSpeed + Mathf.PI * .25f), Boat.transform.position.z);

        // Enemy boat - up & down
        EnemyBoat.transform.localPosition = new Vector3(EnemyBoat.transform.localPosition.x, EnemyBoatStartY - WaveHeight * Mathf.Sin(SceneTime * WaveSpeed), EnemyBoat.transform.position.z);
        // Enemy boat - slide left
        if (EnemyBoat.transform.localPosition.x > EnemyBoatStartX)
        {
            EnemyBoat.transform.localPosition = new Vector3(EnemyBoat.transform.localPosition.x - EnemySpeed * Time.deltaTime, EnemyBoat.transform.localPosition.y, EnemyBoat.transform.localPosition.z);
        }

        // Avatar lean
        string animation = "Idle";
        if (BulletInstance != null && Mathf.Abs(BulletInstance.transform.position.x - Avatar.transform.position.x) < DuckDistance)
        {
            animation = "Crouch";
        }
        else if (BoatAngle >= LeanAtAngle)
        {
            animation = "LeanRight";
        }
        else if (BoatAngle <= -LeanAtAngle)
        {
            animation = "LeanLeft";
        }
        if (Avatar.CurrentAnimation != animation)
        {
            Avatar.SetAnimation(animation);
        }
    }
}
