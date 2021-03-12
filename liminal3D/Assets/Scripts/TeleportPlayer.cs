using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportPlayer : MonoBehaviour
{

    public GameObject localReference;

    private Transform teleportTarget; //variable for the teleportation position

    public GameObject teleportObject;

    public bool inheritRotation = true;

    private Scene currentScene;
    private string sceneName;

    private void Start()
    {

        //SCENE MANAGEMENT
        
        // Create a temporary reference to the current scene.
        currentScene = SceneManager.GetActiveScene ();
 
        // Retrieve the name of this scene.
        sceneName = currentScene.name;

        if (sceneName == "TESTING") 
        {
            teleportTarget = teleportObject.transform;
            teleportTarget.GetComponent<MeshRenderer>().enabled = false;  
        }  
    }


    
    void OnTriggerEnter(Collider other)
    {


        if (other.tag == "PLAYER_CLONE" || other.tag == "HOST")
        {

            if (localReference != null)
            {
                localReference.GetComponent<MeshRenderer>().enabled = false;
                Vector3 displacement = other.transform.position - localReference.transform.position;
                other.transform.position = teleportTarget.transform.position + displacement;
            }

            else
            {
                other.transform.position = teleportTarget.transform.position;
            }
            

            if (inheritRotation)
            {
                other.transform.rotation = teleportTarget.transform.rotation;  
            }



        }
    }

    private void Update() {
    
        if (sceneName == "TESTING") 
        {   
            if (!teleportObject.gameObject.activeSelf)
            {
                Debug.Log("not active");
            } 
        }

    }

}
