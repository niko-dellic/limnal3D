using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class returnMainMenu : MonoBehaviour
{

    // public GameObject multiplayerMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameObject.activeSelf)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
                Time.timeScale = 1f;
            }

        }
    }


}
