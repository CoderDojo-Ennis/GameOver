using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float BulletSpeed;
    public int LiveTime;
    SpriteRenderer s;

	void Start ()
    {
        s = GetComponent<SpriteRenderer>();
        SeaCameraScript.instance.LerpToNewBullet(gameObject);
        this.Delay(LiveTime, () => { Destroy(gameObject); });
	}
	
	void Update ()
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
