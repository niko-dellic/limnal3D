using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ToggleComputerVision : MonoBehaviour
{

    [SerializeField] public bool inZone = false;
    [SerializeField] public bool activateZone = false;

    [SerializeField] public GameObject machineCamera;
    [SerializeField] public GameObject playerCamera;
    [SerializeField] public Camera viewCam;
    [SerializeField] public GameObject pixelizedCamera;

    [Header ("Camera Projections - Ortho")]
    [SerializeField] public float minOrtho = 1f;
    [SerializeField] public float maxOrtho = 25f;
    [SerializeField] public float orthoSensitivity = 10f;
    [SerializeField] public float defaultOrthoSize = 5;

    private float orthoSize;

    void computerZone()
    {

        if (Input.GetKeyDown(KeyCode.Q) && inZone == true)
        {
            activateZone = !activateZone;
        }  

        if (inZone == true && activateZone == true)
        {
            machineCamera.SetActive(true);
            playerCamera.SetActive(false);
            pixelizedCamera.SetActive(false);
            

            if (Input.GetAxis("Mouse ScrollWheel") > 0f ) // forward 
            { 
                orthoSize = viewCam.orthographicSize;
                orthoSize += Input.GetAxis("Mouse ScrollWheel") * orthoSensitivity;
                orthoSize = Mathf.Clamp(orthoSize, minOrtho, maxOrtho);
                viewCam.orthographicSize = orthoSize;
            } 

            else if (Input.GetAxis("Mouse ScrollWheel") < 0f ) // backwards 
            { 
                orthoSize = viewCam.orthographicSize;
                orthoSize += Input.GetAxis("Mouse ScrollWheel") * orthoSensitivity;
                orthoSize = Mathf.Clamp(orthoSize, minOrtho, maxOrtho);
                viewCam.orthographicSize = orthoSize;
            }

        }

        if (inZone == true && activateZone == false)
        {
            machineCamera.SetActive(false);
            playerCamera.SetActive(true);
            pixelizedCamera.SetActive(true);
        }


    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PLAYER_CLONE")
        {
            inZone = true;
        }    
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "PLAYER_CLONE")
        {
            inZone = false;
            activateZone = false;
            machineCamera.SetActive(false);
            playerCamera.SetActive(true);
            pixelizedCamera.SetActive(true);
        }    
    }

    void Update()
    {
    computerZone();
    }
}
