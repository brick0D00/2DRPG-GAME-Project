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
    {   //TODO:���¿�ʼ��Ϸ
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
                //�ȶ�ID�����Ҽ���
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
            //�ƶ������µļ���
            if (checkPoint.CheckPointID == lastestCheckPointID)
            {
                player.transform.position = checkPoint.transform.position;
            }
        }
    }

    public void SaveData(ref GameData _data)
    {    
        //TODO:�������
        _data.checkPoints.Clear();
        foreach(var checkpoint in checkPoints)
        {
            _data.checkPoints.Add(checkpoint.CheckPointID,checkpoint.isActivated);
        }

        //�������µ�һ������
        _data.lastestCheckPointID=FindLastestCheckPoint().CheckPointID;

    }

    private CheckPoint FindLastestCheckPoint()
    {   
        //TODO:�ҳ����µ�һ�������
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
