﻿using UnityEngine;

public class SeaCameraScript : MonoBehaviour
{
    Vector3 NormalPosition;
    public static SeaCameraScript instance;
    GameObject FollowBullet;
    float LerpTime = 0;
    public float LerpSpeed = 7;
    public float BulletZoom = -4;
    public float BulletSlowdown = 0.1f;

    void Start()
    {
        NormalPosition = transform.position;
        instance = this;
    }

    void Update()
    {
        LerpTime += Time.deltaTime * LerpSpeed;
        if (FollowBullet != null)
        {
            Time.timeScale = BulletSlowdown;
            transform.position = Vector3.Lerp(NormalPosition, new Vector3(FollowBullet.transform.position.x, FollowBullet.transform.position.y, BulletZoom), LerpTime);
        }
        else
        {
            transform.position = NormalPosition;
        }
    }

    public void LerpToNewBullet(GameObject bullet)
    {
        LerpTime = 0;
        FollowBullet = bullet;
    }
}
