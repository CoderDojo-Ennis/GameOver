using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    public GameObject Explosion;
    public void Explode()
    {
        Instantiate(Explosion);
        Destroy(gameObject);
    }
}
