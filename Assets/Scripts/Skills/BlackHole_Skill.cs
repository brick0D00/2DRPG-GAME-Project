using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackHole_Skill : Skill
{
    [Header("ºÚ¶´")]
    public bool isBlackHoleUnlocked;
    [SerializeField] private UI_SkillTreeSlot unlockButtonOnBlackHole;

    [SerializeField] private GameObject BlackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private int amountOfTargets;
    [SerializeField] private float blackHoleDuration;
    private BlackHoleController currentBlackhole;
    public override bool TryToUseSkill()
    {
        return base.TryToUseSkill();
    }

    public override bool IsSkillReady()
    {
        if (isBlackHoleUnlocked == false) { return false; }
        return base.IsSkillReady();
    }
    public override void UseSkill()
    {
        base.UseSkill();
        UIController.instance.GetUI_InGame().SetUpImageForBlackHole();
        GameObject newBlackHole=Instantiate(BlackHolePrefab,player.transform.position,Quaternion.identity);

        currentBlackhole = newBlackHole.GetComponent<BlackHoleController>();
        currentBlackhole.SetUpBlackHole(false,true, false, maxSize, amountOfTargets,blackHoleDuration);
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {   
        unlockButtonOnBlackHole.GetComponent<Button>().onClick.AddListener(UnlockBlackHole);
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
    public bool isBlackholeFinished()
    {
        if (currentBlackhole == null)
        {
            return false;
        }
        if (currentBlackhole.canPlayExitBlackHoleState==true)
        {
            currentBlackhole = null;
            return true;
        }
        return false;
    }
    public void UnlockBlackHole()
    {
        if (unlockButtonOnBlackHole.isUnlocked)
        {
            isBlackHoleUnlocked = true;
        }
    }

    protected override void LoadSkillCheckUnlock()
    {
        UnlockBlackHole();
    }
}
