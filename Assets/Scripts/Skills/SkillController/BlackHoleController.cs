using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleController : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    public float maxSize;/*�ڶ��ܳɳ������ߴ�*/
    public float growSpeed;
    public bool canGrow;
    public bool canShrink;
    public float shrinkSpeed;/*̮���ٶ�*/

    public bool canAttack;
    public bool canCreateHotKey=true;
    public int amountOfTargets=4;
    public float cloneAttackCoolDown=.3f;
    public float cloneTimer;

    public float blackHoleTimer;

    private List<Transform> Targets= new List<Transform>();/*������¼qte����Ĺ���Ŀ��*/
    private List<GameObject> UsedHotKey= new List<GameObject>();/*ɾ��hotkey*/

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
            blackHoleTimer = Mathf.Infinity;/*������������ظ�ִ��*/
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

        //��ɫ͸��
        PlayerManager.instance.player.MakeItTransparent(true);
    }

    private void CloneAttackLogic()
    {
        if (Targets.Count == 0) { return; }
        //TODO:������¡�幥��
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
                Invoke("ExitBlackHoleState", 0.5f);/*ȷ���ڹ�����ɺ����*/

            }
        }
    }

    private void GrowShrinkController()
    {
        if (canGrow && !canShrink)
        {
            //TODO:�úڶ�˿������
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
        //TODO:�������ڶ��ĵ���ʱͣ��ͬʱ����hotkey

        if(collision.GetComponent<Enermy>() != null)
        {

            collision.GetComponent<Enermy>().SetFreeze(true);
            CreateHotKey(collision);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {   
        //TODO:���ڶ���ʧ��ʱ��ⶳ
        if (collision.GetComponent<Enermy>() != null)
        {

            collision.GetComponent<Enermy>().SetFreeze(false);
        }
    }

    private void CreateHotKey(Collider2D collision)
    {   
        //TODO:�����ȼ�

        if(keyCodeList.Count <= 0 || canCreateHotKey==false) 
        {   //������Ա��
            return;    
        }

        //����һ��Ԥ����
        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        UsedHotKey.Add(newHotKey);
        //�����ȡһ��KeyCode���Ƴ�
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
