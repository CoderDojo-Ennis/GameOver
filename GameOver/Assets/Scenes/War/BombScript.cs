using UnityEngine;

public class BombScript : MonoBehaviour
{
    public GameObject Explosion;
    public GameObject Shadow;
    public float ShadowY = -1.1f;

    private GameObject shadowOject;

    public void Start()
    {
        CreateShadow();
    }

    public void CreateShadow()
    {
        Vector3 shadowPos = new Vector3(this.transform.position.x, ShadowY, this.transform.position.z);
        shadowOject = Instantiate(Shadow, shadowPos, this.transform.rotation).gameObject;
    }

    public void Explode()
    {
        Destroy(shadowOject);
        Instantiate(Explosion, this.transform.position, this.transform.rotation);
        Destroy(gameObject);

        // todo - calculate damage
        PlayerScript.Instance.Damage(10);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Explode();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Explode();
    }
}
