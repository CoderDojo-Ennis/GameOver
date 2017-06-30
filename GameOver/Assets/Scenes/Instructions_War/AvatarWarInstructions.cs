using UnityEngine;

public class AvatarWarInstructions : MonoBehaviour
{
    private Animator Anim;
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
        int idleTrigger = Animator.StringToHash("IdleTrigger");
        int walkRightTrigger = Animator.StringToHash("WalkRightTrigger");
        this.Anim = GetComponent<Animator>();

        this.Delay(1f, () =>
        {
            Anim.SetTrigger(walkRightTrigger);
            Avatar.GlideX(-1.5f, 1.5f, 2).Then(() =>
            {
                Anim.SetTrigger(idleTrigger);
            });
            this.Delay(1.8f, () =>
            {
                Coin.SetActive(false);
                AudioSource.Play();
            });
        });
    }
}
