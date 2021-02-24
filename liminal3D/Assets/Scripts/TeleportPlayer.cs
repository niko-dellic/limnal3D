using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{


    private Transform teleportTarget; //variable for the teleportation position

    public GameObject teleportObject;

    private void Start()
    {
        teleportTarget = teleportObject.transform;
    }

    
    void OnTriggerEnter(Collider other)
    {

        other.transform.position = teleportTarget.transform.position;
        other.transform.rotation = teleportTarget.transform.rotation;   
        
    }

    private void Update() {

        if (!teleportObject.gameObject.activeSelf)
        {
            Debug.Log("not active");
        } 

    }

}
