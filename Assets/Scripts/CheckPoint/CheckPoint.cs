using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Animator anim;
    public string CheckPointID;
    public bool isActivated;//�жϼ����Ƿ񼤻�
    private void Awake()
    {
        anim= GetComponent<Animator>();
    }

    [ContextMenu("GenerateCheckPointID")]
    private void GenerateCheckPointID()
    {
        //TODO:�Զ����ɼ���ID
        CheckPointID = System.Guid.NewGuid().ToString();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            ActivateCheckPoint();
        }
    }

    public void ActivateCheckPoint()
    {
        anim.SetBool("active", true);
        isActivated = true;
    }
}
