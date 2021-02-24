using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openTrainDoor : MonoBehaviour
{
    
    //[SerializeField] public GameObject Train;

    [Header("Sliding Door")]
    [SerializeField] public GameObject childAnimations;
    private Animator childanim;

    [Header("Top Gears")]
    [SerializeField] public GameObject gear01;
    [SerializeField] public GameObject gear02;
    private Animator gearAnim01;
    private Animator gearAnim02;

    private bool onTrigger = false;

    void Start()
    {
        childanim = childAnimations.GetComponent<Animator>();
        gearAnim01 = gear01.GetComponent<Animator>();
        gearAnim02 = gear02.GetComponent<Animator>();
    }


    private void OnTriggerEnter(Collider other) 
    {       
        onTrigger = true;     

        if(other.tag ==  "PLAYER_CLONE")
        {
                   
        }    
    }

    private void OnTriggerExit(Collider other) 
    {       
        
        onTrigger = false;  
        
        if(other.tag ==  "PLAYER_CLONE")
        {
                      
        }    
    }

    void Update()
    {
        if (onTrigger == true)
        {
            childanim.Play("TrainDoorOpen");
            gearAnim01.Play("Idle");
            gearAnim02.Play("Idle");
        }
    }
}
