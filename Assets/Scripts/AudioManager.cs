using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioSource actualMusic;
    public AudioSource buttonSound;
    public AudioSource deathSound;
    public AudioSource coalSound;

    private void Start()
    {
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += OnSceneChanged;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneChanged;
    }

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

    private void OnSceneChanged(Scene scene, LoadSceneMode mode)
    {
        actualMusic.Stop();
        Destroy(this.gameObject, 1);
    }
}
