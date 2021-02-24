using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class Pause : MonoBehaviour {
     
     public GameObject Canvas;
     public GameObject Camera;
     bool Paused = false;
 
     void Start(){
         Canvas.gameObject.SetActive (true);
     }
 
     void Update () {
         if (Input.GetKeyDown ("escape")) {
             if(Paused == true){
                 Time.timeScale = 1.0f;
                 Paused = false;
             } else {
                 Time.timeScale = 0.0f;
                 Paused = true;
                 Debug.Log("Paused");
             }
         }
     }
     public void Resume(){
         Time.timeScale = 1.0f;
     }
 }    