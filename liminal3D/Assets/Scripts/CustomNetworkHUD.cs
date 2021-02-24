using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class CustomNetworkHUD : MonoBehaviour
{
    NetworkManager manager;
    public TMP_InputField ip_InputField;

    public GameObject HostConnect_go;

    void Awake()
    {
        manager = GetComponent<NetworkManager>();
    }

    public void HostFunction()
    {
        manager.StartHost();
        //HostConnect_go.SetActive(false);
    }

    public void ConnectFunction()
    {
        
         if(ip_InputField.text=="")
         {
             manager.networkAddress = "3.96.200.97";
         }
         else
         {
             manager.networkAddress = ip_InputField.text;
         }
        
        manager.StartClient();
        //HostConnect_go.SetActive(false);
    }

    public bool isClientOnly { get; }


    public void StopConnection()
    {
        
        if(isClientOnly == true)
        {
            manager.StopClient();
            Debug.Log("I am not the host!");
        }
        else
        {
            manager.StopHost();
            Debug.Log("I am the host!");
        }
    }
}
