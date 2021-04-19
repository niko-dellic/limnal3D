using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testMeshCollider : MonoBehaviour
{

    private void OnTriggerEnter(Collider other) {
        
        Debug.Log("IN");
    }


    private void OnTriggerExit(Collider other) {
        
        Debug.Log("OUT");
    }

}
