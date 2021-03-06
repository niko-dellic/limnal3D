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
    // [SerializeField] public GameObject gear01;
    // [SerializeField] public GameObject gear02;

    [SerializeField] public GameObject gear01;
    [SerializeField] public GameObject gear02;
    private Animator gearAnim01;
    private Animator gearAnim02;

    [Header("Additional Objects to Animate")]
    
    //Second train
    public GameObject chooChoo2;
    private Animator bonusAnim;

    //Second Gears
    public GameObject bonusGear01;
    public GameObject bonusGear02;
    private Animator bonusGearAnim01;
    private Animator bonusGearAnim02;

    //Second Door
    public GameObject bonusDoor;
    private Animator bonusDoorAnim;


    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        doorAnim = slidingDoor.GetComponent<Animator>();
        gearAnim01 = gear01.GetComponent<Animator>();
        gearAnim02 = gear02.GetComponent<Animator>();

        //second train
        bonusAnim = chooChoo2.GetComponent<Animator>();
        bonusGearAnim01 = bonusGear01.GetComponent<Animator>();
        bonusGearAnim02 = bonusGear02.GetComponent<Animator>();
        bonusDoorAnim = bonusDoor.GetComponent<Animator>();




    }

    private void OnTriggerEnter(Collider other) 
    {       
        
        if(other.tag ==  "PLAYER_CLONE" || other.tag == "HOST")
        {
             onTrigger = true;           
        }    
    }

    private void OnTriggerExit(Collider other) 
    {       
        
        if(other.tag ==  "PLAYER_CLONE" || other.tag == "HOST")
        {
              onTrigger = false;          
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
            bonusAnim.Play("ChooChoo2");
            bonusGearAnim01.Play("CogRot1");
            bonusGearAnim02.Play("CogRot1");
            bonusDoorAnim.Play("TrainDoorClose");

            //TriggerObject.GetComponent<BoxCollider>();

        }

    }
}
