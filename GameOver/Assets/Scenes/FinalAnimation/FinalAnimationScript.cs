using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalAnimationScript : BaseGameScene
{
    public VideoPlaylists VideoPlaylist;
    public SpriteRenderer Guard;
    public Sprite GuardIdle;
    public Sprite GuardPoint;
    private Camera cam;
    private AvatarScript Avatar;
    private TrumpScript Trump;
    private ChildScript Child;

    public new void Awake()
    {
        SceneRequiresPlayer = false;
        Avatar = GetComponentInChildren<AvatarScript>();
        Trump = GetComponentInChildren<TrumpScript>();
        Child = GetComponentInChildren<ChildScript>();
        cam = GetComponentInChildren<Camera>();
    }

    public override void FirstUpdate()
    {
        this.Delay(.01f, () =>
        {
            GameManager.Instance.SetTimeScale(0);
            if (VideoPlaylist != VideoPlaylists.None)
            {
                PlayNextPlyalistVideo(VideoPlaylist).Then(() =>
                {
                    GameManager.Instance.SetTimeScale(1);
                    GameManager.Instance.FadeCameraIn(1, cam);
                });
            }
            else
            {
                GameManager.Instance.SetTimeScale(1);
            }
        }, true);

        GameManager.Instance.ActiveGameScene = this;
        PlayerScript.Instance.HideKinect(0);
        Guard.sprite = GuardIdle;
        FadeCameraIn();
        if (BackgroundMusic != null)
        {
            GameManager.Instance.PlayBackgroundMusic(BackgroundMusic);
        }

        this.Delay(1, () =>
        {
            Child.SetAnimation("ChildWalk");
            Avatar.SetAnimation("WalkRight");
            Child.GlideX(-9.5f, -5.1f, 2);
            Avatar.GlideX(-8.8f, -4.4f, 2).Then(() =>
            {
                Child.SetAnimation("ChildIdle");
                Avatar.SetAnimation("IdleRight");
                this.Delay(1, () =>
                {
                    Trump.SetAnimation("TrumpWalk");
                    Trump.transform.localScale = new Vector3(-Trump.transform.localScale.x, Trump.transform.localScale.y, Trump.transform.localScale.z);
                    Trump.GlideX(Trump.transform.localPosition.x, Child.transform.localPosition.x - Trump.GetComponent<SpriteRenderer>().sprite.bounds.size.x * Trump.transform.localScale.x, 3).Then(() =>
                    {
                        Trump.SetAnimation("TrumpWalkArm");
                        Trump.transform.localScale = new Vector3(-Trump.transform.localScale.x, Trump.transform.localScale.y, Trump.transform.localScale.z);
                        Child.SetAnimation("ChildTaken");
                        Child.Cry();
                        Child.transform.localScale = new Vector3(-Child.transform.localScale.x, Child.transform.localScale.y, Child.transform.localScale.z);
                        Child.transform.SetParent(Trump.transform);
                        Trump.StartMoving(2);
                        this.Delay(1, () =>
                        {
                            Avatar.SetAnimation("Cry");
                            this.Delay(4, () =>
                            {
                                Guard.sprite = GuardPoint;
                                this.Delay(1, () =>
                                {
                                    //Avatar.transform.localScale = new Vector3(-Avatar.transform.localScale.x, Avatar.transform.localScale.y, Avatar.transform.localScale.z);
                                    Avatar.SetAnimation("WalkCry");
                                    Avatar.GlideX(Avatar.transform.localPosition.x, -8.8f, 2).Then(() =>
                                    {
                                        GameManager.Instance.FadeToScene("GameOverScene", 2);
                                    });
                                });
                            });
                        });
                        //Trump.GlideX(Trump.transform.localPosition.x, 10, 4).Then(() => {
                        //GameManager.Instance.FadeToScene("GameOverScene", 2);
                        //});
                    });
                });
            });
        });
    }
}
