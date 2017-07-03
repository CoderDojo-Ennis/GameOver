using UnityEngine;

public class PlayerImageScript : MonoBehaviour
{
    private MeshRenderer MeshRenderer;

    private PlayerScript Player;

    private void Awake()
    {
        Player = gameObject.transform.parent.GetComponent<PlayerScript>();
        this.MeshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        transform.localPosition = Player.JointOffset;
        transform.localScale = new Vector3(Player.AspectScale.x * Player.JointScale, -Player.AspectScale.y * Player.JointScale, Player.AspectScale.z * Player.JointScale);

        // Render the kinect color image onto a quad
        BackgroundRemovalManager backManager = BackgroundRemovalManager.Instance;

        if (backManager && backManager.IsBackgroundRemovalInitialized())
        {
            var tex = backManager.GetForegroundTex();
            MeshRenderer.material.SetTexture("_MainTex", tex);
        }
    }

    public GmDelayPromise FadeOut(float seconds)
    {
        return MeshRenderer.material.Fade(this, MeshRenderer.material.color, new Color(1, 1, 1, 0), seconds, true);
    }

    public GmDelayPromise FadeIn(float seconds)
    {
        return MeshRenderer.material.Fade(this, MeshRenderer.material.color, new Color(1, 1, 1, 1), seconds, true);
    }

    public void ShowDamaged()
    {
        float damageTime = 0.2f;
        MeshRenderer.material.Fade(this, MeshRenderer.material.color, new Color(1, 0, 0, 1), damageTime, true).Then(() =>
        {
            MeshRenderer.material.Fade(this, new Color(1, 0, 0, 1), new Color(1, 1, 1, 1), damageTime, true);
        });
    }
}
