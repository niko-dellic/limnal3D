using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class miniMap : MonoBehaviour
{


    public float heightOffset = 5f;

    public float sectionHeightOffset = 10f;

    private GameObject player;
    public GameObject postPro;
    public GameObject miniMapUI;

    public GameObject sectionOverlay;
    private Vector3 newLocation;

    private Vector3 sectionNewLocation;

    private bool onPlayer = false;
    
    private void lerpAlpha(float startVal, float targetVal)
    {
            float textureAlpha = Mathf.Lerp(startVal,targetVal,postPro.GetComponent<enableAR>().Counter);
            miniMapUI.GetComponent<RawImage>().color = new Color (1,1,1,textureAlpha);

            if (sectionOverlay != null)
            {
                sectionOverlay.GetComponent<RawImage>().color = new Color (1,1,1,textureAlpha);
            }

            if (textureAlpha < 0.1f)
            {
                miniMapUI.SetActive(false);

                if (sectionOverlay != null)
                {
                    sectionOverlay.SetActive(false);

                }
            }

            if (textureAlpha > 0.1f)
            {
                miniMapUI.SetActive(true);

                if (sectionOverlay != null)
                {
                    sectionOverlay.SetActive(true);
                    
                }
            }
    }

    private void Start() {
        newLocation = new Vector3(0,heightOffset,0); 
        sectionNewLocation = new Vector3(0,sectionHeightOffset,0); 
    }

    private void LateUpdate()
    {

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("PLAYER_CLONE");
        }

        if (player != null)
        {
            if (player != null)
            {
                transform.parent = player.transform;
                
                if (onPlayer == false)
                {
                    if (this.gameObject.tag == "SECTION3D")
                    {
                        transform.localPosition = sectionNewLocation;
                        transform.rotation = player.transform.rotation;
                    }

                    else
                    {
                        transform.localPosition = newLocation;
                    }

                    onPlayer = true;
                }
            }
            

            if (postPro.GetComponent<enableAR>().ARToggle == true)
            {
                if (postPro.GetComponent<enableAR>().lerp == true)
                {
                    lerpAlpha(0f,1f);
                }

                if (miniMapUI.activeSelf == false)
                {

                    
                }
                

            }

            if (postPro.GetComponent<enableAR>().ARToggle == false)
            {
                
                if (postPro.GetComponent<enableAR>().lerp == true)
                {
                    lerpAlpha(1f,0f);
                }
                if (miniMapUI.activeSelf == true)
                {

                }
                

            }
        }
        
    }

}
