using UnityEngine;
using AuraAPI;

public class LandInstructionsAnimation : MonoBehaviour
{
    public Transform Guard;
    public AvatarScript Avatar;
    public Transform PlayerPositions;
    private Vector3 PlayerPosLeft;
    private Vector3 PlayerPosMiddle;
    private Vector3 PlayerPosRight;
    public Transform Spotlight;
    public float SceneStartDelay = 1f;
    //public SpriteRenderer LightColumn;
    private bool GuardWalking = false;
    private bool SpotlightMoving = true;

    // Use this for initialization
    void Start()
    {
        GetComponent<InstructionsMenu>().SceneRequiresPlayer = false;
        // Full health
        PlayerScript.Instance.SetInitialHealth();
        Spotlight.rotation = Quaternion.Euler(155, 100, 0);

        // Start animation
        /*
        this.Delay(SceneStartDelay, () =>
        {
            LightColumn.FadeAlpha(this, LightColumn.color.a, 0, 2f, false);
        });
        */
        PlayerPosLeft = PlayerPositions.Find("Left").position;
        PlayerPosMiddle = PlayerPositions.Find("Middle").position;
        PlayerPosRight = PlayerPositions.Find("Right").position;
        Avatar.transform.position = PlayerPosMiddle;

        foreach (AuraLight light in LandScene.FindObjectsOfTypeAll<AuraLight>())
        {
            light.enabled = true; //sometimes the lights just randomly disable for some reason
        }

        this.Delay(5, () => { PlayerRun(); });
        this.Delay(15, () => { SpotlightMoving = false; });
    }

    private void Update()
    {
        if (SpotlightMoving)
        {
            Spotlight.Rotate(0, -15 * Time.deltaTime, 0, Space.World);
        }
        if (GuardWalking)
        {
            Guard.Translate(-2 * Time.deltaTime, 0, 0);
        }
    }

    void PlayerRun()
    {
        Avatar.SetAnimation("WalkRight");
        Avatar.GlideX(Avatar.transform.localPosition.x, PlayerPosRight.x, 0.5f).Then(() => {
            Avatar.SetAnimation("Crouch");
            GuardWalking = true;
            this.Delay(3, () => {
                Avatar.SetAnimation("WalkLeft");
                Avatar.GlideX(Avatar.transform.localPosition.x, PlayerPosLeft.x, 2).Then(() => {
                    Avatar.SetAnimation("Crouch");
                    this.Delay(3, () => { GameManager.Instance.FadeToScene("LandScene", 2); });
                });
            });
        });
    }
}
