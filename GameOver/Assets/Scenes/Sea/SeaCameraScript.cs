using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaCameraScript : MonoBehaviour
{
    Vector3 NormalPosition;
    public static SeaCameraScript instance;
    GameObject FollowBullet;
    float LerpTime = 0;
    public float LerpSpeed;
    public float BulletZoom;
    public float BulletSlowdown;

	void Start ()
    {
        NormalPosition = transform.position;
        instance = this;
	}
	
	void Update ()
    {
        LerpTime += Time.deltaTime * LerpSpeed;
        if (FollowBullet != null)
        {
            Time.timeScale = BulletSlowdown;
            transform.position = Vector3.Lerp(NormalPosition, new Vector3(FollowBullet.transform.position.x, FollowBullet.transform.position.y, BulletZoom), LerpTime);
        }
        else
        {
            Time.timeScale = 1;
            transform.position = NormalPosition;
        }
	}

    public void LerpToNewBullet(GameObject bullet)
    {
        LerpTime = 0;
        FollowBullet = bullet;
    }
}
