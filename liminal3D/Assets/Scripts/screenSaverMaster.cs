using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screenSaverMaster : MonoBehaviour
{
    [Header("AFK TIMER")]
    public int AFKTimeout = 3;

    [Header("AFK Cameras")]
    public List<Camera> AFKCameras;
    private float cameraSwitch;
    private Camera mainCam;
    private float nextActionTime = 0f;
    public float AFKCameraSwapTimer = 3f;
    private int cameraIndex = 0;


    [Header("Floating Props")]
    public List<GameObject> screenSaverProps;

    [Header("ScreenBounds")]
    public List<GameObject> screenSaverBounds;
    float lastIdleTime;
    private bool AFK = false;

    void Awake()
    {
        lastIdleTime = Time.time;    

        mainCam = Camera.main;

        // Cameras Start
        foreach (Camera i in AFKCameras)
        {
            i.enabled = false;
        }

         //Props Start
        foreach (GameObject i in screenSaverProps)
        {
            i.SetActive(false);
        }

         //Bounds Start
        foreach (GameObject i in screenSaverBounds)
        {
            i.SetActive(false);
        }

    }

    public void playerGone()
    {
        //Display Props
        if (Time.time - lastIdleTime > AFKTimeout)
        {
            AFK = true;
        }
        else
        {
            AFK = false;
        }
    }

   
    void playerIdle()
    {
        if (AFK)
        {
            //Disable Main Cameras
            mainCam.enabled = false;
            cameraSwitch = Time.time;

            //SWITCH TO AFK CAMERAS
            if (Time.time > nextActionTime )
            {

                nextActionTime = Time.time + AFKCameraSwapTimer;
                cameraIndex++;

                if (cameraIndex == AFKCameras.Count)
                {
                    cameraIndex = 1;
                }

            }

            Camera displayAFKCam = AFKCameras[cameraIndex];
            Camera otherAFKCam = AFKCameras[cameraIndex-1];

            displayAFKCam.enabled = true;
            otherAFKCam.enabled = false;

            //ENABLE PROPS
            foreach (GameObject i in screenSaverProps)
            {
                i.SetActive(true);
            }

            //ENABLE PROPS BOUNDS
            foreach (GameObject i in screenSaverBounds)
            {
                i.SetActive(true);
            }
        }

        else
        {
            foreach (GameObject i in screenSaverProps)
            {
                i.SetActive(false);
            }
        }
       
    }

    void Update()
    {
        if(Input.anyKey)
        {
            lastIdleTime = Time.time;
            mainCam.enabled = true;

            foreach (Camera i in AFKCameras)
            {
                i.enabled = false;
            }

        }


        playerIdle();
        playerGone();
    }


}
