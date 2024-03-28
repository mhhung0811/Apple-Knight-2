using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
            }
            return _instance;
        }
    }

    public RawImage MenuGameOver;
    public Image MenuImage;

    public Button buttonUntil;
    public TextMeshProUGUI HP_Text;
    public TextMeshProUGUI Mana_Text;
    public TextMeshProUGUI Exp_Text;
    public TextMeshProUGUI NotEnoughMana_Text;
    //Victory UI
    public TextMeshProUGUI TextHighScore;
    public TextMeshProUGUI TextScore;
    public TextMeshProUGUI TextTime;

    public Slider HPBoss_Slider;
    public Slider Exp_Slider;
    public Slider Mana_Slider;
    public Slider HP_Slider;
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        // Load
    }

    void Update()
    {
        
    }

    public void GameOver()
    {
        MenuGameOver.gameObject.SetActive(true);
    }
    public void Victory(int score, int totalTime)
    {
        MenuImage.gameObject.SetActive(true);
        SetUIVictory(score, totalTime);
        InGameManager.Instance.ButtonPause();
    }
    public void SetHPUi(float value,float valuemax)
    {
        HP_Slider.maxValue = valuemax;
        HP_Slider.value = value;
        HP_Text.text = value.ToString() + "/" + valuemax.ToString();
    }
    public void SetManaUi(float value,float valuemax)
    {
        Mana_Slider.maxValue = valuemax;
        Mana_Slider.value = value;
        Mana_Text.text = value.ToString() + "/" + valuemax.ToString();
    }
    public void SetExpUi(int exp)
    {
        Exp_Slider.value = exp % 100;
        Exp_Text.text = "Lv." + (exp / (int)100).ToString();
    }
    public void SetHPBossUI(float value)
    {
        HPBoss_Slider.value = value;
    }
    public void NotEnoughMana()
    {
        StartCoroutine(VisibleTextManaNotEnough());
    }
    public void OpenButtonUntil()
    {
        buttonUntil.gameObject.SetActive(true);
    }
    private IEnumerator VisibleTextManaNotEnough()
    {
        NotEnoughMana_Text.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        NotEnoughMana_Text.gameObject.SetActive(false);
    }
    
    public void Save()
    {

    }
    public void SetUIVictory(int score, int totalTime)
    {
        TextScore.text = score.ToString();
        int hours = totalTime / 3600;
        int minutes = (totalTime - hours*3600)/60;
        int second = totalTime % 60;
        TextTime.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, second);
    }
}
