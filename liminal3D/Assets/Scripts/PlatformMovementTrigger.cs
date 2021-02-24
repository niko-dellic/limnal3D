using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovementTrigger : MonoBehaviour
 {
     public bool EnteredTrigger;
 
     public void OnTriggerEnter(Collider other)
     {
         if (other.tag == "PLATFORM")
             EnteredTrigger = true;
     }
 }
 
 public class OtherScript : MonoBehaviour
 {
     private GameObject m_Player;
     private void Start()
     {
         m_Player = GameObject.FindWithTag("PLAYER");
     }
 
     private void Update()
     {
         m_Player = GameObject.FindWithTag("PLAYER");

         if (m_Player.GetComponent<PlatformMovementTrigger>().EnteredTrigger)
             {
                Debug.Log("swag");
                m_Player.transform.parent = transform;
             }
     }
 }
