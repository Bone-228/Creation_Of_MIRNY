using UnityEngine;

public class BattleAudioManager : MonoBehaviour
{
    [Header("~~~~~~~~~~~~AUDIO SOURCE REFERENCES~~~~~~~~~~~~")]

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("~~~~~~~~~~~~AUDIO CLIP REFERENCES~~~~~~~~~~~~")]
    public AudioClip safe_zone_music;
    public AudioClip button_click;

    void Start()
    {
        PlayMenuMusic();
    }
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~MUSIC~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    public void PlayMenuMusic()
    {
        musicSource.clip = safe_zone_music;
        musicSource.Play();
    }
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~SFX~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    public void PlayButtonClick()
    {
        sfxSource.clip = button_click;
        sfxSource.Play();
    }
}
