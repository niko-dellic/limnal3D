using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class enableAR : MonoBehaviour
{

    [Header("Power Sounds")]
    public AudioSource PowerUP;
    public AudioSource PowerDOWN;

    [Header("Materials")]
    public Material realWorldMat;
    public Material defaultMat;

    private List<GameObject> ARObjects = new List<GameObject>();
    private List<GameObject> hybridObjects = new List<GameObject>();
    private List<GameObject> realOnlyObjects = new List<GameObject>();

    private bool ARToggle = false;
    private bool lerp = false;
    private float Counter;
    private float interp = 0f; 
    private float negInterp = 0f;

    [Header("PostPro")]

    public Volume volume;
    private ColorAdjustments colorAdjustments;
    private float defaultContrast;
    private float defaultSaturation;
    public float newContrast = -25f;
    public float newSaturation = -100f;
    
    private List<Material> startMat = new List<Material>();
    private List<Material> startHybridMat = new List<Material>();
    private List<Material> startARMat = new List<Material>();

    // Start is called before the first frame update
    void Start()
    {
        //postPro
        var postProEffects = volume.GetComponent<Volume>();
        postProEffects.profile.TryGet(out colorAdjustments);

        defaultContrast = colorAdjustments.contrast.value;
        defaultSaturation = colorAdjustments.saturation.value;

        //set start values
        colorAdjustments.contrast.value = newContrast;
        colorAdjustments.saturation.value = newSaturation;

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
            
            if (realArray[i].GetComponent<MeshRenderer>() != null)
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


        
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.V))
        {
            lerp = true;
            ARToggle = !ARToggle;
            
        }

        if (ARToggle && Input.GetKeyUp(KeyCode.V))
        {
            PowerUP.Play();
        }

        if(ARToggle)
        {

            if (lerp == true)
            {   

                Counter = Counter + Time.deltaTime; 
                interp = Mathf.Lerp(0f,1f,Counter);
                negInterp = Mathf.Lerp(1f,0f,Counter);

                //postPro
                float currentContrast = colorAdjustments.contrast.value;
                float currentSaturation = colorAdjustments.contrast.value;
                colorAdjustments.contrast.value = Mathf.Lerp(currentContrast, defaultContrast,interp);
                colorAdjustments.saturation.value = Mathf.Lerp(currentSaturation, defaultSaturation,interp);

        
                if (Counter > 1)
                {
                    lerp = false;
                    Counter = 0;
                }          


                defaultMat.SetFloat("_Opacity", interp);
                realWorldMat.SetFloat("_Opacity", negInterp);
                
                for (int i = 0; i < startARMat.Count; i++)
                {
                    startARMat[i].SetFloat("_Opacity", interp);
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
                    
                }
 
            }
                    
        }            

        if(!ARToggle)
        {

            if (lerp == true)
            {   
                Counter = Counter + Time.deltaTime; 
                interp = Mathf.Lerp(0f,1f, Counter);
                negInterp = Mathf.Lerp(1f,0f,Counter);
                defaultMat.SetFloat("_Opacity", negInterp);
                realWorldMat.SetFloat("_Opacity", interp);

                //postPro
                float currentContrast = colorAdjustments.contrast.value;
                float currentSaturation = colorAdjustments.contrast.value;
                colorAdjustments.contrast.value = Mathf.Lerp(currentContrast, newContrast, interp);
                colorAdjustments.saturation.value = Mathf.Lerp(currentSaturation, newSaturation, interp);


                for (int i = 0; i < startARMat.Count; i++)
                {
                    startARMat[i].SetFloat("_Opacity", negInterp);
                }

           

                if (Counter > 1)
                {
                    lerp = false;
                    Counter = 0;
                
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

                foreach (GameObject i in hybridObjects)
                {
                    if (Counter > 0.5f)
                    {
                        i.GetComponent<MeshRenderer>().material = realWorldMat;
                    }
                }


                if (Counter > 0.75f)
                {

                    for (int i = 0; i < realOnlyObjects.Count; i++)
                    { 
                        realOnlyObjects[i].GetComponent<MeshRenderer>().material = startMat[i];
                    }

                }
     
                if (Input.GetKeyUp(KeyCode.V))
                {
                    PowerDOWN.Play();
                 
                }
            }  
        }
    }
}
