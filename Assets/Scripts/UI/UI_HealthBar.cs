using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity;
    private CharacterStats myStats;
    private RectTransform rt;
    private Slider slider;

    void Start()
    {
        entity=GetComponentInParent<Entity>();
        rt= GetComponent<RectTransform>();
        slider= GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<CharacterStats>();

        UpdateHealthUI();
        entity.OnFlipped+= FlipUI;//函数变量？
        myStats.OnHealthChanged += UpdateHealthUI;
    }
    private void Update()
    {
        //UpdateHealthUI();
    }
    private void OnDisable()
    {   
        //结束后移除事件委托

        entity.OnFlipped -=FlipUI;
        myStats.OnHealthChanged -= UpdateHealthUI;
    }
    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.Caculate_MaxHealthValue();
        slider.value = myStats.currentHealth;
    }
    private void FlipUI()
    {
        rt.Rotate(0, 180, 0);
    }
}
