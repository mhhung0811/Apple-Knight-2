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
        Point = 5;
        DisPlayTextPoint();
        UpdateAbilityButton();
    }
    private void DisPlayTextPoint()
    {
        pointText.text = Point + "/5";
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
            upgradeText.text = "Không đủ điểm để nâng cấp!";
            StartCoroutine(VisibilityTextUpgrade());
        }
        DisPlayTextPoint();
        UpdateAbilityButton();
    }

    private void UpdateAbilityButton()
    {
        for(int i = 0; i < 9; i++)
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

        if(id == 1|| id == 4 || id == 7)
        {
            activeSkill.gameObject.GetComponent<SkillUp>().isUpgrade = true;
            Point--;
            player.gameObject.GetComponent<PlayerBehavior>().UpgradeSkill(id);
            return;
        }
        else
        {
            bool skillPrevious =  skills[id - 2].gameObject.GetComponent<SkillUp>().isUpgrade;
            if(!skillPrevious)
            {
                upgradeText.gameObject.SetActive(true);
                upgradeText.text = "Chưa thể nâng cấp skill này!";
                StartCoroutine(VisibilityTextUpgrade());
            }
            else
            {
                activeSkill.gameObject.GetComponent<SkillUp>().isUpgrade = true;
                Point--;
                player.gameObject.GetComponent<PlayerBehavior>().UpgradeSkill(id);
            }
        }
    }
}
