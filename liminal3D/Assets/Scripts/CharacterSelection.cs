using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    
    [SerializeField] public GameObject Player1;
    [SerializeField] public GameObject Player2;
    
    [SerializeField] public bool characterSwap = false;
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            characterSwap = !characterSwap;
        }

        if(Input.GetKeyDown(KeyCode.T) && characterSwap == true)
        {
            Debug.Log("if trigger");
            GameObject[] mainPlayer = GameObject.FindGameObjectsWithTag ("Player");
            foreach(GameObject go in mainPlayer)
            {
                go.SetActive (false);
            }

            GameObject[] secondPlayer = GameObject.FindGameObjectsWithTag ("Player2");
            Debug.Log(secondPlayer.Length);
            foreach(GameObject go in secondPlayer)
            {
                go.SetActive (true);
            }
        }

        else if(Input.GetKeyDown(KeyCode.T) && characterSwap == false)
        {
            Debug.Log("else trigger");
            GameObject[] mainPlayer = GameObject.FindGameObjectsWithTag ("Player");
            foreach(GameObject go in mainPlayer)
            {
                go.SetActive (true);
            }

            GameObject[] secondPlayer = GameObject.FindGameObjectsWithTag ("Player2");
            Debug.Log(secondPlayer.Length);
            foreach(GameObject go in secondPlayer)
            {
                go.SetActive (false);
            }

        }
    }

}
