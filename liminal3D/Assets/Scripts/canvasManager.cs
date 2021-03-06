using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvasManager : MonoBehaviour
{

    public static canvasManager instance;
    public GameObject reticle;

    public GameObject menu1;
    public GameObject menu2;
    public GameObject menu3;

    


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
        
        // HideUI();

    }

 void Update()
 {
   
        if (!menu1.activeSelf && !menu2.activeSelf && !menu3.activeSelf)
        {

            if(Input.GetKey(KeyCode.Mouse0))
            {
                reticle.SetActive(true);
            }
            else
            {
                reticle.SetActive(false);
            }
        }
 }


}
