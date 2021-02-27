using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class randomPlanes : MonoBehaviour
{
    public List<UnityEngine.Video.VideoClip> myVideos;

    [Header("Video Resolution")]

    [Range(0.1f,1f)]
    public float videoCompression = 0.5f;
    
    [Header("Materials")]
    public Shader shader;
 
    [Header("Transformation Settings")]

    [Range(0.1f,2f)]
    public float randomDisplacement = 0.5f; 
    
    [Range(0.01f,0.05f)]
    public float speedMin = 0.01f;

    [Range(0.05f,0.2f)]
    public float speedMax = 0.05f;

    [Range(1f,100f)]
    public float slowAngularRotation = 3f;
    private float speed;

    [Range(0.5f,1f)]
    public float minScale = 0.8f;

    [Range(1f,1.5f)]
    public float maxScale = 1.5f;

    [Header("BONUS PLANES")]

    public Material[] bonusPlaneMaterial;

    // public Texture[] imageTextures;

    [HideInInspector]
    public List<int> imageEmpty;
    public int extraPlaneCount;

    [Range(1.001f,1.005f)]
    public float bonusListDisplacement;

    [Range(1f,2f)]
    public float bonusListSpeed;

    // PARAM END
    
    private float randomScale;

    private Vector3 aspectRatio = new Vector3(1.7f,1,1); 

    private Vector3 dynamicAspectRatio;

    private Vector3 localxyz;

    private Vector3 centroid;

    
    void Awake() 
    {
               
        while (imageEmpty.Count < extraPlaneCount)
        {
            imageEmpty.Add(0);   
        }
    }


    public static IEnumerable<List<T>> SplitList<T>(List<T> locations, int nSize=30)  
    {        
        for (int i = 0; i < locations.Count; i += nSize) 
        { 
            yield return locations.GetRange(i, Mathf.Min(nSize, locations.Count - i)); 
        }  
    } 

    void Start()
    {
        localxyz = this.transform.position;
        centroid = GetComponent<Collider>().bounds.center;        
    }

    private void OnEnable()
    {

        foreach (UnityEngine.Video.VideoClip i in myVideos)
        {
            //downsamplibg the videos so my computer doesn't explode
            float compressedWidth = 1920*videoCompression;
            int intCompressedWidth = (int) compressedWidth;
            float compressedHeight = 1080*videoCompression;
            int intCompressedHeight = (int) compressedHeight;
            

            //Debug.Log(intCompressedHeight + "height" + intCompressedWidth + "width");

            //CREATE TEXTURE
            var rt = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
            rt.name = i.name;
            rt.width = intCompressedWidth;
            rt.height = intCompressedHeight;

            //set local variables and create default plane
            speed = Random.Range(speedMin,speedMax);
            randomScale = Random.Range(minScale,maxScale);   
            Vector3 planeVector1 = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
            GameObject videoPlanes = GameObject.CreatePrimitive(PrimitiveType.Plane);
            videoPlanes.name = "videoPlane";
            
            //create the video players
            var videoShell = gameObject.AddComponent<UnityEngine.Video.VideoPlayer>();
            videoShell.isLooping = true;
            videoShell.SetDirectAudioMute(0, true);
            videoShell.EnableAudioTrack(0, false);
            videoShell.renderMode = UnityEngine.Video.VideoRenderMode.RenderTexture;
            videoShell.targetTexture = rt;
            videoShell.clip = i;

            //Initial transform + Rotation
            Vector3 randomPlacement = new Vector3(Random.Range(-randomDisplacement,randomDisplacement), Random.Range(-randomDisplacement,randomDisplacement), Random.Range(-randomDisplacement,randomDisplacement));
            videoPlanes.transform.parent = this.gameObject.transform;
            videoPlanes.transform.localPosition = centroid + randomPlacement;
            videoPlanes.transform.rotation = Random.rotation;

            //Initial Scale
            Vector3 defaultScale = videoPlanes.transform.localScale;
            Vector3 videoScale = Vector3.Scale(defaultScale, aspectRatio);
            videoPlanes.transform.localScale = videoScale * randomScale;

            // physics velocity and direction and rotation
            videoPlanes.GetComponent<MeshCollider>().convex = true;
            videoPlanes.GetComponent<MeshCollider>().isTrigger = true;
            videoPlanes.AddComponent<Rigidbody>().useGravity = false;
            videoPlanes.GetComponent<Rigidbody>().velocity = planeVector1*speed;
            videoPlanes.GetComponent<Rigidbody>().angularVelocity = ((planeVector1*speed)/slowAngularRotation);
            videoPlanes.GetComponent<Rigidbody>().angularDrag = 0;

            //ASSIGN MATERIALS
            Renderer vidMats = videoPlanes.GetComponent<Renderer>();
            vidMats.material = new Material(shader);   
            vidMats.material.mainTexture = rt;
            vidMats.material.EnableKeyword("_EMISSION");
            vidMats.material.SetTexture("_EmissionMap", rt);
            vidMats.material.SetColor("_EmissionColor", Color.white);

        }

    foreach (int i in imageEmpty)
    {  

            //set local variables and create default plane
            randomDisplacement = randomDisplacement*bonusListDisplacement;
            speed = Random.Range(speedMin,speedMax)*bonusListSpeed;
            randomScale = Random.Range(minScale,maxScale);   
            Vector3 planeVector1 = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
            GameObject imagePlanes = GameObject.CreatePrimitive(PrimitiveType.Plane);
            imagePlanes.name = "imagePlane";
            
            //Initial transform + Rotation
            Vector3 randomPlacement = new Vector3(Random.Range(-randomDisplacement,randomDisplacement), Random.Range(-randomDisplacement,randomDisplacement), Random.Range(-randomDisplacement,randomDisplacement));
            imagePlanes.transform.parent = this.gameObject.transform;
            imagePlanes.transform.localPosition = centroid + randomPlacement;
            imagePlanes.transform.rotation = Random.rotation;

            // physics velocity and direction and rotation
            imagePlanes.GetComponent<MeshCollider>().convex = true;
            imagePlanes.GetComponent<MeshCollider>().isTrigger = true;
            imagePlanes.AddComponent<Rigidbody>().useGravity = false;
            imagePlanes.GetComponent<Rigidbody>().velocity = planeVector1*speed;
            imagePlanes.GetComponent<Rigidbody>().angularVelocity = (((planeVector1*speed)/slowAngularRotation)*bonusListSpeed);
            imagePlanes.GetComponent<Rigidbody>().angularDrag = 0;

            //Plane Mat + scale plane
            Material selectImageMaterial =  bonusPlaneMaterial[Random.Range(0, bonusPlaneMaterial.Length)];
            int imgWidth = selectImageMaterial.mainTexture.width;
            int imgHeight = selectImageMaterial.mainTexture.width;
            //Debug.Log("width = " + imgWidth + "height = " + imgHeight);
            imagePlanes.GetComponent<Renderer>().material = selectImageMaterial;

            //Scale
            dynamicAspectRatio = new Vector3((imgWidth/imgHeight),1,1);
            Vector3 defaultScale = imagePlanes.transform.localScale;
            Vector3 videoScale = Vector3.Scale(defaultScale, dynamicAspectRatio);
            imagePlanes.transform.localScale = videoScale * randomScale;
        
    }



    }

}
