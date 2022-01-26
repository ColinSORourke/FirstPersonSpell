using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MainMenuScene : MonoBehaviour
{
    public Canvas mainCanvas;

    void Start()
    {
        if (mainCanvas == null)
        {
            Debug.Log("Missing mainCanvas object");
        }
    }

    void Update()
    {
        
    }

    public void loadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void quitGame()
    {
        #if UNITY_EDITOR
                EditorApplication.ExitPlaymode();
        #else
                Application.Quit();
        #endif
        
    }
}
