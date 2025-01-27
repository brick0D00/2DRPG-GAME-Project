using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private GameObject continueGameButton;
    [SerializeField] private UI_FadeScreen fadeScreen;

    private float delayDuration = 1.5f;
    private void Start()
    {   

        //没有可以继续的游戏
        if (SaveManager.instance.HaveSaveDData() == false)
        {
            continueGameButton.SetActive(false);
        }
    }
    public void ContinueGame()
    {
        StartCoroutine(LoadScenceWithFadeEffect(delayDuration));
    }
    public void NewGame()
    {   //TODO:开始新游戏

        //删除原有存档
        SaveManager.instance.DeleteSavedData();
        StartCoroutine(LoadScenceWithFadeEffect(delayDuration));
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        // 在Unity编辑器中，停止播放模式
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    IEnumerator LoadScenceWithFadeEffect(float _delayDuration)
    {   
        //TODO:加载之前会先变黑，后加载场景
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(_delayDuration);

        SceneManager.LoadScene(sceneName);
    }
}
