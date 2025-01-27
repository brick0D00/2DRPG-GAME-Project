using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class UIController : MonoBehaviour,ISaveManager
{
    public static UIController instance;

    [Header("死亡动画")]
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject youDiedText;
    [SerializeField] private GameObject restartGameButton;

    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;

    public UI_ItemTips itemTips;
    public UI_StatTips statTips;
    public UI_SkillTips skillTips;
    public UI_CraftWindow craftWindow;

    public UI_VolumeSlider[] volumeSettings;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
        //TODO:让skillTreeUI Enable，不然无法分配监听事件
        SwitchTo(skillTreeUI);
    }
    void Start()
    {
        //开始的时候不显示任何UI
        SwitchTo(null);
        SwitchTo(inGameUI);
        itemTips.gameObject.SetActive(false);
        statTips.gameObject.SetActive(false);
    }


    void Update()
    {
        SwitchUIController();
    }
    #region SwitchUI
    private void SwitchUIController()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {
            SwitchWithKeyTo(characterUI);
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            SwitchWithKeyTo(craftUI);
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            SwitchWithKeyTo(skillTreeUI);
        }
        if (Input.GetKeyUp(KeyCode.O)||Input.GetKeyUp(KeyCode.Escape))
        {
            SwitchWithKeyTo(optionsUI);
        }
    }

    public void SwitchTo(GameObject _menu)
    {

        //将所有子菜单虚化
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<UI_FadeScreen>() != null)
            {
                continue;
            }
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
        {
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlaySFX(11, null);
            }
            _menu.SetActive(true);
        }


        if (_menu == null) { return; }
        if(GameManager.instance != null)
        {   
            //TODO:切换到UI界面的时候暂停游戏
            if (_menu != inGameUI)
            {
                GameManager.instance.PauseGame(true);
            }
            else
            {
                GameManager.instance.PauseGame(false);
            }

        }
    }

    public void SwitchWithKeyTo(GameObject _menu)
    {
        //TODO:通过按键转移，但当前界面在打开时再输入会关闭
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            SwitchTo(inGameUI);
            return;
        }
        SwitchTo(_menu);
    }
    #endregion
    public UI_InGame GetUI_InGame()
    {
        return inGameUI.GetComponent<UI_InGame>();
    }
    #region EndScreen
    public void SwitchToEndScreen()
    {   //TODO:切换到死亡界面
        SwitchTo(null);
        fadeScreen.gameObject.SetActive(true);
        fadeScreen.FadeOut();
        StartCoroutine(ShowEndScreen_Coroutine());
    }
    IEnumerator ShowEndScreen_Coroutine()
    {
        
        yield return new WaitForSeconds(2f);

        youDiedText.SetActive(true);

        yield return new WaitForSeconds(1f);
        Debug.Log(1145);
        restartGameButton.SetActive(true);

    }

    public void RestartButton()
    {
        //TODO:重新开始游戏
        GameManager.instance.RestartScene();
    }

    public void LoadData(GameData _data)
    {
        StartCoroutine(DelayLoadData(_data));
    }

    IEnumerator DelayLoadData(GameData _data)
    {
        yield return new WaitForSeconds(0.001f);

        foreach (var pair in _data.volumeSettings)
        {
            foreach (UI_VolumeSlider volumeSetting in volumeSettings)
            {
                if (volumeSetting.parameter == pair.Key)
                {
                    volumeSetting.LoadSlider(pair.Value);
                }
            }
        }
    }


    public void SaveData(ref GameData _data)
    {
        _data.volumeSettings.Clear();
        foreach(var voluemSetting in this.volumeSettings)
        {
            _data.volumeSettings.Add(voluemSetting.parameter,voluemSetting.slider.value);
        }
    }

    #endregion
}
