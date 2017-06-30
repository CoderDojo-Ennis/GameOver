using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoat : MonoBehaviour
{
    SpriteRenderer sr;
    [Header("Objects")]
    public GameObject BulletPrefab;
    public Transform FiringPoint;
    public Sprite normal;
    public Sprite firing;
    [Header("Variables")]
    public float ShootDelay;
    public float SprayAngle;
    public float FireSpriteTime;

	void Start ()
    {
        sr = GetComponent<SpriteRenderer>();
        InvokeRepeating("Shoot", ShootDelay, ShootDelay);
	}
	
    void Shoot()
    {
        sr.sprite = firing;
        GameObject b;
        b = Instantiate(BulletPrefab, FiringPoint.position, transform.rotation);
        b.transform.eulerAngles = new Vector3(b.transform.eulerAngles.x, b.transform.eulerAngles.y, b.transform.eulerAngles.z + Random.Range(-SprayAngle, SprayAngle));
        this.Delay(FireSpriteTime, ResetSprite);
    }

    void ResetSprite()
    {
        sr.sprite = normal;
    }

	void FixedUpdate ()
    {

	}
}
