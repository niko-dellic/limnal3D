using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class TMPScreenShotPath : MonoBehaviour
{

    public TextMeshProUGUI textMesh;

    string mPath;


    // Start is called before the first frame update
    void Start()
    {
        mPath = Application.dataPath;
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.text = "Screenshot Saved to" + mPath;
        Debug.Log(mPath);        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
