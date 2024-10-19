using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int CoinsCount = 0;
    public List<int> BoughtGoodsIndexes = new List<int>();
    public int Deaths = 0;
    public bool IsHaveGame = false;
    public string Name = "";

    public float MusicSettings = 0.5f;
    public float SoundsSettings = 0.5f;
}

public class SaveSystem : MonoBehaviour
{
    private string _filePath = Application.streamingAssetsPath + "\\Data.json";

    public static SaveSystem Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Load();
        }
        else Destroy(this);

        DontDestroyOnLoad(Instance);
    }

    [SerializeField] private SaveData _data;
    public SaveData SaveData { get => _data; }

    public void Save()
    {
        var json = JsonUtility.ToJson(SaveData);
        File.WriteAllText( _filePath, json );
    }

    public void Load()
    {
        var json = File.ReadAllText(_filePath);
        
        if (json != "" )
        {
            _data = JsonUtility.FromJson<SaveData>(json);
        }

    }

    public void ResetSavings()
    {
        var musicSettings = _data.MusicSettings;
        var volumeSettings = _data.SoundsSettings;
        var name = _data.Name;

        _data = new SaveData();

        _data.MusicSettings = musicSettings;
        _data.SoundsSettings = volumeSettings;
        _data.Name = name;
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}
