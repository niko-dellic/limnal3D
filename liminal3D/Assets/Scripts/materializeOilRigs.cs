using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class materializeOilRigs : MonoBehaviour
{


    public Material rigMaterial;

    void Start()
    {
        Renderer[] allChildren = this.transform.GetComponentsInChildren<Renderer>(true);
        for (int i = 0; i < allChildren.Length; i++)
        {
            // Debug.Log(allChildren[i].name);  
            allChildren[i].material = rigMaterial;

        }   
    }

}
