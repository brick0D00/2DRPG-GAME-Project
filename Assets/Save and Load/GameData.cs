using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameData 
{   
    //json�ļ�ֻ�ܱ��������������Ҳ�㹻��
    public int currency;

    //���������ֿ⣬װ����
    public SerializableDictionary<string, int> inventory;
    public List<string> equipmentsID;

    //���漼����
    public SerializableDictionary<string, bool> skillTree;

    //�������
    public SerializableDictionary<string, bool> checkPoints;
    public string lastestCheckPointID;

    //������������
    public SerializableDictionary<string, float> volumeSettings;
    public GameData()
    {
        this.currency = 0;

        inventory = new SerializableDictionary<string, int>();
        equipmentsID = new List<string>();

        skillTree = new SerializableDictionary<string, bool>();

        checkPoints = new SerializableDictionary<string, bool>();
        lastestCheckPointID=string.Empty;

        volumeSettings = new SerializableDictionary<string, float>();
    }
}
