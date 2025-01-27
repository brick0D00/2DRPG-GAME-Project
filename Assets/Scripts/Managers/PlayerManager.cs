using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour,ISaveManager
{   //TODO:ͨ������ģʽ��Player���д��ݽ�����Դ����
    public static PlayerManager instance;
    public Player player;
    public int currency;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
        instance = this;
        }
    }
    public void GainMoney(int _amount)
    {
        currency += _amount;
    }
    public bool HaveEnoughMoney(int _amount)
    {
        if (currency >= _amount)
        {
            currency-=_amount;
            return true;
        }
        else
        {
            return false;
        }
    }
    public int GetCurrentCurrencyAmount()
    {
        return currency;
    }

    public void LoadData(GameData _data)
    {
        this.currency = _data.currency;
    }

    public void SaveData(ref GameData _data)
    {
        _data.currency = this.currency;
    }
    public void AddMoney(int _amount)
    {
        currency += _amount;
    }
}
