using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pauseMaster : MonoBehaviour
{
 
    public GameObject multiplayerMenu;

    // Update is called once per frame
    
    // void Start()
    // {
    //   transform.GetChild(0).gameObject.SetActive(true);   
    // }
    
    
    void Start()
    {
        
    }
    
    void Update()
    {
        if(multiplayerMenu.activeInHierarchy == true)
        {
            gameObject.SetActive(false);
        }
        else
        {
             gameObject.SetActive(true);
        }
        
    }
}
