using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GeekyMonkeyVideoPlaylist VideoPlaylist_War;

    GeekyMonkeyVideoDirector videoDirector;

    GameObject Menu;
    GameObject MainCamera;
    GameObject VideoCamera;

	// Use this for initialization
	void Start () {
        videoDirector = GeekyMonkeyVideoDirector.Instance;

        // Disable everything but the intro video
        Menu = GameObject.Find("Menu");
        Menu.SetActive(false);

        MainCamera = GameObject.Find("Main Camera");
        MainCamera.SetActive(false);

        // Start intro video
        VideoCamera = GameObject.Find("VideoCamera");
        VideoCamera.GetComponent<Camera>().enabled = true;
        //this.Delay(3, () =>
        //{
            VideoPlaylist_War.PlayNext().Then(() =>
            {
                Menu.SetActive(true);
                Debug.Log("video done!");
            });

            // Allow for video fade-in, then activate menu in the background
            this.Delay(3, () =>
            {
                Menu.SetActive(true);
                MainCamera.SetActive(true);
            });
        //});


    }

    // Update is called once per frame
    void Update () {
		
	}
}
