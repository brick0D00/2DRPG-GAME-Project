using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using System.Linq;
public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    private GameData gameData;
    private List<ISaveManager> saveManagers;//接口列表

    private FileDataHandler fileDataHandler;
    [SerializeField] private string fileName;

    [ContextMenu("Delete saved file")]//这样就能在Unity界面直接删除
    public void DeleteSavedData()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        fileDataHandler.DeleteFileData();
    }

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
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);//偷偷写入C盘
        saveManagers = FindAllSaveManager();
        LoadGame();
    }

    public void NewGmae()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {

        //从句柄处读取数据 game = from dataHandle
        gameData = fileDataHandler.LoadData();

        if(gameData == null)
        {
            NewGmae();
        }

        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
    }
    public void SaveGame()
    {   
        foreach(ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref  gameData);
        }
        fileDataHandler.SaveData(gameData);
    }
    private void OnApplicationQuit()
    {   
        //TODO:退出游戏自动保存
        SaveGame();
    }
    private List<ISaveManager> FindAllSaveManager()
    {   
        //TODO:用迭代器遍历找到所有对接口调用，这样避免了一次次的手动绑定
        IEnumerable<ISaveManager> saveManagers=FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
        return new List<ISaveManager>(saveManagers);
    }

    public bool HaveSaveDData()
    {   
        //TODO:查看是否拥有保存的数据
        if(fileDataHandler.LoadData() == null)
        {
            return false;
        }
        return true;
    }
}
