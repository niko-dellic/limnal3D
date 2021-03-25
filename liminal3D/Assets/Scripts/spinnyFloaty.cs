using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinnyFloaty : MonoBehaviour
{
    // Start is called before the first frame update

    public bool spinny = false;
    public bool floaty = false;
    //public float speed = 3f;

    // User Inputs
    public float degreesPerSecond = 15.0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;

    Vector3 posOffset = new Vector3 ();
    Vector3 tempPos = new Vector3 ();


    void Start()
    {
        posOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (spinny)
        {
            // Spin object around Y-Axis
            transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);
        }

        if (floaty)
        {
            // Float up/down with a Sin()
            tempPos = posOffset;
            tempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * frequency) * amplitude;
            transform.position = tempPos;
        }

        // if (spinny)
        // {
        //     transform.Rotate(0,1*speed,0, Space.World);
        // }

        // var direction = 1f;

        // if (floaty)
        // {
        //     transform.localPosition = new Vector3(0,1*direction,0);
        // }
    }
}
