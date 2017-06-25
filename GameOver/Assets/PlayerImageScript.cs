using UnityEngine;

public class PlayerImageScript : MonoBehaviour
{
    private MeshRenderer MeshRenderer;

    public void Awake()
    {
        this.MeshRenderer = GetComponent<MeshRenderer>();
    }


    void Update()
    {
        // Render the kinect color image onto a quad
        BackgroundRemovalManager backManager = BackgroundRemovalManager.Instance;

        if (backManager && backManager.IsBackgroundRemovalInitialized())
        {
            var tex = backManager.GetForegroundTex();
            MeshRenderer.material.SetTexture("_MainTex", tex);
        }
    }
}
