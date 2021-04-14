using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;

public class PlayerMovement : NetworkBehaviour //MonoBehaviour
{

    //PLAYER NAME
    public TextMesh playerNameText;
    public GameObject floatingInfo;

    public string textName;

    [SyncVar(hook = nameof(OnNameChanged))]
    public string playerName;
   
    
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
    private float hostCounter = 0f;

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
    [SerializeField] float maxCameraX = 75f;
    [SerializeField] float minCameraX = -75f;
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
    private Scene currentScene;
    private string sceneName;
    private string playerTag = "PLAYER_CLONE";

    private GameObject hostCam;

    [SyncVar]
    public Vector3 hostCamRot;

    //Name Server Stuff
    void OnNameChanged(string _Old, string _New)
    {
        playerNameText.text = playerName;
    }

    public void inputName(string boxName)
    {
        textName = boxName;

    }

    public override void OnStartLocalPlayer()
    {           
        string name = textName;
        CmdSetupPlayer(name);


        if (isLocalPlayer)
        {
             //make local players run this
             floatingInfo.transform.rotation = Quaternion.Euler(0,180,0);
             return;
        }

    }

  
    [Command]
    public void CmdSetupPlayer(string _name)
    {
        // player info sent to server, then server updates sync vars which handles it on all clients
        playerName = _name;
        //playerColor = _col;
    }


    //name stuff end

    void Start()
    {

        //SCENE MANAGEMENT
        
        // Create a temporary reference to the current scene.
        currentScene = SceneManager.GetActiveScene ();
 
        // Retrieve the name of this scene.
        sceneName = currentScene.name;
        defaultMaterial = transform.Find("MAIN CHARACTER").GetComponent<Renderer>().material;

        //hostCam = GameObject.FindGameObjectWithTag("HOSTCAM");

        if (isLocalPlayer)
        {
            //SET CAMERA
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = cameraOffset;
            Camera.main.transform.rotation = Quaternion.identity;

            //Set Projectile Mount to Child of Main Cam
            projectileMount.transform.parent = Camera.main.transform;

            //Hide Mouse
            Cursor.lockState = CursorLockMode.Locked;

            //Change Tag
            transform.gameObject.tag = playerTag;

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

        if (sceneName == "TESTING") 
        {
            //TELEPORT ZONE
            teleportZone = GameObject.FindGameObjectWithTag("TELEPORT_ZONE").transform.position;

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
    }


    public void AdjustSpeed(float adjustSpeed)
    {

        var player = GameObject.FindGameObjectWithTag(playerTag);
        player.GetComponent<PlayerMovement>().mouseSensitivity = adjustSpeed;
        var playerMove = player.GetComponent<PlayerMovement>().mouseSensitivity;
        //Debug.Log(playerMove);
        
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

                //this.transform.position = teleportZone;

            }

           
            //SCENE MANAGEMENT
            if (sceneName == "TESTING") 
            {
          
                //TELEPORTING IF IN TESTING SCENE
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
    }

    //BECOME HOST
    [Command]
    void CmdBecomeHost()
    {
        RpcShowHost();
    }

    [ClientRpc]
    void RpcShowHost()
    {
        this.gameObject.tag = "HOST";
        GameObject host = GameObject.FindGameObjectWithTag("HOST");
        transform.Find("MAIN CHARACTER").GetComponent<Renderer>().material = hostMaterial;
        
        //Set up Host Cam
        // Transform hostMainCamTest = host.transform.Find("HostCamHolder");
        // hostCamRot = hostMainCamTest.eulerAngles;        
        // hostCam.transform.SetParent(host.transform);
        // hostCam.transform.localPosition = new Vector3 (0,0,0);
        // hostCam.transform.rotation = host.transform.rotation;
        
    }

    [Command]
    void CmdReleaseHost()
    {
        RpcCancelHost();
    }

    [ClientRpc]
    void RpcCancelHost()
    {
        GameObject host = GameObject.FindGameObjectWithTag("HOST");
        host.tag = playerTag;
        transform.Find("MAIN CHARACTER").GetComponent<Renderer>().material = defaultMaterial;
    }


    [Command(ignoreAuthority = true)]
    void CmdTourGuide()
    {
        // if(isLocalPlayer)
        // {
            // // hostLocation = this.gameObject.transform.position;
            RpccollectGuests();
            // Debug.Log("LOCAL_GUIDE");
                
        // }
    }

    [ClientRpc]
    void RpccollectGuests()
    {
        Vector3 serverHostLocation = GameObject.FindGameObjectWithTag("HOST").transform.position;
        //Debug.Log(serverHostLocation);

            
        GameObject[] guests = GameObject.FindGameObjectsWithTag(playerTag);
        //Debug.Log(guests.Length + "GUESTS");
            
        foreach (GameObject i in guests)
        {
            //Debug.Log(hostLocation);
            i.transform.position = serverHostLocation;
            // i.transform.position = new Vector3 (0,0,0);
        }

    } 

    //TANK STUFF HERE
        [Command]
        void CmdFire()
        {
            GameObject projectile = Instantiate(projectilePrefab, projectileMount.position, transform.rotation); 
            // GameObject projectile = Instantiate(projectilePrefab, rayPosition, transform.rotation);
            NetworkServer.Spawn(projectile);
            // cameraDir = Camera.main.transform.forward;
            // SyncCam();
            
            
        }

    //TANK END

    void Update()
    {
        HandleMovement();


        //NAME STUFF ON UPDATE
        if (!isLocalPlayer)
        {
                // make non-local players run this
                floatingInfo.transform.LookAt(Camera.main.transform);
                return;
        }
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
            hostCounter = 1 + hostCounter;
        }

        if (hostCounter > 5f)
        {
            host = true;
        }

        if (host)
        {
            
            if (Input.GetKeyDown(KeyCode.H))
            {
                CmdBecomeHost();
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                host = false;
                hostCounter = 0;
                // transform.Find("MAIN CHARACTER").GetComponent<Renderer>().material = defaultMaterial;
                // this.gameObject.tag = playerTag;

                CmdReleaseHost();
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                CmdTourGuide();
            }
        }


        //TELEPORTING
        if(teleportID == 1 && teleportTimer + teleportBuffer < Time.time)
        {
            teleportID = 0;
            this.transform.position = startPosition;
        }


        if (sceneName == "TESTING") 
        {
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

}



