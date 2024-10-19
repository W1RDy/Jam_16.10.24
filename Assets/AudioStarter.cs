using UnityEngine;

public class AudioStarter : MonoBehaviour
{
    [SerializeField] private AudioSettings _settings;
    [SerializeField] private string _themeMusicIndex;

    private void Awake()
    {
        _settings.Init();
        AudioManager.Instance.PlayMusic(_themeMusicIndex);
    }
}
