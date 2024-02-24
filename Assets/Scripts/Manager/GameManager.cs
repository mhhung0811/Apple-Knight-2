using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }

    public PlayerGameData GameData;

    public UnityAction SaveData;
    public UnityAction LoadData;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
    }
    void Start()
    {
        SaveSystem.LoadData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }
    public void SaveGame()
    {
        SaveData?.Invoke();
    }
    public void LoadGame()
    {
        LoadData?.Invoke();
    }
}
