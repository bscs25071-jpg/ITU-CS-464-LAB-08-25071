using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

// AudioManager: poori game ki awaaz ka "manager".
// Ye sirf EK baar banta hai aur scenes badalne par bhi zinda rehta hai.
public class AudioManager : MonoBehaviour
{
    // Singleton: kisi bhi script se AudioManager.Instance likh ke isse bula sakte hain
    public static AudioManager Instance { get; private set; }

    [Header("Mixer")]
    public AudioMixer mixer; // MainMixer yahan drag hoga

    [Header("Music sources (2, taake crossfade ho sake)")]
    public AudioSource musicA;
    public AudioSource musicB;

    [Header("SFX source (chhoti awaazon ke liye)")]
    public AudioSource sfxSource;

    // Mixer mein jo naam expose kiye the, wahi yahan likhne hain.
    // Agar rename nahi hua to yahan apne asli naam (MyExposedParam waghera) likh do.
    public const string MASTER = "MasterVol";
    public const string MUSIC = "MusicVol";
    public const string SFX = "SFXVol";
    public const string UI = "UIVol";

    AudioSource activeMusic; // abhi kaun si music source chal rahi hai
    Coroutine fadeRoutine;   // chalta hua crossfade (taake do ek saath na chalein)

    void Awake()
    {
        // Agar pehle se koi AudioManager hai to ye wala khatam kar do (duplicate nahi chahiye)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // scene badalne par bhi zinda rahe
        activeMusic = musicA;
        musicA.ignoreListenerPause = true;
        musicB.ignoreListenerPause = true;
    }

    void Start()
    {
        // Saved volumes ko mixer par lagao.
        // Start mein karte hain, Awake mein nahi: kabhi kabhi Awake mein SetFloat kaam nahi karta.
        ApplySaved(MASTER);
        ApplySaved(MUSIC);
        ApplySaved(SFX);
        ApplySaved(UI);
    }

    // ---------- VOLUME ----------

    // Slider ki value (0 se 1) ko decibel mein badal ke mixer ko deta hai
    public void SetVolume(string param, float sliderValue)
    {
        // Log10(0) nahi ho sakta (error), isliye minimum 0.0001 rakha
        float safe = Mathf.Max(sliderValue, 0.0001f);

        // Formula: dB = log10(value) * 20
        // value 1      -> 0 dB   (poori awaaz)
        // value 0.5    -> -6 dB  (kaan ko aadhi lagti hai)
        // value 0.0001 -> -80 dB (lagbhag khamosh = mute)
        float dB = Mathf.Log10(safe) * 20f;

        mixer.SetFloat(param, dB);

        // Restart ke baad bhi yaad rahe
        PlayerPrefs.SetFloat(param, sliderValue);
        PlayerPrefs.Save();
    }

    // Saved value wapas do (pehli baar kuch saved nahi hota, to default 1 = poori awaaz)
    public float GetSaved(string param)
    {
        return PlayerPrefs.GetFloat(param, 1f);
    }

    void ApplySaved(string param)
    {
        SetVolume(param, GetSaved(param));
    }

    // ---------- SFX ----------

    // Chhoti awaaz bajao. PlayOneShot se kai awaazein ek saath overlap ho sakti hain.
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        // Thodi si pitch variation, taake baar baar wahi awaaz boring na lage
        sfxSource.pitch = Random.Range(0.95f, 1.05f);
        sfxSource.PlayOneShot(clip, volume);
    }

    // ---------- MUSIC ----------

    // Nayi music dheere dheere laao, purani dheere dheere khatam karo
    public void PlayMusic(AudioClip clip, float fadeTime = 1f)
    {
        if (clip == null) return;
        if (activeMusic.clip == clip && activeMusic.isPlaying) return; // wahi chal rahi hai

        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(Crossfade(clip, fadeTime));
    }

    IEnumerator Crossfade(AudioClip clip, float fadeTime)
    {
        AudioSource from = activeMusic;
        AudioSource to = (from == musicA) ? musicB : musicA;

        to.clip = clip;
        to.volume = 0f;
        to.loop = true; // background music baar baar chale
        to.Play();

        float t = 0f;
        float fromStart = from.volume;
        while (t < 1f)
        {
            // unscaled: pause (timeScale = 0) mein bhi fade chale
            t += Time.unscaledDeltaTime / fadeTime;
            from.volume = Mathf.Lerp(fromStart, 0f, t);
            to.volume = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }

        from.Stop();
        activeMusic = to;
    }
}