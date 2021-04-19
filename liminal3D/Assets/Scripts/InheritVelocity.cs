using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class InheritVelocity : MonoBehaviour
{

    //public GameObject Player;


    private void OnTriggerEnter(Collider other)
    {
        //if(other.gameObject == Player)
        
        if(other.tag ==  "PLAYER_CLONE" || other.tag == "HOST")
        {
            other.transform.parent = transform;

            other.GetComponent<AudioSource>().mute = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        //if(other.gameObject == Player)
        
        if(other.tag ==  "PLAYER_CLONE" || other.tag == "HOST")
        {
            other.transform.parent = null;
            other.GetComponent<AudioSource>().mute = false;
        }   

    }

} 