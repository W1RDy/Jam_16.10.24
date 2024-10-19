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
        _data = new SaveData();
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}
