using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    private static SkillManager _instance;

    public SkillUp[] skills;
    public SkillButton[] skillButtons;
    public int Point;
    private int level;

    public TextMeshProUGUI pointText;
    public TextMeshProUGUI upgradeText;
    public SkillUp activeSkill;
    public GameObject player;
    public static SkillManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<SkillManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        Point = 0;
        DisPlayTextPoint();
        UpdateAbilityButton();
    }
    public void DisPlayTextPoint()
    {
        pointText.text = "Upgrade point: " + Point.ToString();
    }

    public void PressUpgradeButton()
    {
        if(Point > 0)
        {
            CanUpgradeSkill();
        }
        else
        {
            upgradeText.gameObject.SetActive(true);
            upgradeText.text = "Not enough points to upgrade!";
            StartCoroutine(VisibilityTextUpgrade());
        }
        DisPlayTextPoint();
        UpdateAbilityButton();
    }
    public void CanIncreasePoint(int exp)
    {
        if(exp/100 > level)
        {
            level = exp / (int)100;
            Point++;
        }
        DisPlayTextPoint();
    }

    public void UpdateAbilityButton()
    {
        for(int i = 0; i < 10; i++)
        {
            if (skills[i].isUpgrade)
            {
                skills[i].GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
            }
            else
            {
                skills[i].GetComponent<Image>().color = new Vector4(0.15f, 0.45f, 0.45f, 1);
            }
        }
    }
    
    private IEnumerator VisibilityTextUpgrade()
    {
        yield return new WaitForSeconds(0.5f);
        upgradeText.gameObject.SetActive(false);
    }
    private void CanUpgradeSkill()
    {
        int id = activeSkill.gameObject.GetComponent<SkillButton>().skillButtonId;
        bool isUpgrade = activeSkill.gameObject.GetComponent<SkillUp>().isUpgrade;
        if(isUpgrade == true)
        {
            upgradeText.gameObject.SetActive(true);
            upgradeText.text = "This skill has been upgraded!";
            StartCoroutine(VisibilityTextUpgrade());
            return;
        }

        if(id == 1|| id == 4 || id == 7)
        {
            activeSkill.gameObject.GetComponent<SkillUp>().isUpgrade = true;
            Point--;
            SaveDataManager.Instance.SaveSkillUpgraded(id);
            player.gameObject.GetComponent<PlayerBehavior>().UpgradeSkill(id);
            return;
        }
        else
        {
            bool skillPrevious =  skills[id - 2].gameObject.GetComponent<SkillUp>().isUpgrade;
            if(!skillPrevious)
            {
                upgradeText.gameObject.SetActive(true);
                upgradeText.text = "This skill cannot be upgraded!";
                StartCoroutine(VisibilityTextUpgrade());
            }
            else
            {
                activeSkill.gameObject.GetComponent<SkillUp>().isUpgrade = true;
                Point--;
                SaveDataManager.Instance.SaveSkillUpgraded(id);
                player.gameObject.GetComponent<PlayerBehavior>().UpgradeSkill(id);
                if(id == 3|| id == 6|| id == 9)
                {
                    UIManager.Instance.OpenButtonUntil();
                }
            }
        }
    }
    public void SetUpgradeSkill(int id)
    {
        skills[id-1].isUpgrade = true;
        player.gameObject.GetComponent<PlayerBehavior>().UpgradeSkill(id);
        if (id == 3 || id == 6 || id == 9)
        {
            UIManager.Instance.OpenButtonUntil();
        }
    }
    public void SetPointUpgrade(int exp,int pointUpgraded)
    {
        this.Point = exp/100 - pointUpgraded;
        DisPlayTextPoint();
    }
}
