using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public enum EquipmentType
{
    Weapon,//����
    Aromr,//����
    Jewelry,//��Ʒ
    Flask//ҩƿ
}
[CreateAssetMenu(fileName = "New Item Date", menuName = "Data/Equipment")]//���б�������ѡ��
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;
    public float euipmentCoolDown;

    [Header("װ��Ч��")]
    [SerializeField] List<ItemEffect> itemEffects;

    [Header("��������")]
    public int strength;/*����������������*/
    public int intelligence;/*������Ӱ��ħ���˺�*/
    public int vitality;/*�����������������ֵ*/
    public int aromr;/*����*/
    public int antiMagic;/*ħ��*/

    

    [Header("�ϳ�����")]
    public List<InventoryItem> craftRquest;//�ϳ��������

    #region ApplyItemEffects
    public void ApplyItemEffectsForValue()
    {
        for (int i = 0; i < itemEffects.Count; i++)
        {
            itemEffects[i].ExecuteEffectsForValue();
        }
        Inventory.instance.UpdateUIStatSlots();
    }
    public void RemoveItemEffectsForValue()
    {
        for (int i = 0; i < itemEffects.Count; i++)
        {
            itemEffects[i].RemoveEffectsForValue();
        }
        Inventory.instance.UpdateUIStatSlots();
    }
    public void ApplyItemEffectsForAttack(Vector3 _enermyPostion)
    {
        for (int i = 0; i < itemEffects.Count; i++)
        {
            itemEffects[i].ExecuteEffectsForAttack(_enermyPostion);
        }
        Inventory.instance.UpdateUIStatSlots();
    }
    public void ApplyItemEffectsForDefend()
    {
        for (int i = 0; i < itemEffects.Count; i++)
        {
            itemEffects[i].ExecuteEffectsForDefend();
        }
        Inventory.instance.UpdateUIStatSlots();
    }
    public void ApplyItemEffectsForThrowSword(Vector3 _enermyPostion)
    {
        for (int i = 0; i < itemEffects.Count; i++)
        {
            itemEffects[i].ExecuteEffectsForThrowSword(_enermyPostion);
        }
        Inventory.instance.UpdateUIStatSlots();
    }
    public void ApplyItemEffectsForDrink()
    {
        for (int i = 0; i < itemEffects.Count; i++)
        {
            itemEffects[i].ExecuteEffectsForDrink();
        }
        Inventory.instance.UpdateUIStatSlots();
    }
    #endregion
    #region Add&RemoveModifiers
    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);
        playerStats.armor.AddModifier(aromr);
        playerStats.antiMagic.AddModifier(antiMagic);
    }
    public void RemoveModifiers() 
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        playerStats.strength.RemoveModifier(strength);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);
        playerStats.armor.RemoveModifier(aromr);
        playerStats.antiMagic.RemoveModifier(antiMagic);
    }
    #endregion
    public override string GetDescription()
    {   
        sb.Length = 0;
        AddDescription(strength, "����");
        AddDescription(intelligence, "�ǻ�");
        AddDescription(vitality, "����");
        AddDescription(aromr, "����");
        AddDescription(antiMagic, "ħ��");

        AddDescription(itemDescription);

        return sb.ToString();
    }
    public void AddDescription(int _value,string _name)
    {
        if (_value != 0)
        {
            if (sb.Length > 0)
            {
                sb.AppendLine();
            }
            if (_value > 0)
            {
                sb.Append( "+"+ _value+" "+_name);
            }
        }
    }
    public void AddDescription(string _myDescription)
    {
        if(sb.Length > 0)
        {
            sb.AppendLine();
            sb.Append(_myDescription);
        }
        else
        {
            sb.AppendLine(_myDescription);
        }

    }
}
