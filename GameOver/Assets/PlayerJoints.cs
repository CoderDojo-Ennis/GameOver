using UnityEngine;

public class PlayerJoints : MonoBehaviour
{
    //[Tooltip("GUI-texture used to display the color camera feed on the scene background.")]
    //public GUITexture backgroundImage;

    //[Tooltip("Camera used to estimate the overlay positions of 3D-objects over the background. By default it is the main camera.")]
    //public Camera foregroundCamera;

    [Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
    public int playerIndex = 0;

    public Transform leftHandOverlay;
    public Transform rightHandOverlay;
    public Transform rightFoot;
    public Transform leftFoot;
    public Transform head;
    public Transform spineBase;
    public Transform rightElbow;
    public Transform leftElbow;
    public Transform rightKnee;
    public Transform leftKnee;

    //public float smoothFactor = 10f;

    // reference to KinectManager
    private KinectManager manager;

    private PlayerScript Player;

    public float colImageHeight;
    public float colImageWidth;

    public float ScaleZ = .001f;
    //public float FloorY = 0;
    public float XPercent;

    private void Awake()
    {
        Player = gameObject.GetComponent<PlayerScript>();
    }

    void Update()
    {
        //if (foregroundCamera == null)
        //{
        //    // by default use the main camera
        //    foregroundCamera = Camera.main;
        //}

        if (manager == null)
        {
            manager = KinectManager.Instance;
        }

        if (manager && manager.IsInitialized() /*&& foregroundCamera*/)
        {
            //backgroundImage.renderer.material.mainTexture = manager.GetUsersClrTex();
            //if (backgroundImage && (backgroundImage.texture == null))
            //{
            //    backgroundImage.texture = manager.GetUsersClrTex();
            //}

            // get the background rectangle (use the portrait background, if available)
            //Rect backgroundRect = foregroundCamera.pixelRect;
            //PortraitBackground portraitBack = PortraitBackground.Instance;

            //if (portraitBack && portraitBack.enabled)
            //{
            //    backgroundRect = portraitBack.GetBackgroundRect();
            //}

            if (colImageWidth == 0)
            {
                colImageWidth = manager.GetColorImageWidth();
            }
            if (colImageHeight == 0)
            {
                colImageHeight = manager.GetColorImageHeight();
            }

            // overlay the joints
            if (manager.IsUserDetected(playerIndex))
            {
                long userId = manager.GetUserIdByIndex(playerIndex);
                var spineBaseUnscaled = GetJointPointUnscaled(userId, KinectInterop.JointType.SpineBase);
                var leftFootUnscaled = GetJointPointUnscaled(userId, KinectInterop.JointType.FootLeft);
                var rightFootUnscaled = GetJointPointUnscaled(userId, KinectInterop.JointType.FootRight);

                // Center the player sprite between the feet
                if (leftFootUnscaled.HasValue && rightFootUnscaled.HasValue)
                {
                    // Prefer spine base x
                    float x;
                    if (Player.NaturalX)
                    {
                        x = 0;
                    }
                    else
                    {
                        if (spineBaseUnscaled.HasValue)
                        {
                            x = (spineBaseUnscaled.Value.x + leftFootUnscaled.Value.x + rightFootUnscaled.Value.x) / 3;
                        }
                        else
                        {
                            x = (leftFootUnscaled.Value.x + rightFootUnscaled.Value.x) / 2;
                        }
                    }
                    Vector3 centerFootUnscaled = new Vector3(x, Mathf.Min(leftFootUnscaled.Value.y, rightFootUnscaled.Value.y), (leftFootUnscaled.Value.z + rightFootUnscaled.Value.z) / 2);
                    Player.JointOffset = Vector3.Lerp(Player.JointOffset, -centerFootUnscaled * Player.JointScale, .1f);
                }

                // Move the player sprite on X
                if (Player.NaturalX)
                {
                    //Player.SetX(-Player.JointOffset.x);
                }
                else
                {
                    if (spineBaseUnscaled.HasValue)
                    {
                        XPercent = spineBaseUnscaled.Value.x;
                        Player.SetX(XPercent * Player.TravelScaleX);
                    }
                }

                // Position the colliders
                OverlayJoint(userId, KinectInterop.JointType.HandLeft, leftHandOverlay);
                OverlayJoint(userId, KinectInterop.JointType.HandRight, rightHandOverlay);
                OverlayJoint(userId, KinectInterop.JointType.Head, head);
                OverlayJoint(userId, KinectInterop.JointType.FootLeft, leftFoot);
                OverlayJoint(userId, KinectInterop.JointType.FootRight, rightFoot);
                OverlayJoint(userId, KinectInterop.JointType.ElbowLeft, leftElbow);
                OverlayJoint(userId, KinectInterop.JointType.ElbowRight, rightElbow);
                OverlayJoint(userId, KinectInterop.JointType.KneeLeft, leftKnee);
                OverlayJoint(userId, KinectInterop.JointType.KneeRight, rightKnee);
                OverlayJoint(userId, KinectInterop.JointType.SpineBase, spineBase);
            }

        }
    }

    private Vector3? GetJointPointUnscaled(long userId, KinectInterop.JointType jointIndex)
    {
        Vector3? pos = null;

        if (manager.IsJointTracked(userId, (int)jointIndex))
        {
            Vector3 posJoint = manager.GetJointKinectPosition(userId, (int)jointIndex);

            if (posJoint != Vector3.zero)
            {
                // 3d position to depth
                Vector2 posDepth = manager.MapSpacePointToDepthCoords(posJoint);
                ushort depthValue = manager.GetDepthForPixel((int)posDepth.x, (int)posDepth.y);

                if (depthValue > 0 && colImageWidth > 0 && colImageHeight > 0)
                {
                    // depth pos to color pos
                    Vector2 posColor = manager.MapDepthPointToColorCoords(posDepth, depthValue);

                    float xNorm = (float)posColor.x / colImageWidth - .5f;
                    float yNorm = 1.0f - (float)posColor.y / colImageHeight - .5f;

                    pos = new Vector3(xNorm, yNorm, depthValue * ScaleZ);
                }
            }
        }
        return pos;
    }

    private void OverlayJoint(long userId, KinectInterop.JointType jointIndex, Transform overlayObj)
    {
        if (overlayObj)
        {
            Vector3? jointPointUnscaled = GetJointPointUnscaled(userId, jointIndex);

            if (jointPointUnscaled.HasValue)
            {
                overlayObj.localPosition = jointPointUnscaled.Value * Player.JointScale + Player.JointOffset;
            }
        }
    }
}
