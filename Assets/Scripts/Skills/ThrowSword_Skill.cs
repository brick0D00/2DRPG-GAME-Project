using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public enum SwordType
{
    Regular,
    Bouce,
    Pierce,
    Spin
}

public class ThrowSword_Skill : Skill
{


    [Header("剑的属性值")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchDir;
    [SerializeField] private float swordGravity;
    [SerializeField] private int swordDamage;
    [SerializeField] private SwordType swordType;
    [SerializeField] private Transform forSword;
    private Vector2 aimDirection;

    [Header("弹跳剑")]
    [SerializeField] private int amountOfBounce;
    [SerializeField] private float bounceGravity;
    [SerializeField] private int bouceDamage;

    [Header("穿刺剑")]
    [SerializeField] private float pierceGravity;
    [SerializeField] private int amountOfPierce;
    [SerializeField] private int pierceDamage;

    [Header("链锯剑")]
    [SerializeField] private float maxTravelDistance;
    [SerializeField] private float spinDuration;
    [SerializeField] private float spinGravity;
    [SerializeField] private float hitCoolDown;
    [SerializeField] private int spinDamage;


    [Header("预瞄点")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float distanceBetweenDots;
    [SerializeField] private GameObject dotprefab;
    [SerializeField] private Transform dotsParent;
    private GameObject[] dots;

    [Header("技能效果")]
    public bool isSwordUnlocked;
    [SerializeField] private UI_SkillTreeSlot unlockButtonOnSword;
    [SerializeField] private UI_SkillTreeSlot unlockButtonOnBouce;
    [SerializeField] private UI_SkillTreeSlot unlockButtonOnPierce;
    [SerializeField] private UI_SkillTreeSlot unlockButtonOnSpin;

    public bool isSlowDownUnlocked;
    [SerializeField] private UI_SkillTreeSlot unlockButtonOnSlowDown;
    [Range(0f, 1f)]
    [SerializeField] private float normalSlow;

    public bool isSuperSlowDownUnlocked;
    [SerializeField] private UI_SkillTreeSlot unlockButtonOnSuperSlow;
    [Range(0f, 1f)]
    [SerializeField] private float superSlow;

    public float slowPercent = 0f;
    public float slowDuration = 3f;

    protected override void Start()
    {
        base.Start();

        GenerateDots();
        unlockButtonOnSword.GetComponent<Button>().onClick.AddListener(UnlockSowrd);
        unlockButtonOnBouce.GetComponent<Button>().onClick.AddListener(UnlockBouceSword);
        unlockButtonOnPierce.GetComponent<Button>().onClick.AddListener(UnlockPierceSword);
        unlockButtonOnSpin.GetComponent<Button>().onClick.AddListener(UnlockSpinSword);
        unlockButtonOnSlowDown.GetComponent<Button>().onClick.AddListener(UnlockSlowDown);
        unlockButtonOnSuperSlow.GetComponent<Button>().onClick.AddListener(UnlockSuperSlow);

    }
    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyUp(KeyCode.Mouse1))//松开的时候检测
        {
            aimDirection = new Vector2(GetAimDir().normalized.x * launchDir.x, GetAimDir().normalized.y * launchDir.y);
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < numberOfDots; i++)
            {
                dots[i].transform.position = GetDotsPostion(i * distanceBetweenDots);
            }
        }
    }
    #region Use Skill
    public override void UseSkill()
    {
        base.UseSkill();
        CreateSword();
    }

    public override bool TryToUseSkill()
    {
        return base.TryToUseSkill();
    }
    #endregion
    #region Unlock Skill

    private void UnlockSowrd()
    {
        if (unlockButtonOnSword.isUnlocked)
        {
            swordType = SwordType.Regular;
            isSwordUnlocked = true;
        }
    }
    private void UnlockSlowDown()
    {
        //TODO:避免开倒车
        if (isSuperSlowDownUnlocked) { return; }
        if (unlockButtonOnSlowDown.isUnlocked)
        {
            isSlowDownUnlocked = true;
            slowPercent = normalSlow;
        }
    }

    private void UnlockSuperSlow()
    {
        if (unlockButtonOnSuperSlow.isUnlocked)
        {
            isSuperSlowDownUnlocked = true;
            slowPercent = superSlow;
        }
    }
    private void UnlockBouceSword()
    {
        if (unlockButtonOnBouce.isUnlocked)
        {
            swordType = SwordType.Bouce;
        }
    }
    private void UnlockPierceSword()
    {
        if (unlockButtonOnPierce.isUnlocked)
        {
            swordType = SwordType.Pierce;
        }
    }
    private void UnlockSpinSword()
    {
        if (unlockButtonOnSpin.isUnlocked)
        {
            swordType = SwordType.Spin;
        }
    }

    #endregion

    public void CreateSword()
    {   
        //TODO:创建一把新剑，调用预制体，通过controller控制剑体
        GameObject newSword = Instantiate(swordPrefab, forSword.position, transform.rotation);//player.transform.position,transform.rotation
        SwordController swordController = newSword.GetComponent<SwordController>();
        SetUpGravity();
        if (swordType == SwordType.Bouce)
        {
            swordController.SetBouceSword(true, amountOfBounce);
            swordDamage = bouceDamage;
        }else if(swordType==SwordType.Pierce) 
        {
            swordController.SetPierceSowrd(amountOfPierce);
            swordDamage = pierceDamage;
        }else if (swordType == SwordType.Spin)
        {
            swordController.SetSpinSword(true, maxTravelDistance, spinDuration,hitCoolDown);
            swordDamage = spinDamage;
        }

        swordController.SetUpSword(aimDirection, swordGravity,swordDamage);

        //在玩家那将剑占位
        player.AssignNewSword(newSword);  

        SetDosActive(false);
    }
    public void SetUpGravity()
    {
        switch (swordType)
        {
            case SwordType.Bouce:
                swordGravity = bounceGravity; break;
            case SwordType.Pierce:
                swordGravity=pierceGravity; break;
            case SwordType.Spin:
                swordGravity = spinGravity;break;
            default
                : break;
        }
    }
 #region Aiming region

    private Vector2 GetAimDir()
    {   
        //TODO:通过玩家和鼠标位置计算出瞄准方向
        Vector2 playerPostion = player.transform.position;
        Vector2 mousePostion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePostion - playerPostion;


        return direction;
    }


    private void GenerateDots()
    {   
        //TODO:生成预瞄线，但是不显示
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i]=Instantiate(dotprefab,player.transform.position, Quaternion.identity,dotsParent);
            dots[i].SetActive(false);
        }
    }
    public void SetDosActive(bool _active)
    {   
        //TODO:设置预瞄线是否显示
        for (int i = 0;i < numberOfDots;i++) 
        {
            dots[i].SetActive(_active);
        }
    }
    private Vector2 GetDotsPostion(float t)
    {   
        //TODO:重力公式，算出每一个点的位置
        Vector2 postion = (Vector2)player.transform.position+new Vector2(
            GetAimDir().normalized.x*launchDir.x,
            GetAimDir().normalized.y*launchDir.y)*t+0.5f*(Physics2D.gravity*swordGravity)*(t*t);

        return postion;
    }

    #endregion
    protected override void LoadSkillCheckUnlock()
    {
        UnlockSowrd();
        UnlockBouceSword();
        UnlockPierceSword();
        UnlockSpinSword();
        UnlockSlowDown();
        UnlockSuperSlow();
    }
}
