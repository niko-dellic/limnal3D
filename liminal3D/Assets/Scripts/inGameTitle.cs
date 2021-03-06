using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inGameTitle : MonoBehaviour
{
    public GameObject projectTitle;

    // public float timeDelay = 13f;

    [Header("Title Timing")]
    public AudioClip introSong;
    public AudioClip robotInterupt;
    public float bonusBuffer = 0f;
    public float titleDisplayDuration = 5f;
    
    private float introSongleng;

    // private float titleDelay;
    // private float titleHideDelay;

    // private bool singlePlay = true;

    private float pressedDelay = 0;

    void Awake()
    {
        projectTitle.SetActive(false);
        
        introSongleng = introSong.length + robotInterupt.length;

        pressedDelay = GameObject.Find("CustomAudioManager").GetComponent<CustomAudioManager>().soundTimeDelay;


        // float invokeDelay = GameObject.Find("CustomAudioManager").GetComponent<CustomAudioManager>().longDelay;           

        // float titleDelay = invokeDelay + introSongleng - Time.time + timeDelay;

        // float titleHideDelay = titleDelay + 5f;    
        
        // Invoke("displayTitle", titleDelay);

        // Invoke("hideTitle", titleHideDelay);
    }


    void Update()
    {
        if(pressedDelay == 0 && Input.GetKeyDown(KeyCode.T))
        { 
            pressedDelay = Time.time + bonusBuffer;
        }

        float totalDelay = introSongleng + pressedDelay + bonusBuffer;
        float totalDuration = totalDelay + titleDisplayDuration;


        if (totalDelay < Time.time && pressedDelay > 0)
        {
            projectTitle.SetActive(true);
        }

        if (totalDuration < Time.time && pressedDelay > 0)
        {
            projectTitle.SetActive(false);
        }

    }
}
