using UnityEngine;
using UnityEngine.Rendering;
using static GridSpace;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    // audio source references for bg music and sfx
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    // audio file references
    [Header("Audio Clips")]
    public AudioClip bgMusicClip;
    public AudioClip buttonClickSFX;
    public AudioClip Player1Turn;
    public AudioClip Player2Turn;
    public AudioClip WinScreenSFX;
    public AudioClip DrawScreenSFX;

    //bool to check audio (need to add player prefs)
    bool isBGMPaused;
    bool isSFXDisabled; 

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
        if (!sfxSource.mute && buttonClickSFX != null && !isSFXDisabled)
        {
            sfxSource.PlayOneShot(buttonClickSFX);
        }
    }

    public void ToggleSFX()
    {
        isSFXDisabled = !isSFXDisabled;
    }

    public void ToggleBGMusic()
    {
        if (isBGMPaused) musicSource.UnPause();
        else musicSource.Pause();

        isBGMPaused = !isBGMPaused;
    }

    public void saveAudioSettings()
    {
        PlayerPrefs.SetInt("BGMusic", isBGMPaused ? 1 : 0);
        PlayerPrefs.SetInt("SFXDisabled", isSFXDisabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void LoadAudioSettings()
    {
        isBGMPaused = PlayerPrefs.GetInt("BGMusic", 0) == 1;
        isSFXDisabled = PlayerPrefs.GetInt("SFXDisabled", 0) == 1;
    }

    public bool IsMusicOn() => !isBGMPaused;
    public bool IsSFXOn() => !isSFXDisabled;

    public void PlayerTurnSFX(ref ButtonState state)
    {
        if (!sfxSource.mute && buttonClickSFX != null && !isSFXDisabled)
        {
            if (state == ButtonState.Player1)
            {
                sfxSource.PlayOneShot(Player1Turn);
                return;
            }
            if (state == ButtonState.Player2)
            {
                sfxSource.PlayOneShot(Player2Turn);
                return;
            }
        }
    }

    public void PlayDrawSFX()
    {
        if (!sfxSource.mute && buttonClickSFX != null && !isSFXDisabled)
        {
            sfxSource.PlayOneShot(DrawScreenSFX);
        }
    }

    public void PlayWinSFX()
    {
        if (!sfxSource.mute && buttonClickSFX != null && !isSFXDisabled)
        {
            sfxSource.PlayOneShot(WinScreenSFX);
        } 
    }
}
