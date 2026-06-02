using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("~~~~~~~~~~~~AUDIO SOURCE REFERENCES~~~~~~~~~~~~")]

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("~~~~~~~~~~~~AUDIO CLIP REFERENCES~~~~~~~~~~~~")]
    public AudioClip main_menu_music;
    public AudioClip button_click;

    void Start()
    {
        PlayMenuMusic();
    }
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~MUSIC~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    public void PlayMenuMusic()
    {
        musicSource.clip = main_menu_music;
        musicSource.Play();
    }
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~SFX~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    public void PlayButtonClick() 
    {
        sfxSource.clip = button_click;
        sfxSource.Play();
    }
}
