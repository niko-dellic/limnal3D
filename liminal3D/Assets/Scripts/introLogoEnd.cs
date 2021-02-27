using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class introLogoEnd : MonoBehaviour
{
    
    public UnityEngine.Video.VideoPlayer logoVideo;

    void Start()
    {
        logoVideo.loopPointReached += EndReached;
        
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
    vp.playbackSpeed = vp.playbackSpeed / 10.0F;
    Debug.Log("videoDone");
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Update is called once per frame
    void Update()
    {

        
    }
}
