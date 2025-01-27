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


    [Header("��������ֵ")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchDir;
    [SerializeField] private float swordGravity;
    [SerializeField] private int swordDamage;
    [SerializeField] private SwordType swordType;
    [SerializeField] private Transform forSword;
    private Vector2 aimDirection;

    [Header("������")]
    [SerializeField] private int amountOfBounce;
    [SerializeField] private float bounceGravity;
    [SerializeField] private int bouceDamage;

    [Header("���̽�")]
    [SerializeField] private float pierceGravity;
    [SerializeField] private int amountOfPierce;
    [SerializeField] private int pierceDamage;

    [Header("���⽣")]
    [SerializeField] private float maxTravelDistance;
    [SerializeField] private float spinDuration;
    [SerializeField] private float spinGravity;
    [SerializeField] private float hitCoolDown;
    [SerializeField] private int spinDamage;


    [Header("Ԥ���")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float distanceBetweenDots;
    [SerializeField] private GameObject dotprefab;
    [SerializeField] private Transform dotsParent;
    private GameObject[] dots;

    [Header("����Ч��")]
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
        if (Input.GetKeyUp(KeyCode.Mouse1))//�ɿ���ʱ����
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
        //TODO:���⿪����
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
        //TODO:����һ���½�������Ԥ���壬ͨ��controller���ƽ���
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

        //������ǽ���ռλ
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
        //TODO:ͨ����Һ����λ�ü������׼����
        Vector2 playerPostion = player.transform.position;
        Vector2 mousePostion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePostion - playerPostion;


        return direction;
    }


    private void GenerateDots()
    {   
        //TODO:����Ԥ���ߣ����ǲ���ʾ
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i]=Instantiate(dotprefab,player.transform.position, Quaternion.identity,dotsParent);
            dots[i].SetActive(false);
        }
    }
    public void SetDosActive(bool _active)
    {   
        //TODO:����Ԥ�����Ƿ���ʾ
        for (int i = 0;i < numberOfDots;i++) 
        {
            dots[i].SetActive(_active);
        }
    }
    private Vector2 GetDotsPostion(float t)
    {   
        //TODO:������ʽ�����ÿһ�����λ��
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
