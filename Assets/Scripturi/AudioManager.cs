using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Wired in Inspector")]
    public AudioMixer audioMixer;   // -> GameAudioMixer
    public Slider     musicSlider;  // -> your UI Slider

    private const string PARAM = "MusicVolume";

    void Start()
    {
        // initialize slider from PlayerPrefs (optional)
        float saved = PlayerPrefs.GetFloat(PARAM, 0.75f);
        musicSlider.value = saved;

        // hook up the callback
        musicSlider.onValueChanged.AddListener(SetMusicVolume);

        // apply immediately
        SetMusicVolume(saved);
    }

    /// <summary>
    /// Called whenever the slider moves.
    /// Converts linear [0.0001–1] into dB [-80–0] via log10.
    /// </summary>
    public void SetMusicVolume(float sliderValue)
    {
        // log10(1)=0 → 0dB, log10(0.0001)≈-4 → -80dB
        audioMixer.SetFloat(PARAM, Mathf.Log10(sliderValue) * 20f);

        // save for next session
        PlayerPrefs.SetFloat(PARAM, sliderValue);
    }
}
