using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource ActualMusic;
    public AudioSource buttonSound;
    public AudioSource deathSound;
    public AudioSource coalSound;

    public void PlayButton()
    {
        buttonSound.Play();
    }
    public void PlayDeath()
    {
        deathSound.Play();
    }
    public void PlayCoal()
    {
        coalSound.Play();
    }
}
