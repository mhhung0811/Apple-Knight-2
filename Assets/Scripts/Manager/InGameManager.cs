using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    private int expPlayer;
    private int score;
    private int highScore;
    private int totalTime;
    public bool isStartLvBoss;


    [SerializeField]
    private GameObject doorStartlvBoss;
    [SerializeField]
    private GameObject doorEndlvBoss;
    void Start()
    {
        isStartLvBoss = true;
        expPlayer = 0;
        score = 0;
        highScore = 0;
        totalTime = 0;
        StartCoroutine(UpdateTimeIngame());
    }

    void Update()
    {
        
    }
    public void IncreaseExp(int exp,int scores = 5)
    {
        expPlayer += exp;
        this.score += scores;
        SkillManager.Instance.CanIncreasePoint(expPlayer);
        UIManager.Instance.SetExpUi(expPlayer);
        Debug.Log(score);
        SaveDataManager.Instance.SaveScore(score);
        SaveDataManager.Instance.SaveExp(expPlayer);
    }
    public void GameOver()
    {
        ButtonPause();
        UIManager.Instance.GameOver();
        GameManager.Instance.DeleteKey();
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
        totalTime = 0;
    }
   
    public void Victory()
    {
        ButtonPause();
        if(score >= highScore)
        {
            highScore = score;
            SaveDataManager.Instance.SaveHighScore(highScore);
            Debug.Log("Save highSocer");
        }
        UIManager.Instance.Victory(score,totalTime,highScore);
        GameManager.Instance.DeleteKey();
    }
    public void StartLevelBoss()
    {
        if (isStartLvBoss)
        {
            isStartLvBoss = false;
            IInteractable interactable = doorStartlvBoss.gameObject.GetComponent<IInteractable>();
            interactable.InteractOff();
            UIManager.Instance.HPBoss_Slider.gameObject.SetActive(true);
        }
    }
    public void EndLevelBoss()
    {
        IInteractable interactable = doorEndlvBoss.gameObject.GetComponent<IInteractable>();
        interactable.InteractOn();
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);
        UIManager.Instance.HPBoss_Slider.gameObject.SetActive(false);
    }

    public void SetTotalTime(int totalTime)
    {
        this.totalTime = totalTime;
    }
    public void SetHighScore(int highScore)
    {
        this.highScore = highScore;
    }
    private IEnumerator UpdateTimeIngame()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);
            totalTime += 1;
            SaveDataManager.Instance.SaveTotalTime(totalTime);
        }
    }
}
