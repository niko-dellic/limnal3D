using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Mirror;

public class videoTrigger : NetworkBehaviour
{

    public GameObject videoUI;
    public GameObject blackDrop;
    private double videoLength;

    public GameObject videoPlane;

    public Material videoMaterial;

    private double timer;

 
    private void OnTriggerEnter(Collider other) {
        
        if (other.tag == "HOST")
        {

        CmdPlayVideo();

        videoLength = videoUI.GetComponent<VideoPlayer>().length;
        timer = Time.time + videoLength;
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        
    }

    private void Update() {
        if (videoPlane.activeSelf == true && Time.time > timer)
        {
            videoUI.SetActive(false);
            blackDrop.SetActive(false);
            //videoPlane.SetActive(false);
        }

    }

    [Command (ignoreAuthority = true)]
    void CmdPlayVideo() {
        RpcPlayVideoServer();
    }

    [ClientRpc]
    void RpcPlayVideoServer() {
        videoUI.SetActive(true);
        blackDrop.SetActive(true);
        videoPlane.SetActive(true);
    }
}
