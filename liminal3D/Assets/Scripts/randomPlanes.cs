using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class randomPlanes : MonoBehaviour
{
    
    public List<UnityEngine.Video.VideoClip> myVideos;
    
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
    
    private float randomScale;

    private Vector3 aspectRatio = new Vector3(1.7f,1,1); 

    private Vector3 localxyz;

    private Vector3 centroid;

    
   
    void Start()
    {
        localxyz = this.transform.position;
        centroid = GetComponent<Collider>().bounds.center;        
    }

    private void OnEnable()
    {

        foreach (UnityEngine.Video.VideoClip i in myVideos)
        {
            //CREATE TEXTURE
            var rt = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
            rt.name = i.name;
            rt.width = 1920;
            rt.height = 1080;
            
            //create the video players
            var videoShell = gameObject.AddComponent<UnityEngine.Video.VideoPlayer>();
            videoShell.isLooping = true;
            videoShell.SetDirectAudioMute(0, true);
            videoShell.EnableAudioTrack(0, false);
            videoShell.renderMode = UnityEngine.Video.VideoRenderMode.RenderTexture;
            videoShell.targetTexture = rt;
            videoShell.clip = i;

            //set local variables and create default plane
            speed = Random.Range(speedMin,speedMax);
            randomScale = Random.Range(minScale,maxScale);   
            Vector3 planeVector1 = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
            GameObject videoPlanes = GameObject.CreatePrimitive(PrimitiveType.Plane);

            //Initial transform + Rotation
            Vector3 randomPlacement = new Vector3(Random.Range(-randomDisplacement,randomDisplacement), Random.Range(-randomDisplacement,randomDisplacement), Random.Range(-randomDisplacement,randomDisplacement));
            videoPlanes.transform.parent = this.gameObject.transform;
            videoPlanes.transform.localPosition = centroid + randomPlacement;
            videoPlanes.transform.rotation = Random.rotation;

            //Initial Scale
            Vector3 defaultScale = videoPlanes.transform.localScale;
            Vector3 videoScale = Vector3.Scale(defaultScale, aspectRatio);
            videoPlanes.transform.localScale = videoScale * randomScale;

            //physics velocity and direction and rotation
            videoPlanes.GetComponent<MeshCollider>().convex = true;
            videoPlanes.GetComponent<MeshCollider>().isTrigger = true;
            videoPlanes.AddComponent<Rigidbody>().useGravity = false;
            videoPlanes.GetComponent<Rigidbody>().velocity = planeVector1*speed;
            videoPlanes.GetComponent<Rigidbody>().angularVelocity = ((planeVector1*speed)/slowAngularRotation);

            //ASSIGN MATERIALS
            Renderer vidMats = videoPlanes.GetComponent<Renderer>();
            vidMats.material = new Material(shader);    //Shader.Find("Universal Render Pipeline/Lit")
            vidMats.material.mainTexture = rt;
            vidMats.material.EnableKeyword("_EMISSION");
            vidMats.material.SetTexture("_EmissionMap", rt);
            vidMats.material.SetColor("_EmissionColor", Color.white);

        }

    }

}
