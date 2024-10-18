using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _pause;    

    private bool _paused;

    void Start()
    {
        _settings.SetActive(false);
        _pause.SetActive(false);
        _paused = false;
    }

    public void ChangeSettings()
    {
        _settings.SetActive(!_settings.activeSelf);
    }
    public void ChangePause()
    {
        _pause.SetActive(!_pause.activeSelf);
        _paused = !_paused;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!_paused)
            {
                _pause.SetActive(true);
                _paused = true;
            }
            else if (_paused)
            {
                _pause.SetActive(false);
                _settings.SetActive(false);
                _paused = false;
            }
        }
    }
}
