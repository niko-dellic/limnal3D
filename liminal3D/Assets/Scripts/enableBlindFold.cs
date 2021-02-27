using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enableBlindFold : MonoBehaviour
{

    public GameObject blindFold;

    void Start()
    {
     blindFold.SetActive(false);   
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.tag ==  "PLAYER_CLONE" || other.tag == "HOST")
        {
        blindFold.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag ==  "PLAYER_CLONE" || other.tag == "HOST")
        {
        blindFold.SetActive(false);
        }
    }

}
