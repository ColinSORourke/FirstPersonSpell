using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MainMenuScene : MonoBehaviour
{
    public Canvas mainCanvas;
    public Transform mainCamera;
    public float cameraRotationSpeed;

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

    public void rotateCamera(float degree, bool dir)
    {
        mainCamera.Rotate(Vector3.forward * this.cameraRotationSpeed * (dir ? 1 : -1) * Time.deltaTime);
    }

    public void quitGame()
    {
        Application.Quit();
        /*
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
                Application.Quit();
        #endif
        */
    }
}
