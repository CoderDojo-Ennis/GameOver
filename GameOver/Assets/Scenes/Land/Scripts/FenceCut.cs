using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FenceCut : MonoBehaviour
{
    public Vector3 Center;
    public float Radius = 2;
    public CutterScript BoltCutters;
    public Sprite BoltCutter1;
    public Sprite BoltCutter2;
    public Slider ProgressSlider;
    public float CutSpeed;
    public float BoltCutterAnimationSpeed;    //Lower numbers are faster
    private int FrameCounter;

	void Start ()
    {
        ProgressSlider.value = ProgressSlider.maxValue;
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ProgressSlider.value -= Time.fixedDeltaTime * CutSpeed;
            if (ProgressSlider.value == 0)
            {
                LandScene.instance.Win();
                gameObject.SetActive(false);
            }
            float percent = (ProgressSlider.value / 100) * 2 * Mathf.PI;
            float FramesToFinish = ProgressSlider.maxValue / (CutSpeed * Time.deltaTime);
            //BoltCutters.transform.Rotate(0, 0, (360 / FramesToFinish));
            //BoltCutters.transform.Translate(0, 0.005f, 0, Space.Self);
            BoltCutters.transform.rotation = Quaternion.Euler(0, 0, -percent * Mathf.Rad2Deg + 90);
            BoltCutters.transform.position = new Vector3(Center.x + Mathf.Sin(percent) * Radius, Center.y + Mathf.Cos(percent) * Radius, Center.z);
            FrameCounter++;
            if (FrameCounter == BoltCutterAnimationSpeed)
            {
                ToggleBoltCutterState();
                FrameCounter = 0;
            }
        }
    }

    private void ToggleBoltCutterState()
    {
        SpriteRenderer sr = BoltCutters.GetComponentInChildren<SpriteRenderer>();
        if (sr.sprite == BoltCutter1)
        {
            sr.sprite = BoltCutter2;
        }
        else
        {
            sr.sprite = BoltCutter1;
        }
    }
}
