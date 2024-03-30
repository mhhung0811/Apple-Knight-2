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

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
    }
    void Start()
    {

    }
    void Update()
    {
        
    }
    public void ContinueGame()
    {
        SaveDataManager.Instance.SaveOptionPlay(true);
        SceneManager.LoadScene("Level1Test");
    }
    public void StartGame()
    {
        SaveDataManager.Instance.SaveOptionPlay(false);
        SceneManager.LoadScene("Level1Test");
    }
    public void ButtonQuitGame()
    {
        SceneManager.LoadScene("Start Game");
    }
    public void DeleteKey()
    {
        if (PlayerPrefs.HasKey("HIGH_SCORE"))
        {
            string dataHighScore = PlayerPrefs.GetString("HIGH_SCORE");

            PlayerPrefs.DeleteAll();

            PlayerPrefs.SetString(key: "HIGH_SCORE",value: dataHighScore);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
