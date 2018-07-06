using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterSeaScript : BaseGameScene
{
    [Header("Boat Guard")]
    public SpriteRenderer BoatGuard;
    public Sprite BoatGuardIdle;
    public Sprite BoatGuardWelcome;
    private AvatarScript Avatar;

    [Header("Boat")]
    public Transform Boat;
    public Transform BoatStartPos;
    public Transform BoatEndPos;
    public float BoatSeconds;
    public Vector3 PlayerBoatOffset;

    public new void Awake ()
    {
        Avatar = GetComponentInChildren<AvatarScript>();
        SceneRequiresPlayer = false;
    }

    public override void FirstUpdate()
    {
        GameManager.Instance.ActiveGameScene = this;
        PlayerScript.Instance.HideKinect(0);
        FadeCameraIn();
        if (BackgroundMusic != null)
        {
            GameManager.Instance.PlayBackgroundMusic(BackgroundMusic);
        }
        BoatGuard.sprite = BoatGuardIdle;
        GetComponentInChildren<ChildScript>().StartFollowing(Avatar.transform);
        GlideBoat().Then(() => {
            this.Delay(1, () => {
                Avatar.SetAnimation("WalkRight");
                Avatar.GlideX(-4.5f, 8, 3).Then(() => {
                    FadeToScene("Instructions_LandScene");
                });
            });
            this.Delay(2, () => {
                BoatGuard.sprite = BoatGuardWelcome;
            });
        });
    }

    private GmDelayPromise GlideBoat()
    {
        int steps = 80;
        float step = 0;

        Boat.localPosition = BoatStartPos.localPosition;
        Avatar.transform.localPosition = BoatStartPos.localPosition + PlayerBoatOffset;

        return this.Repeat(BoatSeconds / steps, steps, () =>
        {
            step++;
            Boat.localPosition = Vector3.Lerp(BoatStartPos.localPosition, BoatEndPos.localPosition, step / steps);
            Avatar.transform.localPosition = Boat.localPosition + PlayerBoatOffset;
        }, true);
    }

}
