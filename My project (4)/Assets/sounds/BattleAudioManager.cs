using UnityEngine;

public class BattleAudioManager : MonoBehaviour
{
    [Header("~~~~~~~~~~~~AUDIO SOURCE REFERENCES~~~~~~~~~~~~")]

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("~~~~~~~~~~~~AUDIO CLIP REFERENCES~~~~~~~~~~~~")]
    public AudioClip safe_zone_music;
    public AudioClip button_click;
    public AudioClip faze1_music;
    public AudioClip faze2_music;
    public AudioClip faze3_music;


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


    public void PlayFazeOne() 
    { 
        musicSource.clip = faze1_music;
        musicSource.Play();
    }

    public void PlayFazeTwo()
    {
        musicSource.clip = faze2_music;
        musicSource.Play();
    }

    public void PlayFazeThree() 
    { 
        musicSource.clip = faze3_music;
        musicSource.Play();
    }


    public void PlaySelectedSound(AudioClip selectedClip) 
    { 
        sfxSource.clip = selectedClip;
        sfxSource.Play();
    }
}
