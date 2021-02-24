using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class muteSteps : MonoBehaviour
{

    public bool inWater;


    private void OnTriggerEnter(Collider other)
    {
        
        if(other.tag ==  "PLAYER_CLONE")
        {
            other.GetComponent<AudioSource>().Stop();
            inWater = true;
            //Debug.Log(inWater + "origin");
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if(other.tag ==  "PLAYER_CLONE")
        {
            other.GetComponent<AudioSource>().Play();
            inWater = false;
        }   

    }


}
