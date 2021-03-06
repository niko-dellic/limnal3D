using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class theaterScreen : NetworkBehaviour
{
    
    
    public GameObject screenObject;
    public Material defaultMat;
    public Material hostStreamPixelated;
    public Material hostStreamHiRes;

    void Start()
    {
        screenObject.gameObject.GetComponent<MeshRenderer>().material = defaultMat;
                
    }


    // void CmdPickupItem(NetworkIdentity item)
    // {
    //     item.AssignClientAuthority(connectionToClient); 
    // }
    
    void Update()
    {

        
        // GameObject player = GameObject.FindGameObjectWithTag("PLAYER_CLONE");
        // NetworkIdentity playerID = player.GetComponent<NetworkIdentity>();

        // if (Input.GetKeyDown(KeyCode.L))
        // {
        //     // CmdenableStream(playerID);
        //     CmdenableStream();
        //     Debug.Log("l pressed");

        // }

    }
    
    // [Command (ignoreAuthority=true)]
    // void CmdenableStream() //(NetworkIdentity item)
    // {
    //     //item.AssignClientAuthority(connectionToClient); 
    //     screenObject.gameObject.GetComponent<MeshRenderer>().material = hostStreamHiRes;
    //     CmdenableStreamSpectate();
   
    // }

    // [ClientRpc]
    // void CmdenableStreamSpectate()
    // {
    //     screenObject.gameObject.GetComponent<MeshRenderer>().material = hostStreamHiRes;
    // }


    void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "HOST")
        {
            screenObject.gameObject.GetComponent<MeshRenderer>().material = hostStreamHiRes;
        }
        
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "HOST")
        {
            screenObject.gameObject.GetComponent<MeshRenderer>().material = hostStreamPixelated;
        }
    }

}
