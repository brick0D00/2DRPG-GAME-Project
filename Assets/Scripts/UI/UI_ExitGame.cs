using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ExitGame : MonoBehaviour
{
    public void SaveAndExitGame()
    {
    #if UNITY_EDITOR
        // ��Unity�༭���У�ֹͣ����ģʽ
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
