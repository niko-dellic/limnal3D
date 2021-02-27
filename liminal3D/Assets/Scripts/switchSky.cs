using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchSky : MonoBehaviour
{

    [Header ("SKY")]
    [SerializeField] public Material altSky; 
    private Material regularSky;

    [Header ("FOG")]
    public Color FogColor;
    private Color defaultFogColor;
    public float fogDistance = 0.005f;
    private float defaultFogDistance;

    [Header ("AMBIENT")]
    public Color AmbientColor;
    private Color defaultAmbientColor;


    
    private void Start()
    {
        regularSky = RenderSettings.skybox;    
        defaultFogColor = RenderSettings.fogColor;
        defaultAmbientColor = RenderSettings.ambientLight;
        defaultFogDistance = RenderSettings.fogDensity;
    }

    private void OnTriggerEnter(Collider other) 
    {       
            // Debug.Log("altSky");

            if(other.tag ==  "PLAYER_CLONE" || other.tag == "HOST")
            {
                if(altSky != null)
                {
                    RenderSettings.skybox = altSky;  
                }
                
                
                RenderSettings.fogColor = FogColor;
                RenderSettings.ambientLight = AmbientColor;
                RenderSettings.fogDensity = fogDistance;
                        
            } 

    }

    

    private void OnTriggerExit(Collider other) 
    {       
        if(other.tag ==  "PLAYER_CLONE" || other.tag == "HOST")
        {
            RenderSettings.skybox = regularSky;    
            RenderSettings.fogColor = defaultFogColor;    
            RenderSettings.ambientLight = defaultAmbientColor;
            RenderSettings.fogDensity = defaultFogDistance;

        }    
    }     
    
}
