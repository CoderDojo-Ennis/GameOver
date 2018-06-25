using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalAnimationScript : BaseGameScene
{
    private AvatarScript Avatar;
    private TrumpScript Trump;
    private ChildScript Child;

    public new void Awake()
    {
        SceneRequiresPlayer = false;
        Avatar = GetComponentInChildren<AvatarScript>();
        Trump = GetComponentInChildren<TrumpScript>();
        Child = GetComponentInChildren<ChildScript>();
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
                        Child.transform.localScale = new Vector3(-Child.transform.localScale.x, Child.transform.localScale.y, Child.transform.localScale.z);
                        Child.transform.SetParent(Trump.transform);
                        Trump.GlideX(Trump.transform.localPosition.x, 10, 4).Then(() => {
                            GameManager.Instance.FadeToScene("GameOverScene", 2);
                        });
                    });
                });
            });
        });
    }
}
