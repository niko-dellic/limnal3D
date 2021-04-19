using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Mirror;

public class enableAR : NetworkBehaviour
{
    
    [HideInInspector]
    public GameObject[] ditherArrayAR;
    public GameObject overrideZones;
    public bool exitOvveride = false;
    public Volume volume1;
    public Volume volume2;

    [Header("MINIMAP OVERLAY CAMERAS")]
    public GameObject miniMapCam;
    public GameObject miniMapPostProPlane;
    public GameObject miniMapSectionCam;
    public GameObject miniMapSectionPostProPlane;


    [Header("Pixel UI")]
    public GameObject pixelCam;
    public RenderTexture pixelRT;

    [Header("3D GUI INFO")]
    public GameObject xyzGUI;
    private RawImage xyzGUIRawImg;
    public GameObject xyz3D;

    [Header("MISC.")]
    public GameObject multiplayerMenu;
    public GameObject arUI;
    RawImage arUIRawImg;
    public float ArUISpeed = 0.5f;
    public GameObject fireworks;
    
    [Header("Power Sounds")]
    public AudioSource PowerUP;
    public AudioSource PowerDOWN;

    [Header("Materials")]
    public Material realWorldMat;
    public Material defaultMat;

    private List<GameObject> RealSound = new List<GameObject>();
    private List<GameObject> ARSound = new List<GameObject>();
    private List<GameObject> ARObjects = new List<GameObject>();
    private List<GameObject> hybridObjects = new List<GameObject>();
    private List<GameObject> realOnlyObjects = new List<GameObject>();
    private List<GameObject> realOnlyLights = new List<GameObject>();

    [HideInInspector]
    public bool ARToggle = false;
    
    [HideInInspector]
    public bool lerp = false;
    
    [HideInInspector]
    public float Counter;
    private float interp = 0f; 
    private float negInterp = 0f;

    [Header("PostPro")]

    public Volume volume;
    private ChromaticAberration ChromaticAberration;
    private ChromaticAberration ChromaticAberration2;
    private LensDistortion LensDistortion;
    private LensDistortion LensDistortion2;
    
    private float defaultLensDistortion;

    [Range(1.0f,2.0f)]
    public float distortionIntensity = 1f;
    public float chromaticAR;
    float superSin = 0f;
   
    private List<Material> startMat = new List<Material>();
    private List<Material> startHybridMat = new List<Material>();
    private List<Material> startARMat = new List<Material>();

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

    [HideInInspector]
    public float timeTrigger;

    private bool skyCounter;

    private float fogDensityStart;

    private float fogDensityTarget;

    //sky colors
    float startFogR;
    float startFogG;
    float startFogB;
    float startAmbientR;
    float startAmbientG;
    float startAmbientB;


    // Start is called before the first frame update
    private void OnApplicationQuit() {

        defaultMat.SetFloat("_Opacity", 1);
        realWorldMat.SetFloat("_Opacity", 1);


    }

    //disable dither Zones on AR Toggle
    public void ditherxRef(bool ditherSwitch)
    {
        for (int i = 0; i < ditherArrayAR.Length; i++)
        {
            ditherArrayAR[i].SetActive(ditherSwitch);    
        }

        // if (ditherSwitch == false)
        // {
        //     videoMode = true;
        // }

        // if (ditherSwitch == true)
        // {
        //     videoMode = false;
        // }
    }

    
    void Start()
    {
        //DITHER ZONES
        ditherArrayAR = GameObject.FindGameObjectsWithTag("DITHERZONE");

        //UI
        arUIRawImg = arUI.GetComponent<RawImage>();
        xyzGUIRawImg = xyzGUI.GetComponent<RawImage>(); 

        //fireworks
        fireworks.SetActive(false);

        //Sky and Environment
        regularSky = RenderSettings.skybox;    
        defaultFogColor = RenderSettings.fogColor;
        defaultAmbientColor = RenderSettings.ambientLight;
        defaultFogDistance = RenderSettings.fogDensity;

        //postPro - there are two volumes because i couldnt find the dither override so i just copied everything else and enable and disable the entire thing

        //var postProEffects = volume.GetComponent<Volume>();
        
        var postProEffects = volume1;
        var postProEffects2 = volume2;

        //volume 1
        postProEffects.profile.TryGet(out ChromaticAberration);
        postProEffects.profile.TryGet(out LensDistortion);

        //volume 2
        postProEffects2.profile.TryGet(out ChromaticAberration2);
        postProEffects2.profile.TryGet(out LensDistortion2);


        defaultLensDistortion = LensDistortion.intensity.value;
        ChromaticAberration.intensity.value = 0f;

        //set texture
        realWorldMat.SetFloat("_Opacity", 1f);
              
        //Get AR Objects
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("AR").Length; i++)
        {
            GameObject[] arArray = GameObject.FindGameObjectsWithTag("AR");
            
            if (arArray[i].GetComponent<MeshRenderer>() != null)
            {
                ARObjects.Add(arArray[i]);
            }

            if (arArray[i].GetComponent<AudioSource>() != null) 
            {
                ARSound.Add(arArray[i]);
            }

        }

