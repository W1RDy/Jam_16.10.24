using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "New Data/New AudioData")]
public class AudioData : ScriptableObject
{
    [SerializeField] private AudioConfig[] _audios;

    public AudioConfig[] Audios => _audios;
}
