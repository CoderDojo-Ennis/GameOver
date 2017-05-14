using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TitlePresentationScript : MonoBehaviour
{
    [Tooltip("Camera used for screen-to-world calculations. This is usually the main camera.")]
    public Camera screenCamera;

    public GameObject StepForwardObject;

    private TitleGestureListener gestureListener;

    void Start()
    {
        // hide mouse cursor
        //Cursor.visible = false;

        // by default set the main-camera to be screen-camera
        if (screenCamera == null)
        {
            screenCamera = Camera.main;
        }

        // get the gestures listener
        gestureListener = TitleGestureListener.Instance;
        gestureListener.OnUserDetected += (sender, args) => { UserFound(); };
        gestureListener.OnUserLost += (sender, args) => { UserLost(); };

        // Start with no user
        UserLost();
    }

    void Update()
    {
        // dont run Update() if there is no gesture listener
        if (!gestureListener)
        {
            return;
        }
    }

    public void UserFound()
    {
        StepForwardObject.SetActive(false);
    }

    public void UserLost()
    {
        StepForwardObject.SetActive(true);
    }
}
