using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animateOnPress : MonoBehaviour

{

    [SerializeField] public Animator anim;
    
    private bool onTrigger = false;

    [Header("Sliding Door")]
    [SerializeField] public GameObject slidingDoor;
    private Animator doorAnim;

    [Header("Top Gears")]
    [SerializeField] public GameObject gear01;
    [SerializeField] public GameObject gear02;
    private Animator gearAnim01;
    private Animator gearAnim02;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        doorAnim = slidingDoor.GetComponent<Animator>();
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

    public void Update()
    {
        if (onTrigger == true && Input.GetKeyDown(KeyCode.Q))
        {
            anim.Play("Choochoo");
            doorAnim.Play("TrainDoorClose");
            gearAnim01.Play("CogRot1");
            gearAnim02.Play("CogRot1");

            //TriggerObject.GetComponent<BoxCollider>();

        }

    }
}
