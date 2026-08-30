using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Mixer")]
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;

    [Header("Background Music")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private bool playMusicOnStart = true;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.outputAudioMixerGroup = musicMixerGroup;
        musicSource.loop = true;
        musicSource.playOnAwake = false;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.outputAudioMixerGroup = sfxMixerGroup;
        sfxSource.playOnAwake = false;
    }

    private void Start()
    {
        if (playMusicOnStart && backgroundMusic != null)
        {
            PlayMusic(backgroundMusic);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void SetMasterVolume(float linearValue)
    {
        if (mainMixer == null) return;

        float dB = linearValue <= 0.0001f ? -80f : Mathf.Log10(linearValue) * 20f;
        mainMixer.SetFloat("MasterVolume", dB);
        PlayerPrefs.SetFloat("MasterVolume", linearValue);
        PlayerPrefs.Save();
    }
}
