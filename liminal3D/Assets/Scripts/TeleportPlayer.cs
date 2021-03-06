using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{

    public GameObject localReference;

    private Transform teleportTarget; //variable for the teleportation position

    public GameObject teleportObject;

    public bool inheritRotation = true;

    private void Start()
    {
        teleportTarget = teleportObject.transform;
        teleportTarget.GetComponent<MeshRenderer>().enabled = false;    
    }


    
    void OnTriggerEnter(Collider other)
    {


        if (other.tag == "PLAYER_CLONE" || other.tag == "HOST")
        {

            if (localReference != null)
            {
                localReference.GetComponent<MeshRenderer>().enabled = false;
                Vector3 displacement = other.transform.position - localReference.transform.position;
                other.transform.position = teleportTarget.transform.position + displacement;
            }

            else
            {
                other.transform.position = teleportTarget.transform.position;
            }
            

            if (inheritRotation)
            {
                other.transform.rotation = teleportTarget.transform.rotation;  
            }



        }
    }

    private void Update() {

        if (!teleportObject.gameObject.activeSelf)
        {
            Debug.Log("not active");
        } 

    }

}
