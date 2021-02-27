using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableCollider : MonoBehaviour
{

    public GameObject gameCollidertoDisable;


   
    
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PLAYER_CLONE" || other.tag == "HOST")
        {
            gameCollidertoDisable.GetComponent<MeshCollider>().enabled = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "PLAYER_CLONE" || other.tag == "HOST")
        {
            gameCollidertoDisable.GetComponent<MeshCollider>().enabled = true;
        }
    }


}
