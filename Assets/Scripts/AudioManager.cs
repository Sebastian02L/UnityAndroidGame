using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource ActualMusic;
    public AudioSource buttonSound;
    public AudioSource deathSound;

    public void PlayButton()
    {
        buttonSound.Play();
    }
    public void PlayDeath()
    {
        deathSound.Play();
    }
}
