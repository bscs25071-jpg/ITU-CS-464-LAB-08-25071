using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Mixer")]
    public AudioMixer mixer;

    [Header("Music Sources")]
    public AudioSource musicA;
    public AudioSource musicB;

    [Header("Music Clips")]
    public AudioClip menuMusic;
    public AudioClip gameplayMusic;
    public AudioClip resultMusic;

    [Header("SFX Source & Clips")]
    public AudioSource sfxSource;
    public AudioClip pickupSFX;
    public AudioClip damageSFX;
    public AudioClip deathSFX;
    public AudioClip boostSFX;
    public AudioClip winSFX;
    public AudioClip loseSFX;

    [Header("UI Source & Clips")]
    public AudioSource uiSource;
    public AudioClip buttonHover;
    public AudioClip buttonClick;
    public AudioClip buttonBack;
    public AudioClip panelOpen;
    public AudioClip panelClose;
    public AudioClip pauseSound;
    public AudioClip resumeSound;
    public AudioClip successSound;
    public AudioClip errorSound;

    public const string MASTER = "MasterVol";
    public const string MUSIC = "MusicVol";
    public const string SFX = "SFXVol";
    public const string UI = "UIVol";

    AudioSource activeMusic;
    Coroutine fadeRoutine;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        activeMusic = musicA;

        if (musicA != null) musicA.ignoreListenerPause = true;
        if (musicB != null) musicB.ignoreListenerPause = true;
        if (uiSource != null) uiSource.ignoreListenerPause = true;
    }

    void Start()
    {
        ApplySaved(MASTER);
        ApplySaved(MUSIC);
        ApplySaved(SFX);
        ApplySaved(UI);

        if (menuMusic != null && (activeMusic == null || !activeMusic.isPlaying))
            PlayMusic(menuMusic, 0.5f);
    }

    // ---------- VOLUME ----------

    public void SetVolume(string param, float sliderValue)
    {
        float safe = Mathf.Max(sliderValue, 0.0001f);
        float dB = Mathf.Log10(safe) * 20f;

        if (mixer != null)
            mixer.SetFloat(param, dB);

        PlayerPrefs.SetFloat(param, sliderValue);
        PlayerPrefs.Save();
    }

    public float GetSaved(string param)
    {
        return PlayerPrefs.GetFloat(param, 1f);
    }

    void ApplySaved(string param)
    {
        SetVolume(param, GetSaved(param));
    }

    // ---------- SFX ----------

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null || sfxSource == null) return;

        sfxSource.pitch = Random.Range(0.95f, 1.05f);
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayPickup() => PlaySFX(pickupSFX);
    public void PlayDamage() => PlaySFX(damageSFX);
    public void PlayDeath() => PlaySFX(deathSFX);
    public void PlayBoost() => PlaySFX(boostSFX);
    public void PlayWin() => PlaySFX(winSFX);
    public void PlayLose() => PlaySFX(loseSFX);

    // ---------- UI SOUND ----------

    public void PlayUI(AudioClip clip)
    {
        if (clip == null || uiSource == null) return;

        uiSource.PlayOneShot(clip);
    }

    public void PlayButtonHover() => PlayUI(buttonHover);
    public void PlayButtonClick() => PlayUI(buttonClick);
    public void PlayButtonBack() => PlayUI(buttonBack != null ? buttonBack : buttonClick);
    public void PlayPanelOpen() => PlayUI(panelOpen);
    public void PlayPanelClose() => PlayUI(panelClose);
    public void PlayPause() => PlayUI(pauseSound != null ? pauseSound : panelOpen);
    public void PlayResume() => PlayUI(resumeSound != null ? resumeSound : panelClose);
    public void PlaySuccess() => PlayUI(successSound != null ? successSound : buttonClick);
    public void PlayError() => PlayUI(errorSound != null ? errorSound : buttonBack);

    // ---------- MUSIC ----------

    public void PlayMenuMusic() => PlayMusic(menuMusic, 0.8f);
    public void PlayGameplayMusic() => PlayMusic(gameplayMusic, 0.8f);
    public void PlayResultMusic() => PlayMusic(resultMusic, 0.8f);

    public void PlayMusic(AudioClip clip, float fadeTime = 1f)
    {
        if (clip == null || musicA == null || musicB == null) return;

        if (activeMusic.clip == clip && activeMusic.isPlaying)
            return;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(Crossfade(clip, fadeTime));
    }

    IEnumerator Crossfade(AudioClip clip, float fadeTime)
    {
        AudioSource from = activeMusic;
        AudioSource to = (from == musicA) ? musicB : musicA;

        to.clip = clip;
        to.volume = 0f;
        to.loop = true;
        to.Play();

        float t = 0f;
        float fromStart = from.volume;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / fadeTime;

            from.volume = Mathf.Lerp(fromStart, 0f, t);
            to.volume = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        from.Stop();
        activeMusic = to;
    }
}