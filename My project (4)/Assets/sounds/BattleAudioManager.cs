using UnityEngine;

public class BattleAudioManager : MonoBehaviour
{
    [Header("~~~~~~~~~~~~AUDIO SOURCE REFERENCES~~~~~~~~~~~~")]

    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource sfxSource;

    [Header("~~~~~~~~~~~~AUDIO CLIP REFERENCES~~~~~~~~~~~~")]

    [Header("Music Clips")]
    public AudioClip safe_zone_music;
    public AudioClip faze1_music;
    public AudioClip faze2_music;
    public AudioClip faze3_music;
    public AudioClip death_music;
    public AudioClip survive_music;

    [Header("SFX Clips")]
    public AudioClip button_click;
    public AudioClip walking_player;
    public AudioClip running_player;
    public AudioClip phase_change_sound;
    public AudioClip player_dead_sound;
    public AudioClip player_hit_sound;
    public AudioClip enemy_hit_sound;
    public AudioClip enemy_fire_sound;
    public AudioClip enemy_die;
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

    public void PlayDeathMusic()
    {
        musicSource.clip = death_music;
        musicSource.Play();
    }

    public void stopAllMusic()
    {
        if (musicSource == null)
            return;

        // If death music is playing, DO NOT stop it
        if (musicSource.clip == death_music && musicSource.isPlaying)
            return;

        musicSource.Stop();
    }

    public void PlaySurviveMusic()
    {
        musicSource.clip = survive_music;
        musicSource.Play();
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~SFX~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    public void PlayButtonClick()
    {
        sfxSource.clip = button_click;
        sfxSource.Play();
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

    public void PlayPlayerHitSound()
    {
        sfxSource.clip = player_hit_sound;
        sfxSource.Play();
    }

    public void PlayEnemyHitSound()
    {
        sfxSource.clip = enemy_hit_sound;
        sfxSource.Play();
    }

    public void PlayEnemyShootSound() 
    { 
        sfxSource.clip = enemy_fire_sound;
        sfxSource.Play();
    }

    public void PlayEnemyDie() 
    { 
        sfxSource.clip = enemy_die;
        sfxSource.Play();
    }
}
