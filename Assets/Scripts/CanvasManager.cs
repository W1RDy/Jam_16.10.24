using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject _main;
    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _authors;
    [SerializeField] private GameObject _nameAsk;
    [SerializeField] private GameObject _result;

    [SerializeField] private TextMeshProUGUI _nameLabel;
    [SerializeField] private TextMeshProUGUI _namePlaceholder;
    [SerializeField] private TextMeshProUGUI _resultText;

    void Start()
    {
        _main.SetActive(true);
        _settings.SetActive(false);
        _authors.SetActive(false);
        _nameAsk.SetActive(false);
        _result.SetActive(false);
    }

    public void ChangeSettings()
    {
        _main.SetActive(!_main.activeSelf);
        _settings.SetActive(!_settings.activeSelf);
    }
    public void ChangeAuthors()
    {
        _main.SetActive(!_main.activeSelf);
        _authors.SetActive(!_authors.activeSelf);
    }

    public void OpenAskName()
    {
        _nameAsk.SetActive(true);
        _main.SetActive(!_main.activeSelf);
    }

    public void StartGame()
    {
        SaveSystem.Instance.ResetSavings();

        int count = _nameLabel.text.Count();
        if (count > 1)
        {
            SaveSystem.Instance.SaveData.Name = _nameLabel.text.ToString();
            LoadGame();
        }
        if (count <= 1)
        {
            _namePlaceholder.color = Color.red;
        }
    }

    public void WatchResult()
    {
        _main.SetActive(false);
        _settings.SetActive(false);
        _authors.SetActive(false);
        _nameAsk.SetActive(false);
        _result.SetActive(true);

        string goodText = "������ ����, " + PlayerPrefs.GetString("name") + "! �����������!\r\n\r\n�� �������! �� ���� ����� �� ���������. �� �������� ����!\r\n\r\n������� �����";
        string badText = "������ ����, " + PlayerPrefs.GetString("name") + "! �����������!\r\n\r\n�� ������ ������������ ������ ����� " + PlayerPrefs.GetInt("deaths") + " �������.\r\n���� ����� ��� �� ������� ����� ������.\r\n����� �� ������� ��� ������� � ��������� ����� ������� ���������� ���������� ������ � ����������� ��������� ��������� ���� ����� ������������ �������.\r\n�� �� ������-�� ��������� ���� ������� � ������ �������� ������ ���������� �����-�� ��������� �����.\r\n\r\n��������?\r\n\r\n������� ����� ";

        if (PlayerPrefs.HasKey("deaths") && PlayerPrefs.GetInt("deaths") == 0)
        {
            _resultText.text = goodText;
        }
        else if (PlayerPrefs.HasKey("deaths") && PlayerPrefs.GetInt("deaths") > 0)
        {
            _resultText.text = badText;
        }
        else
        {
            _resultText.text = "���� �� �����?...";
        }
    }

    public void LoadGame()
    {
        if (SaveSystem.Instance.SaveData.Name != "")
        {
            SaveSystem.Instance.SaveData.IsHaveGame = true;
            SceneManager.LoadScene(1);
        }
        else
        {
            OpenAskName();
        }
    }

    private void ActivateClickSound()
    {

    }

    public void GameExit()
    {
        print("exit");
        Application.Quit();
    }
}
