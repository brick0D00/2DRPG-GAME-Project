using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameData 
{   
    //json文件只能保存基础变量，但也足够了
    public int currency;

    //道具栏，仓库，装备栏
    public SerializableDictionary<string, int> inventory;
    public List<string> equipmentsID;

    //保存技能树
    public SerializableDictionary<string, bool> skillTree;

    //保存检查点
    public SerializableDictionary<string, bool> checkPoints;
    public string lastestCheckPointID;

    //保存音量设置
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
