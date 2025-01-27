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

        //û�п��Լ�������Ϸ
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
    {   //TODO:��ʼ����Ϸ

        //ɾ��ԭ�д浵
        SaveManager.instance.DeleteSavedData();
        StartCoroutine(LoadScenceWithFadeEffect(delayDuration));
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        // ��Unity�༭���У�ֹͣ����ģʽ
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    IEnumerator LoadScenceWithFadeEffect(float _delayDuration)
    {   
        //TODO:����֮ǰ���ȱ�ڣ�����س���
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(_delayDuration);

        SceneManager.LoadScene(sceneName);
    }
}
