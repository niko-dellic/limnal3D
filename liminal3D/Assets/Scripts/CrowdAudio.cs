using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdAudio : MonoBehaviour
{
    
    public GameObject audioSphere;
    public bool enableZone = true;
    public float heightOffset = 4f;
    private Transform defaultParent;

    public void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "PLAYER_CLONE" || other.tag == "HOST")
        {
            if (enableZone)
            {
                audioSphere.transform.parent = other.transform;
                audioSphere.transform.localPosition = new Vector3(0,heightOffset,0);

                audioSphere.SetActive(true);
            }

            if (!enableZone)
            {
                audioSphere.transform.parent = defaultParent;
   
            }
        }
    }
    
    public void OnTriggerExit(Collider other) 
    {
        if (other.tag == "PLAYER_CLONE" || other.tag == "HOST")
        {

            if (enableZone)
            {
                audioSphere.transform.parent = defaultParent;
                audioSphere.SetActive(false);
            }
            
            if (!enableZone)
            {
                audioSphere.transform.parent = other.transform;
                audioSphere.transform.localPosition = new Vector3(0,heightOffset,0);
            }
        }
    }


    void Start()
    {
        defaultParent = audioSphere.transform.parent;
        audioSphere.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
