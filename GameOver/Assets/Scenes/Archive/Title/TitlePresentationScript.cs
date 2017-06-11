using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TitlePresentationScript : MonoBehaviour
{
    [Tooltip("Camera used for screen-to-world calculations. This is usually the main camera.")]
    public Camera screenCamera;

    public GameObject StepForwardObject;
    public float StepForwardPulseSpeed = 3f;
    public float StepForwardPulseIntensity = 2f;
    public float CarouselRotationSpeed = 100f;
    private Vector3 StepForwardActive;
    private Vector3 StepForwardInactive;
    private Vector3 StepForwardTarget;

    public AudioSource SoundUserFound;
    public AudioSource SoundUserLost;

    public GameObject LevelCarousel;
    private Vector3 LevelCarouselDown;
    private Vector3 LevelCarouselUp;
    private Vector3 LevelCarouselTarget;
    public float CarouselTargetRotation;
    public float CarouselCurrentRotation;

    private TitleGestureListener gestureListener;

    private int LevelSelected = 1;
    private int MaxLevel = 3;

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
        gestureListener.OnSwipeUp += (sender, args) => { StartLevel(); };
        gestureListener.OnSwipeLeft += (sender, args) => { LevelUp(); };
        gestureListener.OnSwipeRight += (sender, args) => { LevelDown(); };

        // Start with no user
        StepForwardActive = StepForwardObject.transform.position;
        StepForwardInactive = StepForwardActive + new Vector3(0, 0, -30);
        StepForwardTarget = StepForwardActive;

        LevelCarouselUp = transform.position;
        LevelCarouselDown = transform.position + new Vector3(0, -28, 0);
        transform.position = LevelCarouselDown;
        LevelCarouselTarget = LevelCarouselDown;
        CarouselTargetRotation = 0;
    }

    void LevelUp()
    {
        LevelSelected += 1;
        if (LevelSelected > MaxLevel)
        {
            LevelSelected = 1;
        }

        CarouselTargetRotation += 360 / MaxLevel;
    }

    void LevelDown()
    {
        LevelSelected -= 1;
        if (LevelSelected < 1)
        {
            LevelSelected = MaxLevel;
        }

        CarouselTargetRotation -= 360 / MaxLevel;
    }

    void StartLevel()
    {
        Debug.Log("ToDo - Start");
        LevelCarouselTarget = LevelCarouselDown;
    }

    void Update()
    {
        // dont run Update() if there is no gesture listener
        if (!gestureListener)
        {
            return;
        }

        CarouselCurrentRotation = Mathf.MoveTowards(CarouselCurrentRotation, CarouselTargetRotation, Time.deltaTime * CarouselRotationSpeed);
        LevelCarousel.transform.rotation = Quaternion.Euler(new Vector3(0, CarouselCurrentRotation, 0));

        transform.position = Vector3.MoveTowards(transform.position, LevelCarouselTarget, Time.deltaTime * 20);
        StepForwardObject.transform.position = Vector3.MoveTowards(StepForwardObject.transform.position, StepForwardTarget + new Vector3(0, 0, Mathf.Sin(Time.time * StepForwardPulseSpeed) * StepForwardPulseIntensity), Time.deltaTime * 20);
    }

    public void UserFound()
    {
        SoundUserFound.Play();
        StepForwardTarget = StepForwardInactive;
        LevelCarouselTarget = LevelCarouselUp;
    }

    public void UserLost()
    {
        SoundUserLost.Play();
        StepForwardTarget = StepForwardActive;
        LevelCarouselTarget = LevelCarouselDown;
    }
}
