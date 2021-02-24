using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvasManager : MonoBehaviour
{

    public static canvasManager instance;

    [Header("Panels")]
    [SerializeField] GameObject UI_Alive;
    // [SerializeField] GameObject UI_Death;

    [Header("Joysticks")]
    public Joystick leftJoystick;
    public Joystick rightJoystick;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            //There's already existing canvas manager. 
            Destroy(gameObject);
            return;
            
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        
        HideUI();

    }

    public void ChangePlayerState(bool isAlive)
    {
        UI_Alive.SetActive(isAlive);
        // UI_Death.SetActive(!isAlive);
    }

    public void HideUI()
    {
        UI_Alive.SetActive(false);
        // UI_Death.SetActive(false);
        

    }


}
