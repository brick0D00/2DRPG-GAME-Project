using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,ISaveManager
{
    private UIController ui;
    [SerializeField] private int skillCost;
    [SerializeField]private string skillName;
    [TextArea]
    [SerializeField]private string skillDescription;
    [SerializeField] private Color skillLockedColor;



    public bool isUnlocked;

    //�ֱ������������¼Ӧ�������Ĳ�λ�Լ���������
    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;
    private Image skillImage;

    private void OnValidate()
    {
        gameObject.name="SkillTree_UI-"+skillName;
    }
    private void Awake()
    {   
        ui=GetComponentInParent<UIController>();
        skillImage = GetComponent<Image>();

        skillImage.color=skillLockedColor;
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }
    private void Start()
    {
        if (isUnlocked)
        {
            skillImage.color=Color.white;
        }
    }
    public void UnlockSkillSlot()
    {
        //TODO:�������ǰ�õļ�������

        if (isUnlocked)
        {
            return;
        }

        for(int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].isUnlocked == false)
            {
                Debug.Log("false");
                return;
            }
        }
        for(int i = 0;i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].isUnlocked == true)
            {   
                Debug.Log("false");
                return;
            }
        }

        //��һ��Ǯ����������
        if (PlayerManager.instance.HaveEnoughMoney(skillCost) == false)
        {
            return;
        }

        isUnlocked =true;
        skillImage.color=Color.white;
        AudioManager.instance.PlaySFX(9, null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillTips.ShowSkillTips(skillName,skillDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillTips.HideSkillTips();
    }

    public void LoadData(GameData _data)
    {   
        //���ؼ���
        if(_data.skillTree.TryGetValue(skillName, out bool value))
        {
            isUnlocked=value;
        }
    }

    public void SaveData(ref GameData _data)
    {   
        //�����鵽֮ǰ����������Ƴ�֮ǰ�ļ���
        if(_data.skillTree.TryGetValue(skillName, out bool value))
        {
            _data.skillTree.Remove(skillName);
        }

        _data.skillTree.Add(skillName,isUnlocked);
    }
}
