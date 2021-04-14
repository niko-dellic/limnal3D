using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class fontFlash : MonoBehaviour
{

    void Update()
    {
         this.gameObject.GetComponent<TextMeshProUGUI>().color = new Color(1,0.7817019f,0,Mathf.Abs(Mathf.Sin(Time.time)));
    }
}
