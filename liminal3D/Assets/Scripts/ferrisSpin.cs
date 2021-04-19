using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ferrisSpin : MonoBehaviour
{
    // Start is called before the first frame update
    
    public List<GameObject> Carraiges = new List<GameObject>();
    public float degreesPerSecond = 15.0f;

    
    void Start()
    {
        GameObject[] carriageArray = GameObject.FindGameObjectsWithTag("Carriage");
        
        for (int i = 0; i < carriageArray.Length; i++)
        {
            Carraiges.Add(carriageArray[i]);   
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, Time.deltaTime * degreesPerSecond), Space.World);  

        for (int i = 0; i < Carraiges.Count; i++)
        {
            Carraiges[i].transform.Rotate(new Vector3(0f, 0f, -Time.deltaTime * degreesPerSecond), Space.World); 
        }    
    }
}
