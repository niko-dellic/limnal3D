using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraClip : MonoBehaviour
{
    public float clipEndDistance;

    private float defaultClipDistance;

    private Camera playerCamera;

    // private bool clipZone;


    void OnTriggerEnter()
    {
        // clipZone = true;
        Camera.main.GetComponent<Camera>().farClipPlane = clipEndDistance;
    }

    void OnTriggerExit()
    {
        Camera.main.GetComponent<Camera>().farClipPlane = defaultClipDistance;
    }

    void Awake()
    {
        defaultClipDistance = Camera.main.GetComponent<Camera>().farClipPlane;

   
    }

    // void Update()
    // {
        
    // }
}
