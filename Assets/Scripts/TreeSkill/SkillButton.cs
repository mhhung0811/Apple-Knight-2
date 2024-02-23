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
    
    public void PressSkillButton()
    {
        SkillManager.Instance.activeSkill = this.transform.GetComponent<SkillUp>();

        skillIamge.sprite = SkillManager.Instance.skills[skillButtonId - 1].skillSprite;
        skillNameText.text = SkillManager.Instance.skills[skillButtonId - 1].skillName;
        skillDesText.text = SkillManager.Instance.skills[skillButtonId - 1].skillDes;
    }
}
