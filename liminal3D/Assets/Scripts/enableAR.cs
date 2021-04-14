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

    private List<GameObject> ARObjects = new List<GameObject>();
    private List<GameObject> hybridObjects = new List<GameObject>();
    private List<GameObject> realOnlyObjects = new List<GameObject>();
    private List<GameObject> realOnlyLights = new List<GameObject>();

    private bool ARToggle = false;
    private bool lerp = false;
    private float Counter;
    private float interp = 0f; 
    private float negInterp = 0f;

    [Header("PostPro")]

    public Volume volume;
    private ChromaticAberration ChromaticAberration;
    private LensDistortion LensDistortion;
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

    private float timeTrigger;


    // Start is called before the first frame update
    void Start()
    {
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

        //postPro
        var postProEffects = volume.GetComponent<Volume>();
        postProEffects.profile.TryGet(out ChromaticAberration);
        postProEffects.profile.TryGet(out LensDistortion);
        
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

        ARToggle = true;
        lerp = true;
 
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && multiplayerMenu.activeSelf == false)
        {
            lerp = true;
            ARToggle = !ARToggle;
            timeTrigger = Time.time + ArUISpeed;
            
        }

        if (ARToggle && Input.GetKeyUp(KeyCode.E) && multiplayerMenu.activeSelf == false)
        {
            PowerUP.Play();
        }

        if(ARToggle)
        {
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
                if(altSky != null)
                {
                    RenderSettings.skybox = altSky;  
                }
                
                RenderSettings.fogColor = FogColor;
                RenderSettings.ambientLight = AmbientColor;
                RenderSettings.fogDensity = fogDistance;

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

                //Reset Sky
                RenderSettings.skybox = regularSky;    
                RenderSettings.fogColor = defaultFogColor;    
                RenderSettings.ambientLight = defaultAmbientColor;
                RenderSettings.fogDensity = defaultFogDistance;

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
                LensDistortion.intensity.value = -superSin/distortionIntensity;

                if (Counter > 0.15)
                {
                    ChromaticAberration.intensity.value = superSin;
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
                    LensDistortion.intensity.value = defaultLensDistortion;
                    
                    
                
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
                 
                }
            }  
        }
    }
}
