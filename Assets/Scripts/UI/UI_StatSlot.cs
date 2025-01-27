using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    private UIController ui;

    [SerializeField] private string statName;
    [SerializeField] private BuffType statType;

    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    [TextArea]
    [SerializeField] private string statDescription;

    private void OnValidate()
    {
        gameObject.name = "Stat-" + statName;

        if(statName != null)
        {
            statNameText.text = statName;
        }
    }
    private void Start()
    {
        UpdateStatValueUI();
        ui=GetComponentInParent<UIController>();
    }
    public void UpdateStatValueUI()
    {   
        //TODO:更新属性值
        CharacterStats stats = PlayerManager.instance.player.GetComponent<CharacterStats>();

        if(stats != null )
        {
            statValueText.text=stats.GetWantedStat(statType).GetValue().ToString();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {   
        Vector2 mousePostion=Input.mousePosition;
        ui.statTips.ShowStatTip(statDescription,mousePostion);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statTips.HideStatTip();
    }
}
