using System;
using System.IO;
using UnityEngine;
//作为向文件中写入数据的句柄
public class FileDataHandler
{
    //TODO:用来和内存进行交互，对文件进行写入和读取

    private string dataDirPath = "";//文件路径
    private string dataFileName = "";//文件名

    public FileDataHandler(string _dataDataPath, string _dataFileName)
    {
        this.dataDirPath = _dataDataPath;
        this.dataFileName = _dataFileName;

    }

    public void SaveData(GameData _gameData)
    {
        //TODO:将保存数据写入文件
        //完整路径
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //转换成json数据流保存
            string dataToStore = JsonUtility.ToJson(_gameData, true);

            //数据流写入 using 语句确保类在使用后内存可以得到释放,create会直接创建或者覆盖
            //FileStream类似Java的语法
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
            Debug.LogError("保存失败 " + fullPath + "\n" + e);
        }
    }

    public GameData LoadData()
    {
        //TODO:从文件中加载数据
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
                //从json文件转回GameData
                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("加载失败 " + fullPath + "\n" + e);
            }

        }
        return loadData;
    }
    public void DeleteFileData()
    {
        //TODO:删除文件
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}
