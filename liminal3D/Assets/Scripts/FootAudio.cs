using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootAudio : MonoBehaviour
{

       public GameObject water;
       muteSteps muteStepsScript;
       
       public bool muteWater;
          
         CharacterController cc;
       void Start () 
       {
              cc = GetComponent<CharacterController>();
              
       }
 
        void Update () 
       {
              muteStepsScript = water.GetComponent<muteSteps>();
              muteWater = muteStepsScript.inWater;

              //Debug.Log(muteWater);

              if (cc.isGrounded == true && cc.velocity.magnitude > 0f && GetComponent<AudioSource>().isPlaying == false && muteWater == false)
              {
                     GetComponent<AudioSource>().volume = Random.Range(0.01f, 0.03f);
                     GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
                     GetComponent<AudioSource>().Play();
              }
       }
}
