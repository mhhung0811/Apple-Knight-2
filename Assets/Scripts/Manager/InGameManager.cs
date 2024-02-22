using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    private static InGameManager _instance;
    public static InGameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<InGameManager>();
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

    private bool _isPauseGame;
    private bool isStartLvBoss;

    [SerializeField]
    private GameObject doorStartlvBoss;
    [SerializeField]
    private GameObject doorEndlvBoss;
    void Start()
    {
        isStartLvBoss = true;
    }

    void Update()
    {
        
    }

    public void GameOver()
    {
        Debug.Log("gameover");
        UIManager.Instance.GameOver();
    }
    public bool PauseGame()
    {
        if (_isPauseGame)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void ButtonPause()
    {
        _isPauseGame = true;
        Time.timeScale = 0;
    }
    public void ButtonResume()
    {
        _isPauseGame = false;
        Time.timeScale = 1;
    }
    public void ReStartGame()
    {
        _isPauseGame = true;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Victory()
    {
        ButtonPause();
        UIManager.Instance.Victory();
    }
    public void StartLevelBoss()
    {
        if (isStartLvBoss)
        {
            isStartLvBoss = false;
            IInteractable interactable = doorStartlvBoss.gameObject.GetComponent<IInteractable>();
            interactable.InteractOff();
        }
    }
    public void EndLevelBoss()
    {
        IInteractable interactable = doorEndlvBoss.gameObject.GetComponent<IInteractable>();
        interactable.InteractOn();
    }
}
