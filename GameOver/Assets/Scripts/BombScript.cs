using UnityEngine;

public class BombScript : MonoBehaviour
{
    public GameObject Explosion;

    public void Explode()
    {
        Instantiate(Explosion, this.transform.position, this.transform.rotation);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Explode();
    }
}
