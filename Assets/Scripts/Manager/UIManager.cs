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

    public Slider HPBoss_Slider;

    public Slider HP_Slider;
    public TextMeshProUGUI HP_Text;
    public Slider Mana_Slider;
    public TextMeshProUGUI Mana_Text;
    public Slider Exp_Slider;
    public TextMeshProUGUI Exp_Text;
    public TextMeshProUGUI NotEnoughMana_Text;
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
        
    }

    void Update()
    {
        
    }

    public void GameOver()
    {
        MenuGameOver.gameObject.SetActive(true);
    }
    public void Victory()
    {
        MenuImage.gameObject.SetActive(true);
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
    private IEnumerator VisibleTextManaNotEnough()
    {
        NotEnoughMana_Text.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        NotEnoughMana_Text.gameObject.SetActive(false);
    }
}
