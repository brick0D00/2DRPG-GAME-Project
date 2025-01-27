using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour,ISaveManager
{   
    public static GameManager instance;
    [SerializeField] private CheckPoint[] checkPoints;
    [SerializeField]private Player player;
    [SerializeField]private string lastestCheckPointID;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance=this;
        }

        checkPoints = FindObjectsOfType<CheckPoint>();
    }
    public void RestartScene()
    {   //TODO:重新开始游戏
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data)
    {
        foreach (var pair in _data.checkPoints)
        {
            foreach (CheckPoint checkPoint in checkPoints)
            {
                //比对ID，并且激活
                if (pair.Key == checkPoint.CheckPointID && pair.Value == true)
                {
                    checkPoint.ActivateCheckPoint();

                }
            }
        }
        lastestCheckPointID=_data.lastestCheckPointID;
        Invoke("MoveToLastestCheckPoint", .1f);
    }

    private void MoveToLastestCheckPoint()
    {
        foreach (CheckPoint checkPoint in checkPoints)
        {
            //移动至最新的检查点
            if (checkPoint.CheckPointID == lastestCheckPointID)
            {
                player.transform.position = checkPoint.transform.position;
            }
        }
    }

    public void SaveData(ref GameData _data)
    {    
        //TODO:保存检查点
        _data.checkPoints.Clear();
        foreach(var checkpoint in checkPoints)
        {
            _data.checkPoints.Add(checkpoint.CheckPointID,checkpoint.isActivated);
        }

        //保存最新的一个检查点
        _data.lastestCheckPointID=FindLastestCheckPoint().CheckPointID;

    }

    private CheckPoint FindLastestCheckPoint()
    {   
        //TODO:找出最新的一个保存点
        CheckPoint lastestCheckPoint= null;
        float closestDistance=Mathf.Infinity;

        foreach(var  checkPoint in checkPoints)
        {
            float distanceToPlayer = Vector2.Distance(player.transform.position, checkPoint.transform.position);
            
            if(checkPoint.isActivated && distanceToPlayer < closestDistance)
            {
                closestDistance = distanceToPlayer;
                lastestCheckPoint = checkPoint;
            }

        }

        return lastestCheckPoint;
    }

    public void PauseGame(bool _pause)
    {
        if (_pause == true)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
