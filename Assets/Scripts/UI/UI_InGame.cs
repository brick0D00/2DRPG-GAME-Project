using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{   
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;

    [SerializeField] private Image dashImage;
    [SerializeField] private float dashCoolDown;

    [SerializeField] private Image timeRewindImage;
    [SerializeField] private float timeRewindCoolDown;

    [SerializeField] private Image blackHoleImage;
    [SerializeField] private float blackHoleCoolDown;

    [SerializeField] private Image ParryImage;
    [SerializeField] private float ParryCoolDown;

    [SerializeField] private Image flaskItemImage;
    [SerializeField] private float flaskItemCoolDown;

    [Header("金币信息")]
    [SerializeField] private TextMeshProUGUI currentMoneyAmount;
    [SerializeField] private float moneyAmount;
    [SerializeField] private float increaseRate;

    private SkillManager skill;

    private void Start()
    {   
        skill=SkillManager.instance;
        if(playerStats != null)
        {   
            //增加委托
            playerStats.OnHealthChanged += UpdateHealthUI;
        }

        //设置冷却时间
        dashCoolDown=skill.dash.coolDownSetTime;
        timeRewindCoolDown = skill.rewindTime.coolDownSetTime;
        blackHoleCoolDown=skill.blackHole.coolDownSetTime;
        ParryCoolDown=skill.parry.coolDownSetTime;
        flaskItemCoolDown=Inventory.instance.flaskCoolDown;
    }


    private void Update()
    {
        UpdateMoneyAmountUI();

        CheckImageFillAmount(dashImage, dashCoolDown);
        CheckImageFillAmount(timeRewindImage, timeRewindCoolDown);
        CheckImageFillAmount(blackHoleImage, blackHoleCoolDown);
        CheckImageFillAmount(ParryImage, ParryCoolDown);
        CheckImageFillAmount(flaskItemImage, flaskItemCoolDown);

    }

    private void UpdateMoneyAmountUI()
    {
        //改动金币
        if (moneyAmount < PlayerManager.instance.GetCurrentCurrencyAmount())
        {
            moneyAmount += Time.deltaTime * increaseRate;
        }
        else
        {
            moneyAmount = PlayerManager.instance.GetCurrentCurrencyAmount();
        }

        currentMoneyAmount.text = ((int)moneyAmount).ToString("#,#");
    }

    /*private void CheckForInput()
    {   
        //TODO:检测技能按键输入

        //冲刺
        if (Input.GetKeyUp(KeyCode.LeftShift)&&skill.dash.isDashUnlocked)
        {
            SetUpImageCoolDown(dashImage);
        }
        //时间裂隙
        if (Input.GetKeyUp(KeyCode.R)&&skill.rewindTime.isRewindTimeUnlocked)
        {
            SetUpImageCoolDown(timeRewindImage);
        }
        //黑洞
        if (Input.GetKeyUp(KeyCode.Z)&&skill.blackHole.isBlackHoleUnlocked)
        {
            SetUpImageCoolDown(blackHoleImage);
        }
        //嗑药
        if (Input.GetKeyUp(KeyCode.Alpha1)&&Inventory.instance.GetWantedEquipment(EquipmentType.Flask)!=null)
        {
            SetUpImageCoolDown(flaskItemImage);
        }

    }
    */

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.Caculate_MaxHealthValue();
        slider.value = playerStats.currentHealth;
    }
    private void SetUpImageCoolDown(Image _skillImage)
    {   
        //TODO:让技能图像变黑显示冷却
        if (_skillImage.fillAmount <= 0)
        {
            _skillImage.fillAmount = 1;
        }
    }
    private void CheckImageFillAmount(Image _skillImage,float _skillCoolDown)
    {   
        //TODO:每帧更新冷却图片

        if (_skillImage.fillAmount> 0)
        {
            _skillImage.fillAmount -= 1 / _skillCoolDown * Time.deltaTime;          
        }
    }
    #region SetUpImageForSkills
    public void SetUpImageForDash()
    {
        SetUpImageCoolDown(dashImage);
    }
    public void SetUpImageForRewindTime()
    {
        SetUpImageCoolDown(timeRewindImage);
    }
    public void SetUpImageForBlackHole()
    {
        SetUpImageCoolDown(blackHoleImage);
    }
    public void SetUpImageForFlask()
    {
        SetUpImageCoolDown(flaskItemImage);
    }
    public void SetUpImageForParry()
    {
        SetUpImageCoolDown(ParryImage);
    }
    #endregion

}
