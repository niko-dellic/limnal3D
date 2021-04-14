using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Mirror;

public class videoTrigger : NetworkBehaviour
{
    public GameObject interactionTrigger;
    public GameObject videoUI;
    public GameObject blackDrop;

    
    private double videoLength;

    //public GameObject videoPlane;

    //public Material videoMaterial;

    private double timer;

    private bool inZone = false;

    private int toggle = 0;

    private void Start() {
        videoLength = videoUI.GetComponent<VideoPlayer>().length;
        
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

    void Update() {

        if (inZone)
        {
            
            if(Input.GetKeyDown(KeyCode.Q))
            {
                toggle++;
                  
            }

            if (Input.GetKeyDown(KeyCode.Q) && videoUI.activeSelf == true && toggle%2==0)
            {
                videoUI.SetActive(false);
                
                if (blackDrop != null)
                {
                    blackDrop.SetActive(false);
                }
                interactionTrigger.SetActive(true);
            } 
 
            if (Input.GetKeyDown(KeyCode.Q) && videoUI.activeSelf == false && toggle%2==1)
            {
                videoUI.SetActive(true);
                
                if (blackDrop != null)
                {
                    blackDrop.SetActive(true);
                }

                timer = Time.time + videoLength;
                interactionTrigger.SetActive(false);

            } 

           if (Time.time > timer)
            {
                videoUI.SetActive(false);
                
                if (blackDrop != null)
                {
                    blackDrop.SetActive(false);
                }

                toggle = 0;
                interactionTrigger.SetActive(true);
                
     
            }


        }

        if (!inZone)
        {
            videoUI.SetActive(false);
            
            if (blackDrop != null)
            {
                blackDrop.SetActive(false);
            }
            
            toggle = 0;
            //interactionTrigger.SetActive(false);
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
