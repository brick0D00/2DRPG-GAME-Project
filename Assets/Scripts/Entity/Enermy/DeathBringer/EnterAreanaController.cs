using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnterAreanaController : MonoBehaviour
{
    [SerializeField] private GameObject BossUI;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI BossName;
    [SerializeField] private EnermyStats bossState;


    private void OnTriggerEnter2D()
    {   
        //TODO:进入区域触发战斗
        BossUI.SetActive(true);

        BossName.text = "DeathBringer";

        bossState.OnHealthChanged += UpdateHealthUI;

    }
    private void UpdateHealthUI()
    {
        slider.maxValue = bossState.Caculate_MaxHealthValue();
        slider.value = bossState.currentHealth;
    }
}
