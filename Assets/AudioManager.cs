using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour 
{
    [SerializeField] private AudioData _audioData;
    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    private AudioSource _audioSource;

    private AudioData _audioDataInstance;

    private Dictionary<string, AudioConfig> _dict = new Dictionary<string, AudioConfig>();

    public static AudioManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _audioSource = GetComponent<AudioSource>();
            InitAudioDictionary(_audioData);
        }
        else Destroy(gameObject);

        DontDestroyOnLoad(Instance);
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

            _audioSource.volume = audio.Volume;
            
            _audioSource.Play();
        }
    }

    public void PlaySound(string index)
    {
        var audio = _dict[index];

        _audioMixerGroup.audioMixer.GetFloat("Sounds", out var mixerVolume);
        _audioSource.PlayOneShot(audio.AudioClip, audio.Volume * mixerVolume);
    }
}
