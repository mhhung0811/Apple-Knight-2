using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public Image skillIamge;
    public TextMeshProUGUI skillNameText;
    public TextMeshProUGUI skillDesText;

    public int skillButtonId;
    private void Start()
    {
        if(skillButtonId == 10)
        {
            SkillManager.Instance.activeSkill = this.transform.GetComponent<SkillUp>();
            this.gameObject.GetComponent<SkillUp>().isUpgrade = true;
            skillIamge.sprite = SkillManager.Instance.skills[9].skillSprite;
            skillNameText.text = SkillManager.Instance.skills[9].skillName;
            skillDesText.text = SkillManager.Instance.skills[9].skillDes;
            SkillManager.Instance.UpdateAbilityButton();
        }
    }
    public void PressSkillButton()
    {
        SkillManager.Instance.activeSkill = this.transform.GetComponent<SkillUp>();

        skillIamge.sprite = SkillManager.Instance.skills[skillButtonId - 1].skillSprite;
        skillNameText.text = SkillManager.Instance.skills[skillButtonId - 1].skillName;
        skillDesText.text = SkillManager.Instance.skills[skillButtonId - 1].skillDes;
    }

}
