using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false; //Only applies to editor, remove before publish if needed
    }
}
