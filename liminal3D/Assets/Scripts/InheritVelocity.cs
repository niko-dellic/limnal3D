using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InheritVelocity : MonoBehaviour
{

    //public GameObject Player;


    private void OnTriggerEnter(Collider other)
    {
        //if(other.gameObject == Player)
        other.transform.parent = transform;
        if(other.tag ==  "PLAYER_CLONE")
        {
            
        }
    }

    private void OnTriggerExit(Collider other)
    {

        //if(other.gameObject == Player)
        other.transform.parent = null;
        if(other.tag ==  "PLAYER_CLONE")
        {
            
        }   

    }

} 