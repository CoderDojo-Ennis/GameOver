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
        GetComponent<SpriteRenderer>().enabled = false;
        CollectedPosition = this.transform.position;
        this.transform.localScale = new Vector3(InitialScale, InitialScale, InitialScale);

        // Disable physics
        this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
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
        this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        this.transform.position = dropPos;
        GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bomb")
        {
            DestroyedByBomb();
        }
    }

    public void DestroyedByBomb()
    {
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
        this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;

        // Play sound
        this.GetComponent<AudioSource>().PlayOneShot(CollectedSound);
    }
}
