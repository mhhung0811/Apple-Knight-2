using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{
    private const string key = "GAME_DATA";
    public static void SaveData(PlayerGameData data)
    {
        PlayerPrefs.SetString(key, JsonUtility.ToJson(data));
        PlayerPrefs.Save();
    }
    public static void LoadData()
    {
        if (PlayerPrefs.HasKey(key))
        {
            GameManager.Instance.GameData = JsonUtility.FromJson<PlayerGameData>(PlayerPrefs.GetString(key));
            GameManager.Instance.LoadGame();
        }
    }
}
