using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class changePostProandRes : MonoBehaviour
{
    
    public Volume volume;
    public RenderTexture pixelatedRenderTexture;

    [Header("hi res mode")]
    public RenderTexture hiResRenderTexture;

    public GameObject hiResUI;

    [Header("low res mode")]
    public GameObject pixelizedUI;



    private RenderTexture cameraTexture;

    private int defaultCamWidth;
    private int defaultCamHeight;
    private Vignette vg;
    private float defaultVignette;

    private Bloom bl;

    private float defaultBloomIntensity;
    
    private FilmGrain fg;

    private float defaultFilmGrain;

    private ChromaticAberration cAb;

    private float defaultCAb;
    private float defaultBlThreshold;

   
    private void Start() {

        var postProEffects = volume.GetComponent<Volume>();

        //get postPro effects
        postProEffects.profile.TryGet(out vg);
        postProEffects.profile.TryGet(out cAb);
        postProEffects.profile.TryGet(out bl);
        postProEffects.profile.TryGet(out fg);

        //set Defaults
        defaultBlThreshold = bl.threshold.value;
        defaultCAb = cAb.intensity.value;
        defaultFilmGrain = fg.intensity.value;
        defaultBloomIntensity = bl.intensity.value;
        defaultVignette = vg.intensity.value;

        


    }
    
    void OnTriggerEnter(Collider other) {

        
        if(other.tag ==  "PLAYER_CLONE" || other.tag == "HOST")
        {
            
            //Remove Pixelization
            if (other.tag == "HOST")
            {            
                Camera.main.GetComponent<Camera>().targetTexture = hiResRenderTexture;
                hiResUI.SetActive(true);
            }
            else
            {
                Camera.main.GetComponent<Camera>().targetTexture = null;
            }

            pixelizedUI.SetActive(false);
            

            //Adjust post-Pro
            cAb.intensity.value = 0;
            bl.threshold.value = 0;
            fg.intensity.value = 0;
            bl.intensity.value = 1;
            vg.intensity.value = 0.315f;

            //Enable sun
            RenderSettings.sun.enabled = false;
        }



    }

    void OnTriggerExit(Collider other) {

        if(other.tag ==  "PLAYER_CLONE" || other.tag == "HOST")
        {
            vg.intensity.value = defaultVignette;
            cAb.intensity.value = defaultCAb;
            bl.threshold.value = defaultBlThreshold;
            fg.intensity.value = defaultFilmGrain;
            bl.intensity.value = defaultBloomIntensity;

            Camera.main.GetComponent<Camera>().targetTexture = pixelatedRenderTexture;
            pixelizedUI.SetActive(true);
            
            if (other.tag == "HOST")
            {           
                hiResUI.SetActive(false);
            }

            //reActivate sun
            RenderSettings.sun.enabled = true;
        }

    }



}

