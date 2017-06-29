using System;
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
    public bool Collected = false;
    private string PlayerPrefKey;
    private Vector3 CollectedPosition;
    public float AnimationTime;

    private void Start()
    {
        // Keep track of it being collected in player prefs
        PlayerPrefKey = "Collectable_" + this.gameObject.name;
        PlayerPrefs.SetInt(PlayerPrefKey, 0);

        // Start hidden
        Sprite = GetComponent<SpriteRenderer>();
        if (Sprite != null)
        {
            Sprite.enabled = false;
        }
        CollectedPosition = this.transform.position;
        this.transform.localScale = new Vector3(InitialScale, InitialScale, InitialScale);

        // Disable physics
        PhysicsEnable(false);

        // Notify mesh to hide
        if (OnHide != null)
        {
            OnHide(this, null);
        }
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
        if (collision.gameObject.tag == "Bomb")
        {
            DestroyedByBomb();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
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

        // todo - fail sound and/or graphic
        Destroy(gameObject);
    }

    public void Collect()
    {
        Collected = true;
        AnimationTime = 0;
        InitialScale = transform.localScale.x;
        InitialPosition = transform.position;

        // Keep track of it being collected in player prefs
        PlayerPrefs.SetInt(PlayerPrefKey, 0);

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
