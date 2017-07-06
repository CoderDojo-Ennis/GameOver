using System;
using UnityEngine;
//using Windows.Kinect;

public class GameGestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface
{
    [Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
    public int playerIndex = 0;

    /// <summary>
    /// Is the player currently detected
    /// </summary>
    [HideInInspector]
    public bool IsPlayerDetected;

    // singleton instance of the class
    //private static GameGestureListener instance = null;

    // Events
    public event EventHandler OnUserDetected;
    public event EventHandler OnUserLost;
    public event EventHandler OnSwipeLeft;
    public event EventHandler OnSwipeRight;
    public event EventHandler OnSwipeUp;
    public event EventHandler OnSwipeDown;
    public event EventHandler OnOneHandUp;

    /// <summary>
    /// Gets the singleton CubeGestureListener instance.
    /// </summary>
    /// <value>The CubeGestureListener instance.</value>
    //public static GameGestureListener Instance
    //{
    //    get
    //    {
    //        return instance;
    //    }
    //}

    /// <summary>
    /// Invoked when a new user is detected. Here you can start gesture tracking by invoking KinectManager.DetectGesture()-function.
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="userIndex">User index</param>
    public void UserDetected(long userId, int userIndex)
    {
        //Debug.Log("User Detected " + userIndex);

        // the gestures are allowed for the primary user only
        KinectManager manager = KinectManager.Instance;
        if (!manager || (userIndex != playerIndex))
        {
            return;
        }

        IsPlayerDetected = true;

        // detect these user specific gestures
        manager.DetectGesture(userId, KinectGestures.Gestures.SwipeLeft);
        manager.DetectGesture(userId, KinectGestures.Gestures.SwipeRight);
        manager.DetectGesture(userId, KinectGestures.Gestures.SwipeUp);
        manager.DetectGesture(userId, KinectGestures.Gestures.SwipeDown);
        manager.DetectGesture(userId, KinectGestures.Gestures.RaiseLeftHand);
        manager.DetectGesture(userId, KinectGestures.Gestures.RaiseRightHand);

        if (OnUserDetected != null)
        {
            OnUserDetected(this, null);
        }
    }

    /// <summary>
    /// Invoked when a user gets lost. All tracked gestures for this user are cleared automatically.
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="userIndex">User index</param>
    public void UserLost(long userId, int userIndex)
    {
        //Debug.Log("UserLost " + userIndex);

        // the gestures are allowed for the primary user only
        if (userIndex != playerIndex)
        {
            return;
        }

        IsPlayerDetected = false;

        if (OnUserLost != null)
        {
            OnUserLost(this, null);
        }
    }

    /// <summary>
    /// Invoked when a gesture is in progress.
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="userIndex">User index</param>
    /// <param name="gesture">Gesture type</param>
    /// <param name="progress">Gesture progress [0..1]</param>
    /// <param name="joint">Joint type</param>
    /// <param name="screenPos">Normalized viewport position</param>
    public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture,
                                  float progress, KinectInterop.JointType joint, Vector3 screenPos)
    {
        // the gestures are allowed for the primary user only
        if (userIndex != playerIndex)
        {
            return;
        }

        /*
        if ((gesture == KinectGestures.Gestures.ZoomOut || gesture == KinectGestures.Gestures.ZoomIn) && progress > 0.5f)
        {
            if (gestureInfo != null)
            {
                string sGestureText = string.Format("{0} - {1:F0}%", gesture, screenPos.z * 100f);
                gestureInfo.text = sGestureText;

                progressDisplayed = true;
                progressGestureTime = Time.realtimeSinceStartup;
            }
        }
        else if ((gesture == KinectGestures.Gestures.Wheel || gesture == KinectGestures.Gestures.LeanLeft ||
                 gesture == KinectGestures.Gestures.LeanRight) && progress > 0.5f)
        {
            if (gestureInfo != null)
            {
                string sGestureText = string.Format("{0} - {1:F0} degrees", gesture, screenPos.z);
                gestureInfo.text = sGestureText;

                progressDisplayed = true;
                progressGestureTime = Time.realtimeSinceStartup;
            }
        }
        else if (gesture == KinectGestures.Gestures.Run && progress > 0.5f)
        {
            if (gestureInfo != null)
            {
                string sGestureText = string.Format("{0} - progress: {1:F0}%", gesture, progress * 100);
                gestureInfo.text = sGestureText;

                progressDisplayed = true;
                progressGestureTime = Time.realtimeSinceStartup;
            }
        }
        */
    }

