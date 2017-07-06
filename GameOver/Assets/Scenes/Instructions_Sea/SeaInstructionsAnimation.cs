using UnityEngine;

public class SeaInstructionsAnimation : MonoBehaviour
{
    [Header("Transitions")]
    public float SceneStartDelay = 1f;

    [Header("Boat")]
    public GameObject Boat;

    [Header("Avatar")]
    public AvatarScript Avatar;
    public float LeanAtAngle = 25;

    [Header("Waves")]
    public float WaveMaxAngle = 45;
    public float WaveSpeed = 2;
    public float WaveHeight = 2;

    private float BoatStartY;
    public float BoatAngle;
    private float SceneTime = 0;

    // Use this for initialization
    void Start()
    {
        BoatStartY = Boat.transform.localPosition.y;

        // Start animation
        this.Delay(SceneStartDelay, () =>
        {
        });
    }

    // Todo - make this an extension method
    float AngleConvert(float f)
    {
        if (f > 180)
        {
            return (360 - f) * -1;
        }
        else
        {
            return f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        SceneTime += Time.unscaledDeltaTime;

        // Move the boat
        BoatAngle = WaveMaxAngle * Mathf.Sin(SceneTime * WaveSpeed);
        Boat.transform.localEulerAngles = new Vector3(0, 0, BoatAngle);
        Boat.transform.localPosition = new Vector3(Boat.transform.localPosition.x, BoatStartY - WaveHeight * Mathf.Sin(SceneTime * WaveSpeed + Mathf.PI * .25f), Boat.transform.position.z);

        // Avatar lean
        string animation = "Idle";
        if (BoatAngle >= LeanAtAngle)
        {
            animation = "LeanRight";
        }
        else if (BoatAngle <= -LeanAtAngle)
        {
            animation = "LeanLeft";
        }
        if (Avatar.CurrentAnimation != animation)
        {
            Avatar.SetAnimation(animation);
        }
    }
}
