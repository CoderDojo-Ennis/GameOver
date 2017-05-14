using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepIntoTextColor : MonoBehaviour {

    private TextMesh textMesh;
    private float timeAccumulated = 0f;

    public float ColorChangeSeconds = 0.1f;

	// Use this for initialization
	void Start () {
        this.textMesh = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
        timeAccumulated += Time.deltaTime;
        if (timeAccumulated > ColorChangeSeconds)
        {
            textMesh.color = new Color(Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f));
            timeAccumulated = 0;
        }
    }
}
