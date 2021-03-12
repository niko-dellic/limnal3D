using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floodLight : MonoBehaviour
{
    public float rotationSpeed = 3f;

    [Header("Fixed Rotation Settings")]
    public bool limitRot = false;
    public float maxRot = 80;
    public float delta = 1.5f;

    private Vector3 randVector;
    private Vector3 randomStartRotation;
    private Quaternion startPos;
    private Quaternion defaultRot;

    

    // public float randomDir = Random.Range(0.2f, 2f);

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.rotation;

        if (!limitRot)
        {
            //random start location
            var rand360 = Random.Range(-360f,360);
            randomStartRotation = new Vector3(rand360,rand360,rand360);
            transform.Rotate(randomStartRotation);
            //declare random vector
            randVector =  new Vector3(Random.Range(-2f, 2f),Random.Range(-2f, 2f),0);
        }

        if(limitRot)
        {
            defaultRot = transform.rotation;
            transform.Rotate(defaultRot.z,-maxRot,0, Space.World);   
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (limitRot)
        {
            float undulation = delta * Mathf.Sin(Time.time * rotationSpeed);
            Vector3 rot = new Vector3 (defaultRot.x,(undulation*maxRot),defaultRot.z); //fix this
            transform.Rotate(rot *(rotationSpeed*Time.deltaTime), Space.World);
        }
        else
        {
            transform.Rotate(randVector *(rotationSpeed*Time.deltaTime), Space.World);
        }



    }
}
