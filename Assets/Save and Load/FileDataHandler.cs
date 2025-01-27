using System;
using System.IO;
using UnityEngine;
//��Ϊ���ļ���д�����ݵľ��
public class FileDataHandler
{
    //TODO:�������ڴ���н��������ļ�����д��Ͷ�ȡ

    private string dataDirPath = "";//�ļ�·��
    private string dataFileName = "";//�ļ���

    public FileDataHandler(string _dataDataPath, string _dataFileName)
    {
        this.dataDirPath = _dataDataPath;
        this.dataFileName = _dataFileName;

    }

    public void SaveData(GameData _gameData)
    {
        //TODO:����������д���ļ�
        //����·��
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //ת����json����������
            string dataToStore = JsonUtility.ToJson(_gameData, true);

            //������д�� using ���ȷ������ʹ�ú��ڴ���Եõ��ͷ�,create��ֱ�Ӵ������߸���
            //FileStream����Java���﷨
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("����ʧ�� " + fullPath + "\n" + e);
        }
    }

    public GameData LoadData()
    {
        //TODO:���ļ��м�������
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                //��json�ļ�ת��GameData
                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("����ʧ�� " + fullPath + "\n" + e);
            }

        }
        return loadData;
    }
    public void DeleteFileData()
    {
        //TODO:ɾ���ļ�
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}
