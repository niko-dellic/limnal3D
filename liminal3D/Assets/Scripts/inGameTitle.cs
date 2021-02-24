using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inGameTitle : MonoBehaviour
{
    public GameObject projectTitle;

    public AudioClip introSong;

    private float titleDelay;
    private float titleHideDelay;

    // public AudioManager[] audioIntro;
    void Awake()
    {
        projectTitle.SetActive(false);
        
        float invokeDelay = GameObject.Find("CustomAudioManager").GetComponent<CustomAudioManager>().longDelay;  //gameObject.GetComponent<CustomAudioManager>().longDelay;

        float introSongleng = introSong.length;

        float titleDelay = invokeDelay + introSongleng - Time.time;

        float titleHideDelay = titleDelay + 5f;

        //Debug.Log(titleDelay + "delay title" + titleHideDelay + "hide title");        
        
        Invoke("displayTitle", titleDelay);

        Invoke("hideTitle", titleHideDelay);
    }

    public void displayTitle()
    {
        projectTitle.SetActive(true);
    }

    public void hideTitle()
    {
        projectTitle.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
