using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillTips : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;

    public void ShowSkillTips(string _skillName,string _description)
    {   
        skillName.text = _skillName;
        skillDescription.text = _description;

        skillName.gameObject.SetActive(true);
        skillDescription.gameObject.SetActive(true);
    }
    public void HideSkillTips()
    {
        skillName.gameObject.SetActive(false);  
        skillDescription.gameObject.SetActive(false);
    }
}
