using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Mirror;

public class videoTrigger : NetworkBehaviour
{

    [HideInInspector]
    public GameObject[] ditherArray;
    private bool videoMode = false;
    public GameObject interactionTrigger;
    public GameObject[] muteOtherAudioSources;

    [Header("3DVideo")]
    private bool resumeAR;
    public GameObject postProcessingEmpty;
    public GameObject VRSphere;

    [Header("2DVideo")]
    public GameObject videoUI;
    public GameObject blackDrop;
    public GameObject videoPlane;
    public Material videoMaterial;

    private double videoLength;

    private double timer;

    private bool inZone = false;

    private int toggle = 0;

    float ArUISpeedxRef;


    private void ToggleDither(bool ditherSwitch)
    {
        for (int i = 0; i < ditherArray.Length; i++)
        {
            ditherArray[i].SetActive(ditherSwitch);    
        }

        if (ditherSwitch == false)
        {
            videoMode = true;
        }

        if (ditherSwitch == true)
        {
            videoMode = false;
        }
    }

    private void muteExtraAudio(bool enableDisable)
    {
        for (int i = 0; i < muteOtherAudioSources.Length; i++)
        {
            if (muteOtherAudioSources[i].GetComponent<AudioSource>() != null)
            {
                muteOtherAudioSources[i].GetComponent<AudioSource>().mute = enableDisable;
            }
        }
    }

    private void Start() {

        ditherArray = GameObject.FindGameObjectsWithTag("DITHERZONE");

        
        if (videoUI != null)
        {
            videoLength = videoUI.GetComponent<VideoPlayer>().length;
        }

        if (VRSphere != null)
        {
            videoLength = VRSphere.GetComponent<VideoPlayer>().length;
            ArUISpeedxRef = postProcessingEmpty.GetComponent<enableAR>().ArUISpeed;
        }

        
    }
 
    private void OnTriggerEnter(Collider other) {
    
        if (other.tag == "PLAYER_CLONE" || other.tag == "HOST")
        {
            inZone = true;
            interactionTrigger.SetActive(true);
           
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "PLAYER_CLONE" || other.tag == "HOST")
        {
            inZone = false;
            interactionTrigger.SetActive(false);
        }
    }

    void LateUpdate() {

        if (inZone)
        {
            if (VRSphere != null && VRSphere.activeSelf == true)
            {
                VRSphere.transform.position = Camera.main.transform.position;
            }

            if(Input.GetKeyDown(KeyCode.Q))
            {
                toggle++;        
            }

            if (Input.GetKeyDown(KeyCode.Q)  && toggle%2==0) //&& videoUI.activeSelf == true
            {
                ToggleDither(true); //toggle dither On
                

                muteExtraAudio(false);

                // if (muteOtherAudio != null)
                // {
                //     muteOtherAudio.GetComponent<AudioSource>().mute = false;
                // }
                
                
                if (videoUI != null)
                {
                    videoUI.SetActive(false);
                    if (blackDrop != null)
                    {
                        blackDrop.SetActive(false);
                    }
                }


                if (VRSphere != null)
                {
                    VRSphere.SetActive(false);
                }

                interactionTrigger.SetActive(true);

                if (resumeAR){
                fixAR();
                }
            } 
 
            if (Input.GetKeyDown(KeyCode.Q) &&  toggle%2==1) //videoUI.activeSelf == false &&
            {

                //disable postPro Dither zones
                ToggleDither(false);

                muteExtraAudio(true);

                // if (muteOtherAudio != null)
                // {
                //     muteOtherAudio.GetComponent<AudioSource>().mute = true;
                // }

                
                if (videoUI != null)
                {
                    videoUI.SetActive(true);
                
                    if (blackDrop != null)
                    {
                        blackDrop.SetActive(true);
                    }
                }


                if (VRSphere != null)
                {   
                    if (postProcessingEmpty.GetComponent<enableAR>().ARToggle == false)
                    {
                        resumeAR = false;
                    }


                    if (postProcessingEmpty.GetComponent<enableAR>().ARToggle == true)
                    {
                        resumeAR = true;
                        postProcessingEmpty.GetComponent<enableAR>().ARToggle = false;
                        postProcessingEmpty.GetComponent<enableAR>().lerp = true;
                        postProcessingEmpty.GetComponent<enableAR>().timeTrigger = Time.time + ArUISpeedxRef;
                    }
                    
                    VRSphere.SetActive(true);

                }

                timer = Time.time + videoLength;
                interactionTrigger.SetActive(false);

            } 



           if (Time.time > timer)
            {
                if (videoMode == true)
                {
                    ToggleDither(true); //toggle dither On

                    muteExtraAudio(false);

                    // if (muteOtherAudio != null)
                    // {
                    //     muteOtherAudio.GetComponent<AudioSource>().mute = false;
                    // }

                    if (videoUI != null)
                    {
                        videoUI.SetActive(false);   
                    }
                    
                    if (blackDrop != null)
                    {
                        blackDrop.SetActive(false);
                    }
                }

                toggle = 0;
                
                if (interactionTrigger.activeSelf == false)
                {
                    interactionTrigger.SetActive(true);
                }

                if (VRSphere != null && VRSphere.activeSelf == true)
                {
                    VRSphere.SetActive(false);
                }

                if (resumeAR){
                fixAR();
                }

            }
        }

        if (!inZone)
        {
            muteExtraAudio(false);
            // if (muteOtherAudio != null)
            // {
            //     muteOtherAudio.GetComponent<AudioSource>().mute = false;
            // }


            if (videoMode == true)
            {
                ToggleDither(true); //toggle dither On

                if (videoUI != null)
                {
                    
                    videoUI.SetActive(false);
                    
                    if (blackDrop != null)
                    {
                        blackDrop.SetActive(false);
                    }
                
                }
            }

            if (resumeAR){
            fixAR();
            }
            
            toggle = 0;
            
            if (VRSphere != null && VRSphere.activeSelf == true)
            {
                VRSphere.SetActive(false);
            }
        }
        
        void fixAR()
        {
                if (resumeAR)
                {
                    postProcessingEmpty.GetComponent<enableAR>().ARToggle = true;
                    postProcessingEmpty.GetComponent<enableAR>().lerp = true;
                    float ArUISpeedxRef = postProcessingEmpty.GetComponent<enableAR>().ArUISpeed;
                    postProcessingEmpty.GetComponent<enableAR>().timeTrigger = Time.time + ArUISpeedxRef;
                    resumeAR = false;
                }
        }
    }


    //THIS IS FOR PLAYING THE VIDEO FOR EVERYONE AS HOST:

    // private void OnTriggerEnter(Collider other) {
        
    //     if (other.tag == "HOST")
    //     {

    //     CmdPlayVideo();

    //     videoLength = videoUI.GetComponent<VideoPlayer>().length;
    //     timer = Time.time + videoLength;
    //     this.gameObject.GetComponent<BoxCollider>().enabled = false;
    //     }
        
    // }

    // private void Update() {
    //     if (videoPlane.activeSelf == true && Time.time > timer)
    //     {
    //         videoUI.SetActive(false);
    //         blackDrop.SetActive(false);
    //         //videoPlane.SetActive(false);
    //     }

    // }

    // [Command (ignoreAuthority = true)]
    // void CmdPlayVideo() {
    //     RpcPlayVideoServer();
    // }

    // [ClientRpc]
    // void RpcPlayVideoServer() {
    //     videoUI.SetActive(true);
    //     blackDrop.SetActive(true);
    //     videoPlane.SetActive(true);
    // }
}
