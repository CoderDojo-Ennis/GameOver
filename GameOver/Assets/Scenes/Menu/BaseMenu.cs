using UnityEngine;

public abstract class BaseMenu : MonoBehaviour
{
    private AudioSource MenuAudioSource;

    public virtual void ShowMenu(float fadeSeconds)
    {
        var menuCamera = gameObject.GetComponentInChildren<Camera>();
        GameManager.Instance.FadeCameraIn(fadeSeconds, menuCamera);
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
