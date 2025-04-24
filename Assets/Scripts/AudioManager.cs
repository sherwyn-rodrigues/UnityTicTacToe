using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip bgMusicClip;
    public AudioClip buttonClickSFX;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            PlayBGMusic();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGMusic()
    {
        if(!musicSource.isPlaying && bgMusicClip != null)
        {
            musicSource.clip = bgMusicClip;
            musicSource.loop = true;
            musicSource.volume = 1.0f;
            musicSource.Play();
        }
    }

    public void PlaySFX()
    {
        if (!sfxSource.mute && buttonClickSFX != null)
        {
            sfxSource.PlayOneShot(buttonClickSFX);
        }
    }

    public void PlayBoardBtnSFX()
    {

    }
}
