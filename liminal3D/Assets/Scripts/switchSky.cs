using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchSky : MonoBehaviour
{

    [HideInInspector]
    public bool inZone;

    [Header("SKYLIGHT")]
    public GameObject sunLight;
    private float sunLightIntensity;

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
    private float counter = 0;
    private bool startCounterUp;
    private bool startCounterDown;

    public GameObject postPro;

    


    
    private void Start()
    {
        //locate postPro
        //postPro = GameObject.FindGameObjectWithTag("POSTPRO");

        regularSky = RenderSettings.skybox;    
        defaultFogColor = RenderSettings.fogColor;
        defaultAmbientColor = RenderSettings.ambientLight;
        defaultFogDistance = RenderSettings.fogDensity;

        if (inZone == false && sunLight != null)
        {
            sunLight.SetActive(false);
            sunLightIntensity = sunLight.GetComponent<Light>().intensity;
            sunLight.GetComponent<Light>().intensity = 0f;

        }
    }

    
    private void OnTriggerExit(Collider other) 
    {       
        if(other.tag ==  "PLAYER_CLONE" || other.tag == "HOST")
        {
            
            // if (postPro.GetComponent<enableAR>().ARToggle == true)
            // {
                postPro.GetComponent<enableAR>().exitOvveride = true;
            // }

            // if (postPro.GetComponent<enableAR>().ARToggle == false)
            // {
            //     RenderSettings.skybox = regularSky;    
            //     RenderSettings.fogColor = defaultFogColor;    
            //     RenderSettings.ambientLight = defaultAmbientColor;
            //     RenderSettings.fogDensity = defaultFogDistance;
            // }

            inZone = false;
            
            if (sunLight != null)
            {
                //sunLight.SetActive(false); //disable overhead light
                startCounterDown = true;
                
            }

        

        }    
    }   

    
    private void OnTriggerEnter(Collider other) 
    {       
            if(other.tag ==  "PLAYER_CLONE" || other.tag == "HOST")
            {
                if(altSky != null)
                {
                    RenderSettings.skybox = altSky;  
                }
                
                RenderSettings.fogColor = FogColor;
                RenderSettings.ambientLight = AmbientColor;
                RenderSettings.fogDensity = fogDistance;
                inZone = true;
                
                if (sunLight != null)
                {
                    sunLight.SetActive(true); //enable overhead light
                    startCounterUp = true;
                }
                        
            } 

    }

    private void Update() {
        
        if (sunLight != null)
        {
            //sunLight.transform.LookAt(Camera.main.transform);

            if (startCounterUp == true)
            {
                sunLight.GetComponent<Light>().intensity = Mathf.Lerp(0f,sunLightIntensity,counter);
                counter = counter + 0.01f;
            }
        }

        if (sunLight != null && startCounterDown == true)
        {
            sunLight.GetComponent<Light>().intensity = Mathf.Lerp(sunLightIntensity, 0f,counter);
            counter = counter + 0.01f;
        }

        if (counter > 1)
        {
            startCounterUp = false;
            startCounterDown = false;
            counter = 0f;
        }

        
    }

      
    
}
