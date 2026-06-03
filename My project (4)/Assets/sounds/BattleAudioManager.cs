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
    public AudioClip walking_player;
    public AudioClip running_player;
    public AudioClip phase_change_sound;
    public AudioClip player_dead_sound;

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

    public void PlayWalkingSound()
    {
        if (sfxSource.clip == walking_player && sfxSource.isPlaying)
            return;

        sfxSource.clip = walking_player;
        sfxSource.loop = true;
        sfxSource.Play();
    }

    public void StopWalkingSound()
    {
        if (sfxSource.clip == walking_player)
        {
            sfxSource.Stop();
            sfxSource.loop = false;
        }
    }

    public void PlayRunningSound()
    {
        if (sfxSource.clip == running_player && sfxSource.isPlaying)
            return;

        sfxSource.clip = running_player;
        sfxSource.loop = true;
        sfxSource.Play();
    }

    public void StopRunningSound()
    {
        if (sfxSource.clip == running_player)
        {
            sfxSource.Stop();
            sfxSource.loop = false;
        }
    }

    public void PlayPhaseChangeSound()
    {
        sfxSource.clip = phase_change_sound;
        sfxSource.Play();
    }


    public void PlayPlayerDeadSound() 
    { 
        sfxSource.clip = player_dead_sound;
        sfxSource.Play();
    }
}
