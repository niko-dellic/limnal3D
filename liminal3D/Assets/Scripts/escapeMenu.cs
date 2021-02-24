using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class escapeMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
            Debug.Log("Quit!");
        }
    }
}
