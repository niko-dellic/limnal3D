using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerMovement : NetworkBehaviour //MonoBehaviour
{

    [Header("Host Settings")]
    
    // [SyncVar]
    private bool host;
    private float hostTime = 0f;

    // [SyncVar]
    public Material hostMaterial;

    // [SyncVar]
    private Material defaultMaterial;

    [Header("TELEPORTATION")]
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 matrix;
    [SerializeField] private Vector3 station;
    [SerializeField] private Vector3 studiolo;
    [SerializeField] private Vector3 lounge;


    [Header ("Viewport")]
    [SerializeField] Vector3 cameraOffset;
    public float mouseSensitivity = 50f;
    [SerializeField] float maxCameraX = 65f;
    [SerializeField] float minCameraX = -65f;
    float xRotation = 0f;
    private bool onoff = false;

    [Header ("Camera Projections - Perspective")]
    [SerializeField] public float minFov = 15f;
    [SerializeField] public float maxFov = 90f;
    [SerializeField] public float perspectiveSensitivity = 20f;
    [SerializeField] public float defaultFov = 60;
    private float fov;


    [Header ("Camera Projections - Ortho")]
    [SerializeField] public float minOrtho = 1f;
    [SerializeField] public float maxOrtho = 25f;
    [SerializeField] public float orthoSensitivity = 10f;
    [SerializeField] public float defaultOrthoSize = 5;
    [SerializeField] Vector3 orthoCameraOffset;
    private float orthoSize;
  
    [Header ("Movement Settings")]
    public CharacterController controller;  

    public float speed = 12f;

    public float gravity;
    private float defaultGravity;

    private float defaultSpeed;

    [Header("Run Settings")]
    public float runFactor = 2f;
    private bool runMode;
    private float runModeMove;
    private float runModeJump;

    [Header("Walk Settings")]
    public float walkFactor = 0.5f;
    private bool walkMode = false;
    private float walkModeMove;
    private float walkModeJump;

    private float walkModeGravity;

    [Header("Jump Settings")]
    public float jumpHeight;
    public float superJumpFactor = 10f;
    private float defaultJump;


    [Header("Grounding")]

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    // [Header("ADMIN")]
    // public GameObject[] players;



    void Start()
    {
        if (isLocalPlayer)
        {

            //GET DEFAULT TEXTURE
            defaultMaterial = transform.Find("MAIN CHARACTER").GetComponent<Renderer>().material;

            //SET CAMERA
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = cameraOffset;
            Camera.main.transform.rotation = Quaternion.identity;

            //Hide Mouse
            Cursor.lockState = CursorLockMode.Locked;

            //Change Tag
            transform.gameObject.tag = "PLAYER_CLONE";

            //Set Default Jump
            defaultJump = jumpHeight;

            //Set Default Speed
            defaultSpeed = speed;

            //Set Default Gravity
            defaultGravity = gravity;

            //Set Run Speeds
            runModeMove = speed*runFactor;
            runModeJump = jumpHeight*runFactor;

            //Set Walk Speeds
            walkModeJump = jumpHeight*walkFactor;
            walkModeMove = speed*walkFactor;
            walkModeGravity = gravity/walkFactor;

            //TELEPORTING!!

            //PLAYER ORIGIN
            startPosition = this.transform.position;
            //Debug.Log(startPosition);

            //MATRIX
            matrix = GameObject.FindGameObjectWithTag("MATRIX_TELEPORT").transform.position;
            var matrixObj = GameObject.FindGameObjectWithTag("MATRIX_TELEPORT").GetComponent<MeshRenderer>().enabled = false;    

            //STATION         
            station = GameObject.FindGameObjectWithTag("STATION_TELEPORT").transform.position;
            var stationObj = GameObject.FindGameObjectWithTag("STATION_TELEPORT").GetComponent<MeshRenderer>().enabled = false;               

            //STUDIOLO
            studiolo = GameObject.FindGameObjectWithTag("STUDIOLO_TELEPORT").transform.position;
            var studioloObj = GameObject.FindGameObjectWithTag("STUDIOLO_TELEPORT").GetComponent<MeshRenderer>().enabled = false;   
            
            //LOUNGE
            lounge = GameObject.FindGameObjectWithTag("LOUNGE").transform.position;
            




        }
    }

    public void AdjustSpeed(float newSpeed)
    {
        mouseSensitivity = newSpeed;       
    }
     
    public void HandleMovement()
    {
        
        if (isLocalPlayer)
        {

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
                       
            if(isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
    
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);

            if(Input.GetButtonDown("Jump") )
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            //RUN MODE

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                runMode = !runMode;
            }

            if (runMode == true)
            {
                speed = runModeMove;
                jumpHeight = runModeJump*superJumpFactor;
                walkMode = false;
            }

            //WALK MODE

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                walkMode = !walkMode;
            }

            if (walkMode == true)
            {
                speed = walkModeMove;
                jumpHeight = walkModeJump;
                gravity = walkModeGravity;
                runMode = false;
            }

            if (walkMode == false && runMode == false)
            {
                speed = defaultSpeed;
                jumpHeight = defaultJump;
                gravity = defaultGravity;
            }


            //CHARACTER ROTATION
            float rotX = -Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            float rotY = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            

            //Body rotation
            transform.Rotate(0,rotY, 0);

            //Camera rotation
            xRotation = Mathf.Clamp(xRotation + rotX, minCameraX, maxCameraX);
            Camera.main.transform.localEulerAngles = new Vector3(xRotation,0,0);

            //ORTHO CAMERA TOGGLE

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                onoff = !onoff;
                Camera.main.orthographic = true;
                Camera.main.transform.localPosition = orthoCameraOffset;
            }  

            // PERSPECTIVE CAMERA TOGGLE

            if (Input.GetKeyDown(KeyCode.Tab) && onoff == false)
            {
    
                Camera.main.orthographic = false;
                Camera.main.nearClipPlane = 0.1f;
                Camera.main.transform.localPosition = cameraOffset;
            }  

            // PERSPECTIVE SCROLLING
            if (Camera.main.orthographic == false)
            {

                if (Input.GetAxis("Mouse ScrollWheel") > 0f ) // forward 
                { 
                    //Debug.Log("UP!");
                    fov = Camera.main.fieldOfView;
                    fov += Input.GetAxis("Mouse ScrollWheel") * perspectiveSensitivity;
                    fov = Mathf.Clamp(fov, minFov, maxFov);
                    Camera.main.fieldOfView = fov;
                } 

                else if (Input.GetAxis("Mouse ScrollWheel") < 0f ) // backwards 
                { 
                    //Debug.Log("DOWN!");
                    fov = Camera.main.fieldOfView;
                    fov += Input.GetAxis("Mouse ScrollWheel") * perspectiveSensitivity;
                    fov = Mathf.Clamp(fov, minFov, maxFov);
                    Camera.main.fieldOfView = fov;
                }
            }

            // ORTHO SCROLLING
            if (Camera.main.orthographic == true)
            {

                if (Input.GetAxis("Mouse ScrollWheel") > 0f ) // forward 
                { 
                    orthoSize = Camera.main.orthographicSize;
                    orthoSize += Input.GetAxis("Mouse ScrollWheel") * orthoSensitivity;
                    orthoSize = Mathf.Clamp(orthoSize, minOrtho, maxOrtho);
                    Camera.main.orthographicSize = orthoSize;
                } 

                else if (Input.GetAxis("Mouse ScrollWheel") < 0f ) // backwards 
                { 
                    orthoSize = Camera.main.orthographicSize;
                    orthoSize += Input.GetAxis("Mouse ScrollWheel") * orthoSensitivity;
                    orthoSize = Mathf.Clamp(orthoSize, minOrtho, maxOrtho);
                    Camera.main.orthographicSize = orthoSize;
                }
            }

            // RESET VIEWS   
            if (Input.GetKeyDown(KeyCode.Mouse2))
            {
                Camera.main.orthographicSize = defaultOrthoSize;
                Camera.main.fieldOfView = defaultFov;
            }
            
            // RESPAWN CHARACTER
            if (Input.GetKeyDown("1"))
            {               
                this.transform.position = startPosition;
            }

            // GO TO THE MATRIX
            if (Input.GetKeyDown("2"))
            {
                this.transform.position = matrix;
            }

            // GO TO THE STATION
            if (Input.GetKeyDown("3"))
            {
                this.transform.position = station;
            }

            // GO TO THE STUDY
            if (Input.GetKeyDown("4"))
            {
                this.transform.position = studiolo;
            }

            // GO TO THE LOUNGE
            if (Input.GetKeyDown("5"))
            {
                this.transform.position = lounge;
            }



            // BECOME HOST
            
            if (Input.GetKeyDown(KeyCode.H))
            {
                hostTime = 1 + hostTime;
            }

            if (hostTime > 5f)
            {
                host = true;
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                host = false;
            }

            if (host)
            {
                this.gameObject.tag = "HOST";
                transform.Find("MAIN CHARACTER").GetComponent<Renderer>().material  = hostMaterial;

            }

            if (!host)
            {
                this.gameObject.tag = "PLAYER_CLONE";
                transform.Find("MAIN CHARACTER").GetComponent<Renderer>().material  = defaultMaterial;
            }


        }
    }


    void Update()
    {
        HandleMovement();

    }

}



