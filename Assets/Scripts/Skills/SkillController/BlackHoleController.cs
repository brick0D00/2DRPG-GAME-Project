using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleController : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    public float maxSize;/*黑洞能成长的最大尺寸*/
    public float growSpeed;
    public bool canGrow;
    public bool canShrink;
    public float shrinkSpeed;/*坍缩速度*/

    public bool canAttack;
    public bool canCreateHotKey=true;
    public int amountOfTargets=4;
    public float cloneAttackCoolDown=.3f;
    public float cloneTimer;

    public float blackHoleTimer;

    private List<Transform> Targets= new List<Transform>();/*用来记录qte收入的攻击目标*/
    private List<GameObject> UsedHotKey= new List<GameObject>();/*删除hotkey*/

    public bool canPlayExitBlackHoleState {  get; private set; }


    public void SetUpBlackHole(bool _canShrink,bool _canGrow,bool _canAttack,float _maxSize, int _amountOfTargets,float _blackHoleDuration)
    {   
        canShrink = _canShrink;
        canAttack = _canAttack;
        canGrow = _canGrow;
        maxSize = _maxSize;
        amountOfTargets = _amountOfTargets;
        blackHoleTimer = _blackHoleDuration;
    }
    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        blackHoleTimer -= Time.deltaTime;

        if (blackHoleTimer < 0)
        {   
            blackHoleTimer = Mathf.Infinity;/*换成无穷避免重复执行*/
            if(Targets.Count > 0)
            {
                LetUsAttack();
            }
            else
            {   
                ExitBlackHoleState();
            }
        }

        if (Input.GetKeyDown(KeyCode.R)&&canAttack==false)
        {
            LetUsAttack();
        }
        

        CloneAttackLogic();

        GrowShrinkController();

    }

    private void LetUsAttack()
    {   
        if(Targets.Count== 0) { return; }
        canAttack = true;
        canCreateHotKey = false;
        DestoryHotKey();

        //角色透明
        PlayerManager.instance.player.MakeItTransparent(true);
    }

    private void CloneAttackLogic()
    {
        if (Targets.Count == 0) { return; }
        //TODO:创建克隆体攻击
        if (cloneTimer < 0 && canAttack == true && amountOfTargets>0)
        {
            cloneTimer = cloneAttackCoolDown;

            int randomIndex = Random.Range(0, Targets.Count);

            float xOffset;
            if (Random.Range(1, 10) >= 5)
            {
                xOffset = 1.5f;
            }
            else
            {
                xOffset = -1.5f;
            }
            SkillManager.instance.clone.CreateClone(Targets[randomIndex], new Vector3(xOffset, 0, 0));

            amountOfTargets--;
            if (amountOfTargets <= 0)
            {
                canAttack = false;
                canShrink = true;
                Invoke("ExitBlackHoleState", 0.5f);/*确保在攻击完成后结束*/

            }
        }
    }

    private void GrowShrinkController()
    {
        if (canGrow && !canShrink)
        {
            //TODO:让黑洞丝滑长大
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {   
        //TODO:将碰到黑洞的敌人时停，同时创建hotkey

        if(collision.GetComponent<Enermy>() != null)
        {

            collision.GetComponent<Enermy>().SetFreeze(true);
            CreateHotKey(collision);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {   
        //TODO:当黑洞消失的时候解冻
        if (collision.GetComponent<Enermy>() != null)
        {

            collision.GetComponent<Enermy>().SetFreeze(false);
        }
    }

    private void CreateHotKey(Collider2D collision)
    {   
        //TODO:创建热键

        if(keyCodeList.Count <= 0 || canCreateHotKey==false) 
        {   //这里满员了
            return;    
        }

        //创造一个预制体
        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        UsedHotKey.Add(newHotKey);
        //随机抽取一个KeyCode再移除
        KeyCode chosenKey = keyCodeList[Random.Range(0, keyCodeList.Count - 1)];
        keyCodeList.Remove(chosenKey);

        BlackHotKeyController blackHotKeyController = newHotKey.GetComponent<BlackHotKeyController>();
        blackHotKeyController.SetUpHotKey(chosenKey, collision.transform, this);

        //Targets.Add(collision.transform);
    }
    private void DestoryHotKey()
    {
        for(int i = 0; i < UsedHotKey.Count; i++) 
        {
            Destroy(UsedHotKey[i]);
            UsedHotKey[i] = null;
        }

    }
    public void ExitBlackHoleState()
    {
        canShrink = true;
        canPlayExitBlackHoleState = true;
        DestoryHotKey();
        PlayerManager.instance.player.MakeItTransparent(false);
    }

    public void AddTarget(Transform target)
    {
        Targets.Add(target);
    }
}
