using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xyzRotate : MonoBehaviour
{

    // public List<GameObject> axis = new List<GameObject>();
    // public GameObject gumballCam;

    // Update is called once per frame
    void Update()
    {

        if(Camera.main != null)
        {
            transform.rotation = Camera.main.transform.rotation;
        }
        // for (int i = 0; i < axis.Count; i++)
        // {
        //     axis[i].transform.LookAt(gumballCam.transform);
        // }
        
    }
}
