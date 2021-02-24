using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{

    [Header("Panels")]
    [SerializeField] GameObject UI_Alive;
    
    public static bool GameisPaused = false;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameisPaused)
            {
                Resume();
            } 
            else
            {
                Pause();
            }
        }
    }

    public void Resume ()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameisPaused = false;
        Cursor.lockState = CursorLockMode.Locked;

        //ShowUI(); // only enable when troubleshooting mobile
        
        // if (Application.isMobilePlatform || Application.isConsolePlatform)
        // {
        //     ShowUI();
        // }

    }

    void Pause ()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameisPaused = true;
        Cursor.lockState = CursorLockMode.None;
        // HideUI();
    }

    public void loadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MENU");
    }


    public void quitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }


}
