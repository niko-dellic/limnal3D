using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class captureScreenNiko : MonoBehaviour
{

    public GameObject multiplayerMenu;
    private int count = 0;

    private float timer = 0.0f;

    public float visibleTime = 0.5f;
    public string filename = "screenshot";

    public string directory = "Screenshots"; //"C:/Users/nikod/Desktop/Screenshots";

    string mPath;

    public GameObject screenCaptureNotification; 

    private TextMeshProUGUI textMesh;


    

    public void Start()
    {
        textMesh = screenCaptureNotification.GetComponent<TextMeshProUGUI>();
        
        // textMesh = GetComponent<TextMeshProUGUI>();

        

        mPath = Application.dataPath;

        textMesh.text = mPath + "/" + directory + "/" + filename;

        //Debug.Log("dataPath : " + mPath);

    }
    void click()
    {

        //Debug.Log(timer);
        timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.P) && multiplayerMenu.activeSelf == false)
        {

            var dateTime = System.DateTime.Now;

            var strdateTime = (dateTime.ToString("yyyyMMddTHH.mm.ssZ"));

            Debug.Log(strdateTime);

            if (!Directory.Exists(mPath + "/" + directory))
            {
                 Directory.CreateDirectory(mPath + "/" + directory);
                 Debug.Log("Created Directory");
            }            
        
            count = count + 1;
            Debug.Log(count);
            ScreenCapture.CaptureScreenshot(mPath + "/" + directory + "/" + filename + "-" + count + strdateTime + ".png");

        }

        if (Input.GetKeyUp(KeyCode.P) && multiplayerMenu.activeSelf == false)
        {
            screenCaptureNotification.SetActive(true);
            timer = 0;
            
        }

        if (timer > visibleTime)
        {
            screenCaptureNotification.SetActive(false);
        }

    }


    private void Update()
    {
        
        click();


    }

}
