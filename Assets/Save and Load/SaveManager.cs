using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using System.Linq;
public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    private GameData gameData;
    private List<ISaveManager> saveManagers;//�ӿ��б�

    private FileDataHandler fileDataHandler;
    [SerializeField] private string fileName;

    [ContextMenu("Delete saved file")]//����������Unity����ֱ��ɾ��
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
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);//͵͵д��C��
        saveManagers = FindAllSaveManager();
        LoadGame();
    }

    public void NewGmae()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {

        //�Ӿ������ȡ���� game = from dataHandle
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
        //TODO:�˳���Ϸ�Զ�����
        SaveGame();
    }
    private List<ISaveManager> FindAllSaveManager()
    {   
        //TODO:�õ����������ҵ����жԽӿڵ��ã�����������һ�δε��ֶ���
        IEnumerable<ISaveManager> saveManagers=FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
        return new List<ISaveManager>(saveManagers);
    }

    public bool HaveSaveDData()
    {   
        //TODO:�鿴�Ƿ�ӵ�б��������
        if(fileDataHandler.LoadData() == null)
        {
            return false;
        }
        return true;
    }
}
