using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class SaveSystem
{
    private const string key = "GAME_DATA";
    public static UnityAction onSave;
    public static UnityAction onLoad;
    public static void SaveData(PlayerGameData data)
    {
        onSave?.Invoke();
        PlayerPrefs.SetString(key, JsonUtility.ToJson(data));
        PlayerPrefs.Save();
    }
    public static void LoadData()
    {
        if (PlayerPrefs.HasKey(key))
        {
            GameManager.Instance.GameData = JsonUtility.FromJson<PlayerGameData>(PlayerPrefs.GetString(key));
            onLoad?.Invoke();
        }
    }
}
