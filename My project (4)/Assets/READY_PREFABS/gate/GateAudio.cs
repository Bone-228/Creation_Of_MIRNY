using UnityEngine;

public class GateAudio : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip openClip;

    public void PlayOpenSound()
    {
        audioSource.PlayOneShot(openClip);
    }
}