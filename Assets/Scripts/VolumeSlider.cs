using UnityEngine;
using UnityEngine.UI;

// VolumeSlider: ek slider ko mixer ke ek volume se jodta hai.
// Ye script har slider par lagegi, bas "Param Name" alag alag hoga.
[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    // Mixer ka exposed naam, jaise MasterVol / MusicVol / SFXVol / UIVol
    public string paramName = "MasterVol";

    Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();

        // Slider ki range: 0.0001 se 1 (0 nahi, kyunke Log10(0) se error aata hai)
        slider.minValue = 0.0001f;
        slider.maxValue = 1f;
        slider.wholeNumbers = false;
    }

    // Settings screen khulte hi chalta hai
    void OnEnable()
    {
        if (AudioManager.Instance != null)
        {
            // Saved value slider par dikhao.
            // SetValueWithoutNotify: bina event bheje value badlo (warna phir se save ho jati)
            slider.SetValueWithoutNotify(AudioManager.Instance.GetSaved(paramName));
        }

        // += se "sunna shuru": slider hile to OnSliderChanged chale
        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    // Screen band hote hi chalta hai
    void OnDisable()
    {
        // -= se "sunna band": ye karna zaroori hai, warna purana listener atka rehta hai
        slider.onValueChanged.RemoveListener(OnSliderChanged);
    }

    // Slider hilne par: nayi value AudioManager ko do (wahi dB mein badal ke save karega)
    void OnSliderChanged(float value)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetVolume(paramName, value);
    }
}