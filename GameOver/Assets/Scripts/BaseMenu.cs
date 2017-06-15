using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMenu : MonoBehaviour
{
    private AudioSource MenuAudioSource;

    public virtual void ShowMenu()
    {

    }

    /// <summary>
    /// Play a menu sound
    /// </summary>
    /// <param name="sound">The sound to play</param>
    public void PlayMenuSound(AudioClip sound)
    {
        if (MenuAudioSource == null)
        {
            MenuAudioSource = GameObject.Find("MenuAudioSource").GetComponent<AudioSource>();
        }

        MenuAudioSource.PlayOneShot(sound);
    }
}
