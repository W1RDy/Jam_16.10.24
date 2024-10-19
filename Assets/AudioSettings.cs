using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundsSlider;

    public void Init()
    {
        Subscribe();

       _musicSlider.value = SaveSystem.Instance.SaveData.MusicSettings;
       _soundsSlider.value = SaveSystem.Instance.SaveData.SoundsSettings;
    }

    private void MusicChanges(float newValue)
    {
        if (SaveSystem.Instance.SaveData.MusicSettings != newValue)
        {
            _musicSlider.value = newValue;
            SaveSystem.Instance.SaveData.MusicSettings = newValue;

            AudioManager.Instance.SetVolume();
        }
    }

    private void SoundsChanges(float newValue)
    {
        if (SaveSystem.Instance.SaveData.SoundsSettings != newValue)
        {
            _soundsSlider.value = newValue;
            SaveSystem.Instance.SaveData.SoundsSettings = newValue;

            AudioManager.Instance.SetVolume();
        }
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
