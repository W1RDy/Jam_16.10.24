using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour 
{
    [SerializeField] private AudioData _audioData;
    private AudioSource _audioSource;

    private AudioData _audioDataInstance;

    private Dictionary<string, AudioConfig> _dict = new Dictionary<string, AudioConfig>();

    public static AudioManager Instance { get; private set; }

    private float _musicVolume;
    private float _soundsVolume;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _audioSource = GetComponent<AudioSource>();
            InitAudioDictionary(_audioData);

            SetVolume();
        }
        else Destroy(gameObject);

        DontDestroyOnLoad(Instance);
    }

    public void SetVolume()
    {
        _audioSource.volume /= _musicVolume;
        _musicVolume = SaveSystem.Instance.SaveData.MusicSettings;
        _audioSource.volume = _musicVolume;

        _soundsVolume = SaveSystem.Instance.SaveData.SoundsSettings;
    }

    private void InitAudioDictionary(AudioData audioData)
    {
        _audioDataInstance = Instantiate(audioData);

        foreach (var audioConfig in _audioDataInstance.Audios)
        {
            _dict.Add(audioConfig.Index, audioConfig);
        }
    }

    public void PlayMusic(string index)
    {
        var audio = _dict[index];

        if (_audioSource.isPlaying && _audioSource.clip == audio.AudioClip) return;
        else
        {
            if (_audioSource.isPlaying) _audioSource.Stop();

            _audioSource.clip = audio.AudioClip;
            _audioSource.volume = audio.Volume * _musicVolume;
            _audioSource.Play();
        }
    }

    public void PlaySound(string index)
    {
        var audio = _dict[index];

        _audioSource.PlayOneShot(audio.AudioClip, audio.Volume * _soundsVolume);
    }
}
