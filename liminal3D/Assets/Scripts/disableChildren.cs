using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableChildren : MonoBehaviour
{
    public bool activeZone = true;
    void OnTriggerEnter(Collider other)
    {
        activeZone = true;    
    }

    void OnTriggerExit(Collider other)
    {
        activeZone = false;      
    }

    void Start()
    {
          
    }


    // Update is called once per frame
    void Update()
    {

        if (!activeZone)
        {
            // this.transform.parent.gameObject.SetActive(false);
            foreach (Transform child in transform)
            // child.gameObject.GetComponent<MeshRenderer>().enabled = false;
            child.gameObject.SetActive(false);
        }

        if (activeZone)
        {
            foreach (Transform child in transform)
            // child.gameObject.GetComponent<MeshRenderer>().enabled = true;
            child.gameObject.SetActive(true);
            //SetActive(true);
        }
        
    }
}
