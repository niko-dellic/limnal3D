using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class playerControllerFPS : NetworkBehaviour
{
    [Header ("Player Speed")]
    [SerializeField] float PlayerSpeed;
    [SerializeField] float PlayerSpeedMax;

    [Header ("Sensitivity")]
    [SerializeField] float sensitivityX = 1.5f;
    [SerializeField] float sensitivityY = 1.5f;

    [Header ("Camera")]
    [SerializeField] Vector3 cameraOffset;

    [Header ("Debug")]
    [SerializeField] bool DebugMode = false;

    public float mouseSensitivity = 100f;
    public Transform playerBody;

    // float xRotation = 0f;
    Vector3 velocity;
    public float jumpHeight = 3f;
    public float gravity = -9.81f;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        if (isLocalPlayer)
        {
            //its local player

            //Setup FPS Cam
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = cameraOffset;
            Camera.main.transform.rotation = Quaternion.identity;
            canvasManager.instance.ChangePlayerState(true);

        }
        else
        {
        //its not local player
        rb.isKinematic = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(!isLocalPlayer)
            return;

        if (DebugMode)
        {
            Camera.main.transform.localPosition = cameraOffset;
        }

        if (Application.isMobilePlatform)
        {
            //Do mobile controlls.
            MobileInput();
        }
        else
        {
            //PC Input
            //MobileInput(); //enable for mobile controls on PC 
            PCInput();

            // Cursor.lockState = CursorLockMode.Locked; enable once rotation has been established for PC
        }
    }

    private void PCInput()
    {
        //Translate
        if (Input.GetAxis("Horizontal") != Mathf.Epsilon || Input.GetAxis("Vertical") != Mathf.Epsilon)
        {
            Vector3 movementDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); 
            movementDirection *= PlayerSpeed;
            movementDirection = Vector3.ClampMagnitude(movementDirection, PlayerSpeed);    
            if(rb.velocity.magnitude < PlayerSpeedMax)
                rb.AddRelativeForce(movementDirection*Time.deltaTime *100);
        }

        //Rotate
        float rotX = -Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        float rotY = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        //Body rotation
            transform.Rotate(0,rotY, 0);

        //Camera rotation
            Camera.main.transform.Rotate(rotX, 0, 0);

        if(Input.GetButtonDown("Jump") )
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        
    }


    private void MobileInput()
    {   
        //Translation
        if (canvasManager.instance.leftJoystick.Horizontal != Mathf.Epsilon || canvasManager.instance.leftJoystick.Vertical != Mathf.Epsilon)
        {
            Vector3 movementDirection = new Vector3(canvasManager.instance.leftJoystick.Horizontal, 0, canvasManager.instance.leftJoystick.Vertical); 
            movementDirection *= PlayerSpeed;
            movementDirection = Vector3.ClampMagnitude(movementDirection, PlayerSpeed);
            if(rb.velocity.magnitude < PlayerSpeedMax)
            rb.AddRelativeForce(movementDirection*Time.deltaTime *100);
        }

        //Rotation
        if (canvasManager.instance.rightJoystick.Horizontal != Mathf.Epsilon || canvasManager.instance.rightJoystick.Vertical != Mathf.Epsilon)
        {
            float rotY = canvasManager.instance.rightJoystick.Horizontal * sensitivityY;
            float rotX = -canvasManager.instance.rightJoystick.Vertical * sensitivityX;

            //Body rotation
            transform.Rotate(0,rotY, 0);

            //Camera rotation
            Camera.main.transform.Rotate(rotX, 0, 0);
        }
    }

}




// public class playerControllerFPS : NetworkBehaviour
// {
//     [Header ("Player Speed")]
//     [SerializeField] float PlayerSpeed;
//     [SerializeField] float PlayerSpeedMax;

//     [Header ("Sensitivity")]
//     [SerializeField] float sensitivityX = 1.5f;
//     [SerializeField] float sensitivityY = 1.5f;

//     [Header ("Camera")]
//     [SerializeField] Vector3 cameraOffset;

//     [Header ("Debug")]
//     [SerializeField] bool DebugMode = false;

//     Rigidbody rb;

//     // Start is called before the first frame update
//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
        
//         if (isLocalPlayer)
//         {
//             //its local player

//             //Setup FPS Cam
//             Camera.main.transform.SetParent(transform);
//             Camera.main.transform.localPosition = cameraOffset;
//             Camera.main.transform.rotation = Quaternion.identity;
//             canvasManager.instance.ChangePlayerState(true);

//         }
//         else
//         {
//         //its not local player
//         rb.isKinematic = true;
//         }
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if(!isLocalPlayer)
//             return;

//         if (DebugMode)
//         {
//             Camera.main.transform.localPosition = cameraOffset;
//         }

//         if (Application.isMobilePlatform)
//         {
//             //Do mobile controlls.
//             MobileInput();
//         }
//         else
//         {
//             //PC Input
//             MobileInput(); //enable for mobile controls on PC 
//             PCInput();
            
//             // Cursor.lockState = CursorLockMode.Locked; enable once rotation has been established for PC
//         }
//     }

//     private void PCInput()
//     {
//         if (Input.GetAxis("Horizontal") != Mathf.Epsilon || Input.GetAxis("Vertical") != Mathf.Epsilon)
//         {
//             Vector3 movementDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); 
//             movementDirection *= PlayerSpeed;
//             movementDirection = Vector3.ClampMagnitude(movementDirection, PlayerSpeed);    
//             if(rb.velocity.magnitude < PlayerSpeedMax)
//                 rb.AddRelativeForce(movementDirection*Time.deltaTime *100);
//         }
//     }


//     private void MobileInput()
//     {   
//         //Translation
//         if (canvasManager.instance.leftJoystick.Horizontal != Mathf.Epsilon || canvasManager.instance.leftJoystick.Vertical != Mathf.Epsilon)
//         {
//             Vector3 movementDirection = new Vector3(canvasManager.instance.leftJoystick.Horizontal, 0, canvasManager.instance.leftJoystick.Vertical); 
//             movementDirection *= PlayerSpeed;
//             movementDirection = Vector3.ClampMagnitude(movementDirection, PlayerSpeed);
//             if(rb.velocity.magnitude < PlayerSpeedMax)
//             rb.AddRelativeForce(movementDirection*Time.deltaTime *100);
//         }

//         //Rotation
//         if (canvasManager.instance.rightJoystick.Horizontal != Mathf.Epsilon || canvasManager.instance.rightJoystick.Vertical != Mathf.Epsilon)
//         {
//             float rotY = canvasManager.instance.rightJoystick.Horizontal * sensitivityY;
//             float rotX = -canvasManager.instance.rightJoystick.Vertical * sensitivityX;

//             //Body rotation
//             transform.Rotate(0,rotY, 0);

//             //Camera rotation
//             Camera.main.transform.Rotate(rotX, 0, 0);
//         }
//     }

// }

