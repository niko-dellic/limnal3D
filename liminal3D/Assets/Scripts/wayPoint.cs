using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class wayPoint : MonoBehaviour
{

    [Header("Inputs")]

    [SerializeField] public GameObject menu1;
    [SerializeField] public GameObject menu2;

    [SerializeField] private bool disablePointer = false;

    // [SerializeField] public GameObject rayObject;
    
    // [SerializeField] LineRenderer lineRend;
    [SerializeField] public GameObject pointerReticle;
    [SerializeField] public GameObject marker;
    [SerializeField] public float lifespan = 3f;

    [SerializeField] public LayerMask IgnoreMe;

    [SerializeField] public Transform instantiatePoint;

    private Vector3 target;

    [Header("Remap")]
    
    [SerializeField] [Range(1f,5f)] public float remapFactor = 3f;
    [SerializeField] [Range(0.0f,0.3f)] public float smallScale= 0.1f;

    [SerializeField] [Range(1f,10f)] public float largeScale = 5f;

    private float gumballScale;

    [Header("Adjustments")]
    private Vector3 markerDistanceScale; 
    private bool activatePointer = false;
    Vector3 pos = new Vector3(0.5f,0.5f,0);
    Ray ray;

    private void Update()
    {

        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            var Vray = Camera.main.ViewportPointToRay(pos);
            RaycastHit hit;
            ray = Camera.main.ViewportPointToRay(pos);
        

            if (Physics.Raycast(Vray, out hit))
            {
                float gumballScale = Mathf.Clamp((Mathf.Log(hit.distance)/remapFactor), smallScale, largeScale);
                markerDistanceScale = new Vector3 (gumballScale, gumballScale, gumballScale);
                marker.transform.localScale = markerDistanceScale;

            }
        

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {

                if (menu1.activeSelf || menu2.activeSelf)
                {
                    disablePointer = true;
                    //Debug.Log("it works!");
                }
                else
                {
                    disablePointer = false;
                }

                if (!disablePointer)
                {
                    activatePointer = true;
                    Rigidbody rigidbody = marker.GetComponent<Rigidbody>();
                    rigidbody.useGravity = false;
                    SphereCollider sphereCollider = marker.GetComponent<SphereCollider>();
                    sphereCollider.enabled = false;
                }

            }

            if (!disablePointer && activatePointer && Physics.Raycast(ray, out hit, 100, ~IgnoreMe))
            {
                marker.transform.position = hit.point;
                pointerReticle.SetActive(true);
                marker.SetActive(true);
                instantiatePoint.transform.position = hit.point;
                //Debug.Log(instantiatePoint);      
            }
        }

        if (!disablePointer && Input.GetKeyUp(KeyCode.Mouse0))
        {
            activatePointer = false;
            //Debug.Log(activatePointer);
            var rollypolly = Instantiate (marker, instantiatePoint.position, instantiatePoint.rotation); 
            
            SphereCollider sphereCollider = rollypolly.GetComponent<SphereCollider>();
            sphereCollider.enabled = true;
           
            Destroy(rollypolly, lifespan);
        }

        if (!activatePointer)
        {
            pointerReticle.SetActive(false);
            marker.SetActive(false);    
        }

    }

}
