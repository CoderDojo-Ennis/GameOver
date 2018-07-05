using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GeekyMonkeyVideoPlaylist : MonoBehaviour {

    /// <summary>
    /// List of videos
    /// </summary>
    public VideoClip[] VideoClips;

    /// <summary>
    /// Heading text
    /// </summary>
    public string Heading;

    /// <summary>
    /// Current video index
    /// </summary>
    private int Index;

    private GeekyMonkeyVideoDirector VideoDirector;

	// Use this for initialization
	void Start () {
        VideoDirector = GeekyMonkeyVideoDirector.Instance;
	}
	
    /// <summary>
    /// Play the next video in the playlist
    /// </summary>
    /// <returns>Promise</returns>
    public GmDelayPromise PlayNext()
    {
        string prefsKey = "VideoPlaylistIndex_" + this.name;
        int index = PlayerPrefs.GetInt(prefsKey, 0);
        index++;
        if (index >= this.VideoClips.Length)
        {
            index = 0;
        }
        PlayerPrefs.SetInt(prefsKey, index);

        return VideoDirector.PlayClip(VideoClips[index], Heading);
    }

	// Update is called once per frame
	//void Update () {
	//}
}
