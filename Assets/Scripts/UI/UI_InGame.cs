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

    [Header("�����Ϣ")]
    [SerializeField] private TextMeshProUGUI currentMoneyAmount;
    [SerializeField] private float moneyAmount;
    [SerializeField] private float increaseRate;

    private SkillManager skill;

    private void Start()
    {   
        skill=SkillManager.instance;
        if(playerStats != null)
        {   
            //����ί��
            playerStats.OnHealthChanged += UpdateHealthUI;
        }

        //������ȴʱ��
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
        //�Ķ����
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
        //TODO:��⼼�ܰ�������

        //���
        if (Input.GetKeyUp(KeyCode.LeftShift)&&skill.dash.isDashUnlocked)
        {
            SetUpImageCoolDown(dashImage);
        }
        //ʱ����϶
        if (Input.GetKeyUp(KeyCode.R)&&skill.rewindTime.isRewindTimeUnlocked)
        {
            SetUpImageCoolDown(timeRewindImage);
        }
        //�ڶ�
        if (Input.GetKeyUp(KeyCode.Z)&&skill.blackHole.isBlackHoleUnlocked)
        {
            SetUpImageCoolDown(blackHoleImage);
        }
        //�ҩ
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
        //TODO:�ü���ͼ������ʾ��ȴ
        if (_skillImage.fillAmount <= 0)
        {
            _skillImage.fillAmount = 1;
        }
    }
    private void CheckImageFillAmount(Image _skillImage,float _skillCoolDown)
    {   
        //TODO:ÿ֡������ȴͼƬ

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