        //Get hybrid Objects
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("HYBRID").Length; i++)
        {
            GameObject[] realArray = GameObject.FindGameObjectsWithTag("HYBRID");
            
            if (realArray[i].GetComponent<MeshRenderer>() != null )
            {
                hybridObjects.Add(realArray[i]);
            }    
        }
        
        //Get RealOnly Objects
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("RealOnly").Length; i++)
        {
            GameObject[] realOnlyArray = GameObject.FindGameObjectsWithTag("RealOnly");
            
            if (realOnlyArray[i].GetComponent<MeshRenderer>() != null)
            {
                realOnlyObjects.Add(realOnlyArray[i]);
            } 

            if (realOnlyArray[i].GetComponent<AudioSource>() != null) 
            {
                RealSound.Add(realOnlyArray[i]);
            }
        }

        //Get RealOnly Lights
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("RealOnly").Length; i++)
        {
            GameObject[] realOnlyArray = GameObject.FindGameObjectsWithTag("RealOnly");
            
            if (realOnlyArray[i].GetComponent<Light>() != null)
            {
                realOnlyLights.Add(realOnlyArray[i]);
            }    
        }
        
        for (int i = 0; i < hybridObjects.Count; i++)
        {
            if (hybridObjects[i].GetComponent<MeshRenderer>() != null)
            {
               startHybridMat.Add(hybridObjects[i].GetComponent<MeshRenderer>().material);
            }
        }

        for (int i = 0; i < ARObjects.Count; i++)
        {
            if (ARObjects[i].GetComponent<MeshRenderer>() != null)
            {
               startARMat.Add(ARObjects[i].GetComponent<MeshRenderer>().material);
                
            }
        }

        for (int i = 0; i < realOnlyObjects.Count; i++)
        {
            if (realOnlyObjects[i].GetComponent<MeshRenderer>() != null)
            {
                startMat.Add(realOnlyObjects[i].GetComponent<MeshRenderer>().material);

            }
        }
        for (int i = 0; i < ARObjects.Count; i++)
        {
            if (ARObjects[i].GetComponent<MeshRenderer>() != null)
            {
                ARObjects[i].GetComponent<MeshRenderer>().enabled = false;
            }
            if (ARObjects[i].GetComponent<MeshCollider>() != null)
            {
                ARObjects[i].GetComponent<MeshCollider>().enabled = false;
            }
        }


        for (int i = 0; i < hybridObjects.Count; i++)
        {
            if (hybridObjects[i].GetComponent<MeshRenderer>() != null)
           {
               hybridObjects[i].GetComponent<MeshRenderer>().material = realWorldMat;
           }
            
        }

        volume2.enabled = false;
        ARToggle = true;
        lerp = true;
 
    }

    void Update()
    {

        if (exitOvveride == true)
        {
            lerp = true;
        }

        if (Input.GetKeyDown(KeyCode.E) && multiplayerMenu.activeSelf == false)
        {
            lerp = true;
            ARToggle = !ARToggle;
            timeTrigger = Time.time + ArUISpeed;     
        }

        if (ARToggle && Input.GetKeyUp(KeyCode.E) && multiplayerMenu.activeSelf == false)
        {
            PowerUP.Play();
            volume2.enabled = false;
            volume1.enabled = true;
        }


        void changeEnvironment (Color resultAmbientColor, Color resultFogColor, float FogThickness, Material alternateSky)
        {
            
            if(altSky != null)
            {
                RenderSettings.skybox = alternateSky;  
            }

            if (skyCounter == false)
            {
                fogDensityStart = RenderSettings.fogDensity;
                        
                //Start Fog Color
                startFogR = RenderSettings.fogColor.r;
                startFogG = RenderSettings.fogColor.g;
                startFogB = RenderSettings.fogColor.b;

                //Start Ambient Color
                startAmbientR = RenderSettings.ambientLight.r;
                startAmbientG = RenderSettings.ambientLight.g;
                startAmbientB = RenderSettings.ambientLight.b;

                skyCounter = true;

            }

            //LERP FOG COLOR
            //GET Target FOG COLOR
            float targetFogR = resultFogColor.r;
            float targetFogG = resultFogColor.g;
            float targetFogB = resultFogColor.b;

            //CREATE NEW COLOR
            Color destinationFog = new Color (Mathf.Lerp(startFogR,targetFogR,Counter),Mathf.Lerp(startFogG,targetFogG,Counter),Mathf.Lerp(startFogB,targetFogB,Counter));
                    
            //APPLY NEW COLOR
            RenderSettings.fogColor = destinationFog;

            //LERP AMBIENT COLOR
            float targetAmbientR = resultAmbientColor.r;
            float targetAmbientG = resultAmbientColor.g;
            float targetAmbientB = resultAmbientColor.b;

            //CREATE NEW COLOR FOR AMBIENT LIGHT
            Color desinationAmbient = new Color (Mathf.Lerp(startAmbientR,targetAmbientR, Counter),Mathf.Lerp(startAmbientG,targetAmbientG, Counter),Mathf.Lerp(startAmbientB,targetAmbientB, Counter));
            //Color desintation
            RenderSettings.ambientLight = desinationAmbient;
                    
            //Change Fog Density
            RenderSettings.fogDensity = Mathf.Lerp(fogDensityStart, FogThickness, Counter);
        }

        if(ARToggle)
        {
            for (int i = 0; i < ditherArrayAR.Length; i++)
            {
                if (ditherArrayAR[i].activeSelf == false)
                {
                    ditherArrayAR[i].SetActive(true);
                }
            }

            if (miniMapCam.activeSelf == false)
            {
                miniMapCam.SetActive(true);
                miniMapPostProPlane.SetActive(true);

            }

            if (miniMapSectionCam.activeSelf == false)
            {
                miniMapSectionCam.SetActive(true);
                miniMapSectionPostProPlane.SetActive(true);
            }

            if (Camera.main != null && Camera.main.GetComponent<Camera>().targetTexture == null)
            {
                Camera.main.GetComponent<Camera>().targetTexture = pixelRT;
            }

            if (lerp == true)
            {   
                
                //enable pixel cam
                pixelCam.SetActive(true);
                 
                //enable UI
                arUI.SetActive(true);
                arUIRawImg.CrossFadeAlpha(1,ArUISpeed,false);

                xyzGUI.SetActive(true);
                xyz3D.SetActive(true);
                xyzGUIRawImg.CrossFadeAlpha(1,ArUISpeed,false);

                //enable Fireworks
                //fireworks.SetActive(true);

                Counter = Counter + Time.deltaTime; 
                interp = Mathf.Lerp(0f,1f,Counter);
                negInterp = Mathf.Lerp(1f,0f,Counter);

                //Change Sky
                if (overrideZones.GetComponent<switchSky>().inZone == false)
                {
                    changeEnvironment(AmbientColor, FogColor, fogDistance, altSky);
                }

                //postPro
                superSin = Mathf.Sin(Counter*Mathf.PI); //set undulation value
                LensDistortion.intensity.value = -superSin/distortionIntensity;
                ChromaticAberration.intensity.value = superSin;

                if (Counter > 0.83)
                {
                    ChromaticAberration.intensity.value = chromaticAR;
                }

                if (Counter > 1)
                {
                    lerp = false;
                    Counter = 0;
                    skyCounter = false;
                    exitOvveride = false;
                    LensDistortion.intensity.value = defaultLensDistortion;

                    for (int i = 0; i < realOnlyObjects.Count; i++)
                    {
                        if (realOnlyObjects[i].GetComponent<MeshRenderer>() != null)
                        {  
                            realOnlyObjects[i].SetActive(false);              
                        }     
                    }
                }          


                defaultMat.SetFloat("_Opacity", interp);
                realWorldMat.SetFloat("_Opacity", negInterp);
                
                for (int i = 0; i < startARMat.Count; i++)
                {
                    if (startARMat[i].HasProperty("_Opacity") && startARMat[i].GetFloat("_Opacity") != 1)
                    {
                    startARMat[i].SetFloat("_Opacity", interp);
                    }
                }

                for (int i = 0; i < startHybridMat.Count; i++)
                {
                    if (startHybridMat[i].HasProperty("_Opacity") && startHybridMat[i].GetFloat("_Opacity") != 1)
                    {
                    startHybridMat[i].SetFloat("_Opacity", interp);
                    }
                }

                //Enable AR Audio
                for (int i = 0; i < ARSound.Count; i++)
                {
                    ARSound[i].GetComponent<AudioSource>().enabled = true;
                }

                //DISABLE Real Audio
                for (int i = 0; i < RealSound.Count; i++)
                {
                    RealSound[i].GetComponent<AudioSource>().enabled = false;
                }


                for (int i = 0; i < ARObjects.Count; i++)
                {
 
                    if (ARObjects[i].GetComponent<MeshRenderer>() != null)
                    {
                        ARObjects[i].GetComponent<MeshRenderer>().enabled = true; 
                        ARObjects[i].GetComponent<MeshRenderer>().material = startARMat[i];
                    }

                    if (ARObjects[i].GetComponent<MeshCollider>() != null)
                    {
                        ARObjects[i].GetComponent<MeshCollider>().enabled = true;
                    }
                }


                //change material of hybrid objects
                
                for (int i = 0; i < hybridObjects.Count; i++)
                {
                    if (Counter > 0.5f)
                    {
                        hybridObjects[i].GetComponent<MeshRenderer>().material = startHybridMat[i];
                    }
                }
               
                for (int i = 0; i < realOnlyObjects.Count; i++)
                {
                    if (realOnlyObjects[i].GetComponent<MeshRenderer>() != null)
                    {  
                        realOnlyObjects[i].GetComponent<MeshRenderer>().material = realWorldMat;                      
                    }
                    
                    if (realOnlyObjects[i].GetComponent<Collider>() != null)
                    {
                        realOnlyObjects[i].GetComponent<Collider>().enabled = false;
                    }
                }

                for (int i = 0; i < realOnlyLights.Count; i++)
                {
                    if (realOnlyLights[i].GetComponent<Light>() != null)
                    {
                        realOnlyLights[i].GetComponent<Light>().enabled = false;
                    }
                }
 
            }
                    
        }            

        if(!ARToggle)
        {
            for (int i = 0; i < ditherArrayAR.Length; i++)
            {
                if (ditherArrayAR[i].activeSelf == true)
                {
                    ditherArrayAR[i].SetActive(false);
                }
            }

            if (Camera.main != null && Camera.main.GetComponent<Camera>().targetTexture != null)
            {
                //disable pixel cam
                pixelCam.SetActive(false);
                Camera.main.GetComponent<Camera>().targetTexture = null;
            }
            

            if (lerp == true)
            {   
                

                //disable ui
                arUIRawImg.CrossFadeAlpha(0,ArUISpeed,false);
                xyzGUIRawImg.CrossFadeAlpha(0,ArUISpeed,false);

                if (Time.time > timeTrigger)
                {
                    //arUI.SetActive(false);
                    xyzGUI.SetActive(false);
                    xyz3D.SetActive(false);
                }

                

                //Reset Sky Normally
                if (overrideZones.GetComponent<switchSky>().inZone == false)
                {
                    changeEnvironment(defaultAmbientColor, defaultFogColor, defaultFogDistance, regularSky);
                }

                // Reset Sky if in an Overide Zone
                if (overrideZones.GetComponent<switchSky>().inZone == true)
                {
                    Color overrideAmbient = overrideZones.GetComponent<switchSky>().AmbientColor;
                    Color overrideFog = overrideZones.GetComponent<switchSky>().FogColor;
                    float overideFogDistance = overrideZones.GetComponent<switchSky>().fogDistance; 
                    Material overideSky = RenderSettings.skybox;

                    if (overrideZones.GetComponent<switchSky>().altSky != null)
                    {
                        overideSky =  overrideZones.GetComponent<switchSky>().altSky;
                    }

                    changeEnvironment(overrideAmbient, overrideFog, overideFogDistance, overideSky);
                }

                //fireworks
                //fireworks.SetActive(false);

                float superSIn = Mathf.Sin(Counter*Mathf.PI);

                Counter = Counter + Time.deltaTime; 
                interp = Mathf.Lerp(0f,1f, Counter);
                negInterp = Mathf.Lerp(1f,0f,Counter);

                defaultMat.SetFloat("_Opacity", negInterp);
                realWorldMat.SetFloat("_Opacity", interp);
                
            
                //postPro
                superSin = Mathf.Sin(Counter*Mathf.PI); //set undulation value
                LensDistortion2.intensity.value = -superSin/distortionIntensity; //post pro 2 added here

                if (Counter > 0.15)
                {
                    ChromaticAberration2.intensity.value = superSin; //post pro 2 added here
                }

                for (int i = 0; i < startARMat.Count; i++)
                {
                    if (startARMat[i].HasProperty("_Opacity"))
                    {
                        startARMat[i].SetFloat("_Opacity", negInterp);
                    }
                }

                for (int i = 0; i < startHybridMat.Count; i++)
                {   
                    if (startHybridMat[i].HasProperty("_Opacity"))
                    {
                        startHybridMat[i].SetFloat("_Opacity", negInterp);  
                    }
                }

                //reActrivate Real Lights
                for (int i = 0; i < realOnlyLights.Count; i++)
                {
                    if (realOnlyLights[i].GetComponent<Light>() != null)
                    {
                        realOnlyLights[i].GetComponent<Light>().enabled = true;
                    }
                    
                }

                

                if (Counter > 1)
                {
                    lerp = false;
                    Counter = 0;
                    skyCounter = false;
                    exitOvveride = false;
                    LensDistortion.intensity.value = defaultLensDistortion;

                    //Disable MiniMAP UI
                    miniMapCam.SetActive(false);
                    miniMapPostProPlane.SetActive(false);

                    miniMapSectionCam.SetActive(false);
                    miniMapSectionPostProPlane.SetActive(false);

                    //Disable AR Audio
                    for (int i = 0; i < ARSound.Count; i++)
                    {
                        ARSound[i].GetComponent<AudioSource>().enabled = false;
                    }
                    
                    //Enable Real Audio
                    for (int i = 0; i < RealSound.Count; i++)
                    {
                        RealSound[i].GetComponent<AudioSource>().enabled = false;
                    }
                    
                
                    for (int i = 0; i < ARObjects.Count; i++)
                    {
                        if (ARObjects[i].GetComponent<MeshRenderer>() != null)
                        {
                            ARObjects[i].GetComponent<MeshRenderer>().enabled = false;
                        }

                        if (ARObjects[i].GetComponent<MeshCollider>() != null)
                        {
                            ARObjects[i].GetComponent<MeshCollider>().enabled = false;
                        }  
                    }


                }

                for (int i = 0; i < hybridObjects.Count; i++)
                {
                    if (Counter > 0.5f)
                    {
                        hybridObjects[i].GetComponent<MeshRenderer>().material = realWorldMat;
                    }
                }

                for (int i = 0; i < realOnlyObjects.Count; i++)
                { 
                    if (realOnlyObjects[i].GetComponent<MeshRenderer>().material != null)
                    {
                        realOnlyObjects[i].SetActive(true);
                    }
                }


                if (Counter > 0.75f)
                {

                    for (int i = 0; i < realOnlyObjects.Count; i++)
                    { 
                        
                        if (realOnlyObjects[i].GetComponent<MeshRenderer>().material != null)
                        {
                            realOnlyObjects[i].GetComponent<MeshRenderer>().material = startMat[i];
                        }
                        if (realOnlyObjects[i].GetComponent<Collider>() != null)
                        {
                            realOnlyObjects[i].GetComponent<Collider>().enabled = true;
                        }
                    }



                }
     
                if (Input.GetKeyUp(KeyCode.E) && multiplayerMenu.activeSelf == false)
                {
                    PowerDOWN.Play();
                    volume2.enabled = true;
                    volume1.enabled = false;
                 
                }
            }  
        }
    }
}
