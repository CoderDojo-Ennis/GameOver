using UnityEngine;

public class AvatarSeaInstructions : MonoBehaviour
{
    private AvatarScript Avatar;
    public GameObject Coin;
    private AudioSource AudioSource;

    private void Awake()
    {
        this.Avatar = GetComponent<AvatarScript>();
        this.AudioSource = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {
        this.Delay(1f, () =>
        {
            Avatar.SetAnimation("WalkRight");
            Avatar.GlideX(-1.5f, 1.5f, 2).Then(() =>
            {
                Avatar.SetAnimation("Idle");
            });
            this.Delay(1.8f, () =>
            {
                Coin.SetActive(false);
                AudioSource.Play();
            });
        });
    }
}
