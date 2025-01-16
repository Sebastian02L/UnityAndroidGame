using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource ActualMusic;
    public AudioSource buttonSound;

    public void PlayButton()
    {
        buttonSound.Play();
    }
}
