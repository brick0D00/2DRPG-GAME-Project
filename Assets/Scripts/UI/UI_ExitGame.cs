using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ExitGame : MonoBehaviour
{
    public void SaveAndExitGame()
    {
    #if UNITY_EDITOR
        // 在Unity编辑器中，停止播放模式
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
