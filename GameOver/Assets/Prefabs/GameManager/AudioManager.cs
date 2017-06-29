using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance;

    private AudioSource[] TypingSound;
    private int typingSourceIndex;

    /// <summary>
    /// Before Start
    /// </summary>
    void Awake()
    {
        // Singleton that survives scene changes
        if (Instance != null)
        {
            Debug.Log("Audio manager 2nd instance abort");
            Destroy(gameObject);
            return;
        }

        Debug.Log("Audio manager awake");

        Instance = this;
    }

    /// <summary>
    /// After Awake
    /// </summary>
    void Start ()
    {
        this.TypingSound = GameObject.Find("Sound_Typing").GetComponents<AudioSource>();
	}
	
    /// <summary>
    /// Play the sound of typing one character
    /// </summary>
    public void PlayTypeCharacter()
    {
        typingSourceIndex++;
        if (typingSourceIndex == TypingSound.Length)
        {
            typingSourceIndex = 0;
        }
        TypingSound[typingSourceIndex].Play();
    }
}
