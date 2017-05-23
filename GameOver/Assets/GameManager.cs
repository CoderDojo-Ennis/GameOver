using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    GeekyMonkeyVideoDirector videoDirector;

	// Use this for initialization
	void Start () {
        videoDirector = GeekyMonkeyVideoDirector.Instance;

        this.Delay(3, () =>
        {
            videoDirector.PlayClip().Then(() =>
            {
                Debug.Log("video done!");
            });
        });
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
