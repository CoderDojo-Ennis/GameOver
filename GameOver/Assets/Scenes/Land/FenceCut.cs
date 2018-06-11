using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FenceCut : MonoBehaviour
{
    public Slider ProgressSlider;
    public float CutSpeed;

	void Start ()
    {
        ProgressSlider.value = ProgressSlider.maxValue;
	}

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Player"))
        {
            ProgressSlider.value -= Time.fixedDeltaTime * CutSpeed;
            if (ProgressSlider.value == 0)
            {
                LandScene.instance.Win();
                gameObject.SetActive(false);
            }
        }
    }
}
