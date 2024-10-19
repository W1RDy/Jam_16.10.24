using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundsSlider;

    [SerializeField] private AudioMixerGroup _soundsGroup;
    [SerializeField] private AudioMixerGroup _musicGroup;

    public void Init()
    {
        Subscribe();

        _musicSlider.value = SaveSystem.Instance.SaveData.MusicSettings;
        _soundsSlider.value = SaveSystem.Instance.SaveData.SoundsSettings;

        SetSettings();
    }

    private void SetSettings()
    {
        if (_musicSlider.value != SaveSystem.Instance.SaveData.MusicSettings) SaveSystem.Instance.SaveData.MusicSettings = _musicSlider.value;
        if (_soundsSlider.value != SaveSystem.Instance.SaveData.SoundsSettings) SaveSystem.Instance.SaveData.SoundsSettings = _soundsSlider.value;

        _musicGroup.audioMixer.SetFloat("Music", Mathf.Lerp(-80, 20, SaveSystem.Instance.SaveData.MusicSettings));
        _soundsGroup.audioMixer.SetFloat("Sounds", Mathf.Lerp(-80, 20, SaveSystem.Instance.SaveData.SoundsSettings));
    }

    private void MusicChanges(float newValue)
    {
        if (newValue != SaveSystem.Instance.SaveData.MusicSettings) SetSettings();
    }

    private void SoundsChanges(float newValue)
    {
        if (newValue != SaveSystem.Instance.SaveData.SoundsSettings) SetSettings();
    }

    private void Subscribe()
    {
        _musicSlider.onValueChanged.AddListener(MusicChanges);
        _soundsSlider.onValueChanged.AddListener(SoundsChanges);
    }

    private void Unsubscribe()
    {
        _musicSlider.onValueChanged.AddListener(MusicChanges);
        _soundsSlider.onValueChanged.AddListener(SoundsChanges);
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }
}
