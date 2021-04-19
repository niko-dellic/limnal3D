using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootAudio : MonoBehaviour
{

       muteSteps muteStepsScript;
       

       public float stepVolMin = 0.01f;
       public float stepVolMax = 0.03f;
       public float stepPitchMin = 0.8f;
       public float stepPitchMax = 1.2f;

       private float  horizontalVelocity;
          
       CharacterController cc;

       void Start () 
       {
              cc = GetComponent<CharacterController>();
              
       }
 
        void Update () 
       {

              if (cc.isGrounded == true && cc.velocity.magnitude > 0f && GetComponent<AudioSource>().isPlaying == false)
              {
                     GetComponent<AudioSource>().volume = Random.Range(stepVolMin, stepVolMax);

                     horizontalVelocity = this.gameObject.GetComponent<PlayerMovement>().speed;

                     float newPitch = Mathf.Sqrt(horizontalVelocity) * Random.Range(stepPitchMin, stepPitchMax);
                     GetComponent<AudioSource>().pitch = newPitch;

                     GetComponent<AudioSource>().Play();
              }
       }
}
