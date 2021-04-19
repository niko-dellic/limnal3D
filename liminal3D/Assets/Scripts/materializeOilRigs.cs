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
            if (allChildren[i].GetComponent<MeshRenderer>() != null && allChildren[i].gameObject.layer != LayerMask.NameToLayer("PROTECTMATERIAL"))
            {
                allChildren[i].material = rigMaterial;
            }
        }   
    }

}