    /// <summary>
    /// Invoked if a gesture is completed.
    /// </summary>
    /// <returns>true</returns>
    /// <c>false</c>
    /// <param name="userId">User ID</param>
    /// <param name="userIndex">User index</param>
    /// <param name="gesture">Gesture type</param>
    /// <param name="joint">Joint type</param>
    /// <param name="screenPos">Normalized viewport position</param>
    public bool GestureCompleted(long userId, int userIndex, KinectGestures.Gestures gesture,
                                  KinectInterop.JointType joint, Vector3 screenPos)
    {
        //Debug.Log("Gesture " + gesture + " for user " + userIndex);

        // the gestures are allowed for the primary user only
        if (userIndex != playerIndex)
        {
            return false;
        }

        if (gesture == KinectGestures.Gestures.SwipeLeft)
        {
            if (OnSwipeLeft != null)
            {
                OnSwipeLeft(this, null);
            }
        }
        else if (gesture == KinectGestures.Gestures.SwipeRight)
        {
            if (OnSwipeRight != null)
            {
                OnSwipeRight(this, null);
            }
        }
        else if (gesture == KinectGestures.Gestures.SwipeUp)
        {
            if (OnSwipeUp != null)
            {
                OnSwipeUp(this, null);
            }
        }
        else if (gesture == KinectGestures.Gestures.SwipeDown)
        {
            if (OnSwipeDown != null)
            {
                OnSwipeDown(this, null);
            }
        }
        else if (gesture == KinectGestures.Gestures.RaiseLeftHand || gesture == KinectGestures.Gestures.RaiseRightHand)
        {
            if (OnOneHandUp != null)
            {
                OnOneHandUp(this, null);
            }
        }

        return true;
    }

    /// <summary>
    /// Invoked if a gesture is cancelled.
    /// </summary>
    /// <returns>true</returns>
    /// <c>false</c>
    /// <param name="userId">User ID</param>
    /// <param name="userIndex">User index</param>
    /// <param name="gesture">Gesture type</param>
    /// <param name="joint">Joint type</param>
    public bool GestureCancelled(long userId, int userIndex, KinectGestures.Gestures gesture,
                                  KinectInterop.JointType joint)
    {
        // the gestures are allowed for the primary user only
        if (userIndex != playerIndex)
        {
            return false;
        }

        return true;
    }

    void Awake()
    {
        //instance = this;
    }

    void Update()
    {
        KeyboardEmulateKinectEvents();
    }

    private void KeyboardEmulateKinectEvents()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            if (OnSwipeLeft != null)
            {
                OnSwipeLeft(this, null);
            }
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            if (OnSwipeRight != null)
            {
                OnSwipeRight(this, null);
            }
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            if (OnSwipeUp != null)
            {
                OnSwipeUp(this, null);
            }
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            if (OnOneHandUp != null)
            {
                OnOneHandUp(this, null);
            }
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            if (OnSwipeDown != null)
            {
                OnSwipeDown(this, null);
            }
        }
        else if (Input.GetKeyUp(KeyCode.I))
        {
            if (OnUserDetected != null)
            {
                OnUserDetected(this, null);
            }
        }
        else if (Input.GetKeyUp(KeyCode.O))
        {
            if (OnUserLost != null)
            {
                OnUserLost(this, null);
            }
        }
    }
}
