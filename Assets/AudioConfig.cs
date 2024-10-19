using System;
using UnityEngine;

[Serializable]
public class AudioConfig
{
    [SerializeField] private string _index;
    [SerializeField] private AudioClip _audioClip;

    [SerializeField] private float _volume;

    public string Index => _index;
    public AudioClip AudioClip => _audioClip;
    public float Volume => _volume;
}
