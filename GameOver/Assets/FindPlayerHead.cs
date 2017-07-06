using UnityEngine;

public class FindPlayerHead : MonoBehaviour
{

    public float MetersToWorldUnits = .5f;
    public float UserHeight;
    public Transform AimTowards;
    public Transform RaftTransform;
    public Transform PlayerFootTransform;

    private void FixedUpdate()
    {
        if (BodySlicer.Instance != null)
        {
            UserHeight = BodySlicer.Instance.getUserHeight();
        }

        PlayerFootTransform.localRotation = RaftTransform.rotation;

        if (UserHeight > 0.1)
        {
            float scaledHeight = UserHeight * MetersToWorldUnits;
            AimTowards.localPosition = new Vector3(AimTowards.localPosition.x, scaledHeight, AimTowards.localPosition.z);
        }
    }
}
