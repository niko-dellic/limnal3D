using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InheritVelocity : MonoBehaviour
{

    //public GameObject Player;


    private void OnTriggerEnter(Collider other)
    {
        //if(other.gameObject == Player)
        
        if(other.tag ==  "PLAYER_CLONE" || other.tag == "HOST")
        {
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        //if(other.gameObject == Player)
        
        if(other.tag ==  "PLAYER_CLONE" || other.tag == "HOST")
        {
            other.transform.parent = null;
        }   

    }

} 