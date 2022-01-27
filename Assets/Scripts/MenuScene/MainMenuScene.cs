using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MainMenuScene : MonoBehaviour
{
    public Canvas mainCanvas;
    public GameObject mainCamera;
    public float cameraRotationSpeed;
    public Vector2 angles; //angles.x is min, angles.y is max

    private bool isMenu = true;

    void Start()
    {
        if (mainCanvas == null)
        {
            Debug.Log("Missing mainCanvas object");
        }
    }

    void Update()
    {
        if ((!isMenu && this.mainCamera.transform.eulerAngles.y <= angles.y) || 
            (isMenu && this.mainCamera.transform.eulerAngles.y >= angles.x))
        {
            mainCamera.transform.Rotate(Vector3.up * this.cameraRotationSpeed * (isMenu ? -1 : 1) * Time.deltaTime, Space.World);
        }
            
    }

    public void loadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void rotateCamera()
    {
        this.isMenu = !this.isMenu;
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
