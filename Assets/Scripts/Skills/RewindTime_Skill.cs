using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewindTime_Skill : Skill
{
    //TODO:类似艾克大招实现3秒的时间倒转
    [Header("时间裂隙")]
    public bool isRewindTimeUnlocked;
    [SerializeField] private UI_SkillTreeSlot unlockButtonOnRewindTime;
    private struct PlayerInfo
    {
        public Vector3 postionInfo;
        public int HpInfo;

        public PlayerInfo(Vector3 _postionInfo, int _hpInfo)
        {
            postionInfo = _postionInfo;
            HpInfo = _hpInfo;
        }
    }
    [SerializeField] private GameObject preRewindTimePrefab;/*锁阳入腹*/
    [SerializeField] private GameObject rewindTimeExplodePrefab;/*出来时候的爆炸*/

    [SerializeField] private int explodeDamage;

    private RewindTimeExplodeController myRTEController;
    private PreRewindTimeController myPRTController;
    private PlayerInfo infoToRewind;
    private Queue<PlayerInfo> info_3S_Ago_Queue;/*用来记录三秒内每一次的坐标信息*/

    protected override void Start()
    {   
        unlockButtonOnRewindTime.GetComponent<Button>().onClick.AddListener(UnlockRewindTime);
        base.Start();
        info_3S_Ago_Queue = new Queue<PlayerInfo>();

        InvokeRepeating("RecordInfoPerSecond",0, .2f);
    }
    protected override void Update()
    {
        base.Update();

    }
    public override void UseSkill()
    {
        base.UseSkill();
        UIController.instance.GetUI_InGame().SetUpImageForRewindTime();

        infoToRewind = info_3S_Ago_Queue.Dequeue();
        PreRewindDisappear();

        Invoke("RewindTimeExplode", 1);
    }

    private void RewindTimeExplode()
    {
        //TODO:将位置传送同时产生爆炸的实体
        LetInfoRewind();
        GameObject newExplode = Instantiate(rewindTimeExplodePrefab, player.transform.position, Quaternion.identity);
        myRTEController = newExplode.GetComponentInChildren<RewindTimeExplodeController>();
        myRTEController.SetUpRTEController(explodeDamage);
    }

    private void LetInfoRewind()
    {
        player.transform.position = infoToRewind.postionInfo;
        if (player.stats.currentHealth < infoToRewind.HpInfo)
        {
            player.stats.currentHealth= infoToRewind.HpInfo;
        }
    }

    public void RecordInfoPerSecond()
    {   
        //TODO:记录信息
        Vector3 temp_Postion =player.transform.position;
        int temp_Hp = player.stats.currentHealth;
        PlayerInfo temp_Info = new PlayerInfo(temp_Postion, temp_Hp);
        info_3S_Ago_Queue.Enqueue(temp_Info);

        if (info_3S_Ago_Queue.Count > 10)
        {
            PlayerInfo a = info_3S_Ago_Queue.Dequeue();
        }
    }
    public void PreRewindDisappear()
    {   
        //TODO:人物隐身缩入黑洞,同时时停
        PlayerManager.instance.player.MakeItTransparent(true);
        GameObject newDisappear = Instantiate(preRewindTimePrefab, player.transform.position, Quaternion.identity);
        FreezeController.instance.FreezeAll();
    }

    public override bool TryToUseSkill()
    {
        //TODO:黑洞和未解锁无法使用
        if (player.stateMachine.currentState == player.blackHoleState||isRewindTimeUnlocked==false)
        {
            return false;
        }
        return base.TryToUseSkill();
    }
    private void UnlockRewindTime()
    {
        if (unlockButtonOnRewindTime.isUnlocked)
        {
            isRewindTimeUnlocked = true;
        }
    }

    protected override void LoadSkillCheckUnlock()
    {
        UnlockRewindTime();
    }
}
