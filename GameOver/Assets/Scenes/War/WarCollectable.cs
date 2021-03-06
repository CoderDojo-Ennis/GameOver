﻿using System;
using UnityEngine;

public class WarCollectable : MonoBehaviour
{
    [Header("Drop")]
    public AudioClip DropSound;
    public float InitialScale = 1;

    [Header("Collected")]
    // Scale of the item when collected
    public float CollectedScale = 0.5f;
    public float CollectAnimationSeconds = 0.5f;
    public AudioClip CollectedSound;

    // Events
    public event EventHandler OnCollected;
    public event EventHandler OnDestroyed;
    public event EventHandler OnHide;
    public event EventHandler OnShow;

    private SpriteRenderer Sprite;

    // Has it been collected
    private Vector3 InitialPosition;
    private string PlayerPrefKey;
    private Vector3 CollectedPosition;
    public float AnimationTime;

    private void Awake()
    {
        // Keep track of it being collected in player prefs
        PlayerPrefKey = "Collectable_" + this.gameObject.name;
    }

    private void Start()
    {
        // Disable physics
        PhysicsEnable(false);
        CollectedPosition = this.transform.position;

        if (Collected)
        {
            transform.localScale = Vector3.one * CollectedScale;
            AnimationTime = CollectAnimationSeconds;
        }
        else
        {
            Hide();
        }
    }

    public void Hide()
    {
        // Start hidden
        Sprite = GetComponent<SpriteRenderer>();
        if (Sprite != null)
        {
            Sprite.enabled = false;
        }

        // Notify mesh to hide
        if (OnHide != null)
        {
            OnHide(this, null);
        }
        AnimationTime = 0;
    }

    private void PhysicsEnable(bool enable)
    {
        this.gameObject.GetComponent<Rigidbody2D>().bodyType = enable ? RigidbodyType2D.Dynamic : RigidbodyType2D.Static;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = enable;
    }

    public void Update()
    {
        // Animation
        if (Collected && AnimationTime <= CollectAnimationSeconds)
        {
            AnimationTime += Time.deltaTime;
            var pct = Mathf.Clamp(AnimationTime / CollectAnimationSeconds, 0, 1);
            float scale = InitialScale + ((CollectedScale - InitialScale) * pct);
            transform.localScale = new Vector3(scale, scale, scale);
            transform.position = Vector3.Lerp(InitialPosition, CollectedPosition, pct);
        }
    }

    public void DropFrom(Vector3 dropPos)
    {
        this.transform.localScale = new Vector3(InitialScale, InitialScale, InitialScale);

        // Drop sound
        this.GetComponent<AudioSource>().PlayOneShot(DropSound);

        // Enable physics
        PhysicsEnable(true);
        this.transform.position = dropPos;
        if (Sprite != null)
        {
            Sprite.enabled = true;
        }
        if (OnShow != null)
        {
            OnShow(this, null);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bomb"))
        {
            DestroyedByBomb();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Collect();
        }
    }

    public void DestroyedByBomb()
    {
        // Notify
        if (OnDestroyed != null)
        {
            OnDestroyed(this, null);
        }

        PlayerScript.Instance.Damage(10);//damage player when missed collectable

        // todo - fail sound and/or graphic
        Destroy(gameObject);
    }

    public bool Collected
    {
        get
        {
            return PlayerPrefs.GetInt(PlayerPrefKey) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(PlayerPrefKey, value ? 1 : 0);
        }
    }

    public void Collect()
    {
        // Keep track of it being collected in player prefs
        Collected = true;

        AnimationTime = 0;
        InitialScale = transform.localScale.x;
        InitialPosition = transform.position;

        // Disable physics
        PhysicsEnable(false);

        // Play sound
        this.GetComponent<AudioSource>().PlayOneShot(CollectedSound);

        // Notify
        if (OnCollected != null)
        {
            OnCollected(this, null);
        }
    }
}
