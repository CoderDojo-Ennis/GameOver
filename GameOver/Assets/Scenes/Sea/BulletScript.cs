using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float BulletSpeed;
    public float ZoomTime;
    public float LiveTime;
    SpriteRenderer s;

    void Start()
    {
        s = GetComponent<SpriteRenderer>();
        this.Delay(ZoomTime, () =>
        {
            if (SeaCameraScript.instance != null)
            {
                SeaCameraScript.instance.LerpToNewBullet(gameObject);
            }
            else if (SeaInstructionsCameraScript.instance != null)
            {
                SeaInstructionsCameraScript.instance.LerpToNewBullet(gameObject);
            }
        });
        //SeaCameraScript.instance.LerpToNewBullet(gameObject);
        this.Delay(LiveTime, () =>
        {
            Time.timeScale = 1;
            Destroy(gameObject);
        });
    }

    void Update()
    {
        transform.Translate(-BulletSpeed * Time.deltaTime, 0, 0, Space.Self);
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            s.enabled = false;
            RaftScript.instance.Drown();
        }
    }
}
