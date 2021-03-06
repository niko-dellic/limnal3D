using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerMovement : NetworkBehaviour //MonoBehaviour
{
    
    //RAYCASTING
    [Header("PROJECTILE (RAYCAST AND GUMBALL PREFAB)")]
    // [SerializeField] public LayerMask IgnoreMe;
    [SerializeField] [Range(1f,5f)] public float remapFactor = 3f;
    [SerializeField] [Range(0.0f,0.3f)] public float smallScale= 0.1f;
    [SerializeField] [Range(1f,10f)] public float largeScale = 5f;
    // private bool disablePointer = false;
    // private bool activatePointer = false;
    private Vector3 markerDistanceScale; 
    Vector3 pos = new Vector3(0.5f,0.5f,0);
    Ray ray;
    
    //TANK STUFF

     public GameObject projectilePrefab;
    public Transform projectileMount;
    private Vector3 rayPosition;


    //TANK STUFF END



    [Header("Host Settings")]
    
    // [SyncVar]
    private bool host;
    private float hostTime = 0f;

    //[SyncVar]
    public Material hostMaterial;

    private Material defaultMaterial;

    [Header("TELEPORTATION")]
    private float teleportID;
    public float teleportBuffer;

    private float teleportTimer;

    private Vector3 teleportZone;
    private Vector3 startPosition;
    private Vector3 myRoom;
    private Vector3 matrix;
    private Vector3 station;
    private Vector3 studiolo;
    private Vector3 lounge;


    [Header ("Viewport")]
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] public float mouseSensitivity = 50f;
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



    void Start()
    {
        defaultMaterial = transform.Find("MAIN CHARACTER").GetComponent<Renderer>().material;

        if (isLocalPlayer)
        {

            //GET DEFAULT TEXTURE
            

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

            //TELEPORT ZONE
            teleportZone = GameObject.FindGameObjectWithTag("TELEPORT_ZONE").transform.position;

            //PLAYER ORIGIN
            startPosition = this.transform.position;
            
            //MyRoom
            myRoom = GameObject.FindGameObjectWithTag("MYROOM_TELEPORT").transform.position;
            var myRoomObj = GameObject.FindGameObjectWithTag("MYROOM_TELEPORT").GetComponent<MeshRenderer>().enabled = false;    

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
                teleportID = 1;
                teleportTimer = Time.time;

                this.transform.position = teleportZone;

            }

            // GO TO MY ROOM
            if (Input.GetKeyDown("2"))
            {   
                teleportID = 2;
                teleportTimer = Time.time;
                
                this.transform.position = teleportZone;
            }

            // GO TO THE MATRIX
            if (Input.GetKeyDown("3"))
            {   
                teleportID = 3;
                teleportTimer = Time.time;
                
                this.transform.position = teleportZone;
            }

            // GO TO THE STATION
            if (Input.GetKeyDown("4"))
            {  
                teleportID = 4; 
                teleportTimer = Time.time;

                this.transform.position = teleportZone;
            }

            // GO TO THE STUDY
            if (Input.GetKeyDown("5"))
            {   
                teleportID = 5;
                teleportTimer = Time.time;
                
                this.transform.position = teleportZone;
            }

            // GO TO THE Lounge
            if (Input.GetKeyDown("6"))
            {   
                teleportID = 6;
                teleportTimer = Time.time;
                this.transform.position = teleportZone;
            }




        }
    }

    //BECOME HOST

    [Command]
    void CmdBecomeHost()
    {
        if (isLocalPlayer)
        {
            if (host)
            {
                this.gameObject.tag = "HOST";
                transform.Find("MAIN CHARACTER").GetComponent<Renderer>().material = hostMaterial;
            }      
        }
    }

    // [ClientRpc]
    // void RpcShowHost()
    // {

    // }


    //TANK STUFF HERE
        [Command]
        void CmdFire()
        {
            GameObject projectile = Instantiate(projectilePrefab, projectileMount.position, transform.rotation); 
            // GameObject projectile = Instantiate(projectilePrefab, rayPosition, transform.rotation);
            NetworkServer.Spawn(projectile);
            // RpcOnFire();
        }

        
        // this is called on the server

        // this is called on the tank that fired for all observers
        [ClientRpc]
        void RpcOnFire()
        {
            GameObject projectile = Instantiate(projectilePrefab, projectileMount.position, transform.rotation);
            // GameObject projectile = Instantiate(projectilePrefab, rayPosition, transform.rotation); 
            NetworkServer.Spawn(projectile);
        }


    //TANK END


    void Update()
    {
        HandleMovement();
        
        //TANK STUFF

        if (!isLocalPlayer) return;

        //RAYCAST
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CmdFire();

            var Vray = Camera.main.ViewportPointToRay(pos);
            RaycastHit hit;
            ray = Camera.main.ViewportPointToRay(pos);

            if (Physics.Raycast(Vray, out hit))
            {
                float gumballScale = Mathf.Clamp((Mathf.Log(hit.distance)/remapFactor), smallScale, largeScale);
                markerDistanceScale = new Vector3 (gumballScale, gumballScale, gumballScale);
                //projectilePrefab.transform.localScale = markerDistanceScale;
                rayPosition = hit.point;
            }
                
           
        }
        
        //TANK STUFF + RAYCAST END
       
        

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
            hostTime = 0;
            transform.Find("MAIN CHARACTER").GetComponent<Renderer>().material  = defaultMaterial;
            this.gameObject.tag = "PLAYER_CLONE";
        }
        
        if (host)
        {
            CmdBecomeHost();
        }


        //TELEPORTING
        if(teleportID == 1 && teleportTimer + teleportBuffer < Time.time)
        {
            teleportID = 0;
            this.transform.position = startPosition;
        }

        if(teleportID == 2 && teleportTimer + teleportBuffer < Time.time)
        {
            teleportID = 0;
            this.transform.position = myRoom;
        }

        if(teleportID == 3 && teleportTimer + teleportBuffer < Time.time)
        {
            teleportID = 0;
            this.transform.position = matrix;
        }

        if(teleportID == 4 && teleportTimer + teleportBuffer < Time.time)
        {
            teleportID = 0;
            this.transform.position = station;
        }

        if(teleportID == 5 && teleportTimer + teleportBuffer < Time.time)
        {
            teleportID = 0;
            this.transform.position = studiolo;
        }

        if(teleportID == 6 && teleportTimer + teleportBuffer < Time.time)
        {
            teleportID = 0;
            this.transform.position = lounge;
        }


    }

}



