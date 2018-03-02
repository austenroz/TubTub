//This Script is used to control the boats and display the health of the boat.

using UnityEngine;
using System.Collections;

public class BoatMovement : MonoBehaviour {

	//REFERENCE VARAIBLES//
    private GameObject go_TargetBoat = null;
    private Rigidbody rb_BoatRigidbody;
    private GameObject go_WaitingPlatform;
    private GameObject go_PlaGraphic;
	public GameObject go_PlayerCam;
	public GameObject go_BoatCam;
    private SmoothFollowC ref_Smoothfollow;
    private BoatHealth ref_Boathealth;
    private CameraPosition ref_CameraPos;
    private CameraRotate ref_CameraRot;
    private GameObject go_Button;
    public GameObject go_LaserObject;
    private LineRenderer lr_Laser;
    public GameObject go_BubbleLaunchSound;

    //INPUT METHODS
    private string s_InputMethod;
    public GameObject go_InputReference;

    //GUI CONTROL VARIABLES
    private bool b_DisplayBoatHealthGUI = false;
    public GUIStyle gs_HealthDisplay;

	//USED WHEN PLAYER SWITCHES BETWEEN DRIVING AND WALKING
	//Vector3 vec3_PlayerExitPosition = new Vector3(0, 3, 0);
    private float f_DistanceToEnter = 10.0f;
    private float f_DistanceToPushButton = 10.0f;
	private bool b_IsDriving = false;

	//BOAT MOVEMENT VARIABLES
    private float f_BoatForwardSpeed = 20.0f;
    private float f_BoatBackwardSpeed = 10.0f;
    private float f_Acceleration = 20.0f;
    private float f_Deceleration = 10.0f;
    private bool b_Aiming = false;
    public GameObject go_BoatTurnDirectionMesh;

	//CANNON VARIABLES
	private Transform tm_Cannon;
	private float f_ControllerXInput;
	public Transform tm_Projectile;
	private float f_CannonTurnSensitivity = 150.0f;
    private float f_CannonTiltSensitivity = 65.0f;
	private float f_ControllerFireInput;
	private bool b_ReleaseTrigger = true;
	private float f_CannonTimer = 0.0f;
	private float f_ReloadTime = .1f;
	private float f_Damage = 15.0f;
    [HideInInspector] public float f_BoatHealth = 100.0f;
    private float f_MinimumTilt = -15F;
    private float f_MaximumTilt = 15F;
    private float f_CannonRotationY = 0F;
    public RaycastHit rch_LaserHit;
    public Material mat_LaserMaterial;
    private float f_CameraHeight;

    //Used to tell script to kick player out of boat
    [HideInInspector] public bool b_BoatDead = false;

	//Used to keep a local record of the boats speed before each update
	private Vector3 vec3_BoatVelocity;

    //Set variables
    void Start()
    {
        go_BoatCam.camera.enabled = false;
        vec3_BoatVelocity = new Vector3(0, 0, 0);
        ref_CameraPos = transform.Find("Main Camera").GetComponent<CameraPosition>();
        ref_CameraRot = transform.Find("Main Camera").GetComponent<CameraRotate>();
        go_WaitingPlatform = GameObject.FindGameObjectWithTag("WaitingPlatform");
        go_PlaGraphic = transform.Find("Graphics").gameObject;
        ref_Smoothfollow = go_BoatCam.GetComponent<SmoothFollowC>();
        f_CameraHeight = ref_Smoothfollow.height;
        lr_Laser = go_LaserObject.GetComponent<LineRenderer>();
        s_InputMethod = go_InputReference.name;
    }

    //display the boat health on screen 
    void OnGUI()
    {
        if (b_DisplayBoatHealthGUI)
        {
            float offsetHeight = transform.Find("Main Camera").GetComponent<Camera>().rect.y;
            GUI.Label(new Rect(10, (Screen.height - (Screen.height * offsetHeight)) - 30, 200, 20), "Boat Health: " + 
                      go_TargetBoat.GetComponent<BoatHealth>().getHealth(), gs_HealthDisplay);
        }
    }

    //Getter and setters for player f_Damage and player f_ReloadTime
    public float getDamage()
    {
        return f_Damage;
    }
    public void setDamage(float damage)
    {
        f_Damage = damage;
    }
    public void setReloadTime(float reloadTime)
    {
        f_ReloadTime = reloadTime;
    }
    public void setBoatHealth(float boatHealth)
    {
        f_BoatHealth = boatHealth;
    }
    public void setHealthGUIVisiblility(bool b_Visible)
    {
        b_DisplayBoatHealthGUI = b_Visible;
    }

	//function outputs a velocity that is multiplied by a constant rate for a smooth acceleration between velocities
	//Used for boat movement
	float acceleration(float initial, float rate, float minVelocity, float maxVelocity)
	{
		if (initial + (rate * Time.fixedDeltaTime) < maxVelocity && (initial + (rate * Time.fixedDeltaTime) > minVelocity))
		{
			return initial + (rate * Time.fixedDeltaTime);
		}
		if (initial + (rate * Time.fixedDeltaTime) >= maxVelocity)
		{
			return maxVelocity;
		}
		if (initial + (rate * Time.fixedDeltaTime) <= minVelocity)
		{
			return minVelocity;
		}
		return 0.0f;
	}

	//Is called by the collision script on the boats if they collide with an object
	public void WallCollision()
	{
        /*
		if (rb_BoatRigidbody != null)
		{
			if (vec3_BoatVelocity.z > 5.0f)
			{
                rb_BoatRigidbody.velocity = go_TargetBoat.transform.Find("PlayerBoat").TransformDirection(vec3_BoatVelocity.x, vec3_BoatVelocity.y, vec3_BoatVelocity.z / -2);
				vec3_BoatVelocity = new Vector3 (vec3_BoatVelocity.x,vec3_BoatVelocity.y,vec3_BoatVelocity.z / -2);
			} else
			{
                rb_BoatRigidbody.velocity = go_TargetBoat.transform.Find("PlayerBoat").TransformDirection(vec3_BoatVelocity.x, vec3_BoatVelocity.y, -vec3_BoatVelocity.z);
                vec3_BoatVelocity = new Vector3(vec3_BoatVelocity.x, vec3_BoatVelocity.y, -vec3_BoatVelocity.z);
			}
		}*/
	}

	//Determine if there is a boat in range of the player. If there is a boat, returns true and
    //sets a reference to the boat in go_TargetBoat.
	bool BoatInRange(GameObject[] boats)
	{
		bool b_IsInRange = false;
		GameObject go_BestTarget = null;
		float f_ClosestDistanceSqr = Mathf.Infinity;
		Vector3 vec3_CurrentPostion = transform.position;
		foreach(GameObject go_potentialTarget in boats)
		{
			Vector3 vec3_DirectionToTarget = go_potentialTarget.transform.position - vec3_CurrentPostion;
			float f_DSqrToTarget = vec3_DirectionToTarget.sqrMagnitude;
			if (f_DSqrToTarget < f_ClosestDistanceSqr)
			{
				f_ClosestDistanceSqr = f_DSqrToTarget;
				go_BestTarget = go_potentialTarget;
				if (f_DSqrToTarget < f_DistanceToEnter && go_BestTarget.GetComponent<BoatHealth>().b_IsActive == false)
				{
                    //Set Variables
					b_IsInRange = true;
                    go_TargetBoat = go_BestTarget;
                    rb_BoatRigidbody = go_TargetBoat.GetComponent<Rigidbody>();
                    vec3_BoatVelocity = rb_BoatRigidbody.velocity;
                    tm_Cannon = go_TargetBoat.transform.Find("Cannon").transform.Find("CannonTopBase");
                    ref_Smoothfollow.target = tm_Cannon;
                    ref_Boathealth = go_TargetBoat.GetComponent<BoatHealth>();
                    f_BoatHealth = go_TargetBoat.GetComponent<BoatHealth>().getHealth();
				}
			}
		}
		return b_IsInRange;
	}

    //Used to calculate if there is a button in range of the player when action button is pressed
    //Returns true if there is a button in range and puts reference to object in go_Button variable
    bool ButtonInRange(GameObject[] buttons)
    {
        bool b_IsInRange = false;
        GameObject go_BestTarget = null;
        float f_ClosestDistanceSqr = Mathf.Infinity;
        Vector3 vec3_CurrentPostion = transform.position;
        foreach (GameObject go_potentialTarget in buttons)
        {
            Vector3 vec3_DirectionToTarget = go_potentialTarget.transform.position - vec3_CurrentPostion;
            float f_DSqrToTarget = vec3_DirectionToTarget.sqrMagnitude;
            if (f_DSqrToTarget < f_ClosestDistanceSqr)
            {
                f_ClosestDistanceSqr = f_DSqrToTarget;
                go_BestTarget = go_potentialTarget;
                if (f_DSqrToTarget < f_DistanceToPushButton)
                {
                    //Set Variables
                    b_IsInRange = true;
                    go_Button = go_BestTarget;
                }
            }
        }
        return b_IsInRange;
    }

	//Method that moves the boat according to user input
	void UpdateMovement()
	{
		//INPUTS//
		float f_SidewaysInput = 0;
		float f_ForwardInput = 0;

        //If player is using keyboard for input
        if (s_InputMethod == "Keyboard")
        {
            if (Input.GetKey(KeyCode.A))
            {
                f_SidewaysInput += -1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                f_SidewaysInput += 1;
            }
            if (Input.GetKey(KeyCode.W))
            {
                f_ForwardInput += 1;
            }
            if (Input.GetKey(KeyCode.S))
            {
                f_ForwardInput += -1;
            }
        }
            //If player is using controller for input
        else
        {
            f_SidewaysInput = Input.GetAxis(s_InputMethod + "Horizontal");
            f_ForwardInput = Input.GetAxis(s_InputMethod + "Vertical");
        }


        //Boat Rotation handling
        go_BoatTurnDirectionMesh.transform.rotation = tm_Cannon.rotation;
        Vector3 vec3_LocalDir = tm_Cannon.TransformDirection(tm_Cannon.forward);
        vec3_LocalDir *= -f_ForwardInput;
        Vector3 vec3_LocalDir2 = go_BoatTurnDirectionMesh.transform.right;
        vec3_LocalDir2 *= f_SidewaysInput;
        go_BoatTurnDirectionMesh.transform.position = tm_Cannon.position + vec3_LocalDir + vec3_LocalDir2;
        Vector3 vec3_Angle = go_BoatTurnDirectionMesh.transform.position - tm_Cannon.position;
        Quaternion qt_Rotate = Quaternion.LookRotation(vec3_Angle);
        if (f_SidewaysInput != 0 || f_ForwardInput != 0)
        {
            go_TargetBoat.transform.Find("PlayerBoat").rotation = Quaternion.RotateTowards(go_TargetBoat.transform.Find("PlayerBoat").rotation, qt_Rotate, 300.0f * Time.deltaTime);
        }

        //Calculate speed of boat based on current direction and desired direction
        if (f_SidewaysInput != 0 || f_ForwardInput != 0)
        {
            go_BoatTurnDirectionMesh.transform.rotation = qt_Rotate;
            float f_Diff = Mathf.Abs(go_TargetBoat.transform.Find("PlayerBoat").rotation.eulerAngles.y - go_BoatTurnDirectionMesh.transform.rotation.eulerAngles.y);
            if (f_Diff > 1.0f)
                f_ForwardInput = 1.0f / (f_Diff);
            else
                f_ForwardInput = 1.0f;
        }

		//Determine if input calls for boat to move forward, backward, or to decelerate.
		//Then uses acceleration function to determine the velocity
		Vector3 vec3_RelativeVelocity = new Vector3 (0,0,0);
		if (f_ForwardInput > 0)
		{
			vec3_RelativeVelocity = go_TargetBoat.transform.Find("PlayerBoat").TransformDirection (0, 0, acceleration(vec3_BoatVelocity.z, f_Acceleration * f_ForwardInput, -f_BoatBackwardSpeed, f_BoatForwardSpeed));
			vec3_BoatVelocity = new Vector3 (0, vec3_BoatVelocity.y, acceleration(vec3_BoatVelocity.z, f_Acceleration * f_ForwardInput, -f_BoatBackwardSpeed, f_BoatForwardSpeed));

		} 
		//move backward (Used for bumping into objects)
		else if (f_ForwardInput < 0)
		{
            vec3_RelativeVelocity = go_TargetBoat.transform.Find("PlayerBoat").TransformDirection(0, 0, acceleration(vec3_BoatVelocity.z, -f_Acceleration * -f_ForwardInput, -f_BoatBackwardSpeed, f_BoatForwardSpeed));
			vec3_BoatVelocity = new Vector3 (0, vec3_BoatVelocity.y, acceleration(vec3_BoatVelocity.z, -f_Acceleration * -f_ForwardInput, -f_BoatBackwardSpeed, f_BoatForwardSpeed));

		}
		//decelerate
		else
		{
			if (vec3_BoatVelocity.z > 0)
			{
                vec3_RelativeVelocity = go_TargetBoat.transform.Find("PlayerBoat").TransformDirection(0, 0, acceleration(vec3_BoatVelocity.z, -f_Deceleration, 0.0f, f_BoatForwardSpeed));
				vec3_BoatVelocity = new Vector3 (0, vec3_BoatVelocity.y, acceleration(vec3_BoatVelocity.z, -f_Deceleration, 0.0f, f_BoatForwardSpeed));
			}
			else if (vec3_BoatVelocity.z != 0)
			{
                vec3_RelativeVelocity = go_TargetBoat.transform.Find("PlayerBoat").TransformDirection(0, 0, acceleration(vec3_BoatVelocity.z, f_Deceleration, -f_BoatBackwardSpeed, 0.0f));
				vec3_BoatVelocity = new Vector3 (0, vec3_BoatVelocity.y, acceleration(vec3_BoatVelocity.z, f_Deceleration, -f_BoatBackwardSpeed, 0.0f));
			}
		}
        //set boat to speed calculated above
		rb_BoatRigidbody.velocity = vec3_RelativeVelocity;
	}
	
	void FixedUpdate()
	{
		//CONTROLLING BOAT//
		if (b_IsDriving)
		{
			UpdateMovement ();
		}
	}

	// Update is called once per frame
	void Update () 
	{
        //If game is not paused
        if (Time.timeScale > 0f)
        {
            //INPUTS
            bool b_ActionButton = false;
            f_ControllerXInput = 0.0f;
            float f_MouseXInput = 0.0f;
            float f_ControllerYInput = 0.0f;
            float f_MouseYInput = 0.0f;
            bool b_LaserButton = false;
            bool b_FireCannonButton = false;

            //If player is using keyboard for input
            if (s_InputMethod == "Keyboard")
            {
                if (Input.GetKeyDown(KeyCode.F))
                    b_ActionButton = true;
                f_MouseXInput = Input.GetAxis("Mouse X");
                f_MouseYInput = -(Input.GetAxis("Mouse Y"));
                if (Input.GetMouseButton(1))
                    b_LaserButton = true;
                if (Input.GetMouseButton(0))
                    b_FireCannonButton = true;
            }
            //Using controller for input
            else
            {
                if (Input.GetButtonDown(s_InputMethod + "Enter"))
                    b_ActionButton = true;
                f_ControllerXInput = Input.GetAxis(s_InputMethod + "RotateX");
                f_ControllerYInput = Input.GetAxis(s_InputMethod + "RotateY");
                float controllerLaserInput = Input.GetAxis(s_InputMethod + "Laser");
                if (controllerLaserInput > 0.2f)
                    b_LaserButton = true;
                f_ControllerFireInput = Input.GetAxis(s_InputMethod + "Fire");
                if (f_ControllerFireInput > 0.2f)
                {
                    b_FireCannonButton = true;
                }
            }

            //Enter closest boat in range or leave boat player is currently driving
            if (b_ActionButton || b_BoatDead)
            {
                b_BoatDead = false;
                if (b_IsDriving)
                {
                    //eject from target boat
                    b_IsDriving = false;
                    rb_BoatRigidbody.velocity = new Vector3(0, 0, 0);
                    rb_BoatRigidbody.isKinematic = true;
                    vec3_BoatVelocity = new Vector3(0, 0, 0);
                    transform.position = go_TargetBoat.transform.Find("PlayerExitPosition").position;
                    ref_CameraPos.resetRotation();
                    ref_CameraRot.resetRotation();
                    go_PlaGraphic.renderer.enabled = true;
                    go_BoatCam.camera.enabled = false;
                    go_PlayerCam.camera.enabled = true;
                    go_TargetBoat = null;
                    rb_BoatRigidbody = null;
                    ref_Boathealth.b_IsActive = false;
                    b_DisplayBoatHealthGUI = false;
                    lr_Laser.SetVertexCount(0);
                }
                else
                //player not currently driving boat
                {
                    //ACTION BUTTON PRESSED
                    //See if go_Button is in range and use it if it is
                    if (ButtonInRange(GameObject.FindGameObjectsWithTag("Button")))
                    {
                        if (go_Button.GetComponent<GateOpener>() != null)
                        {
                            go_Button.GetComponent<GateOpener>().OpenGate();
                        }
                    }
                    //If there isn't a go_Button in range, eject player from boat
                    else if (BoatInRange(GameObject.FindGameObjectsWithTag("Boat")))
                    {
                        GameObject.FindGameObjectWithTag("LevelManager").GetComponent<CurrentLevelStats>().b_BeginRound = true;
                        transform.position = go_WaitingPlatform.transform.position + new Vector3(0, 5.0f, 0);
                        go_PlaGraphic.renderer.enabled = false;
                        b_IsDriving = true;
                        rb_BoatRigidbody.isKinematic = false;
                        go_BoatCam.transform.position = go_PlayerCam.transform.position + new Vector3(0, 20, 0);//
                        go_BoatCam.camera.enabled = true;
                        go_PlayerCam.camera.enabled = false;
                        ref_Boathealth.b_IsActive = true;
                        b_DisplayBoatHealthGUI = true;
                        go_TargetBoat.GetComponent<BoatHealth>().go_CurrentPlayer = gameObject;
                    }
                }
            }
            //If player is currently driving boat
            if (b_IsDriving)
            {
                //Cannon Position
                tm_Cannon.transform.position = go_TargetBoat.transform.Find("PlayerBoat").Find("CannonBase").position + new Vector3(0, .8f, 0);

                //Cannon rotation
                if (f_ControllerXInput != 0)
                {
                    tm_Cannon.RotateAround(go_TargetBoat.transform.Find("PlayerBoat").Find("CannonBase").position, go_TargetBoat.transform.up, f_ControllerXInput * f_CannonTurnSensitivity * Time.deltaTime);
                }
                else if (f_MouseXInput != 0)
                {
                    tm_Cannon.RotateAround(go_TargetBoat.transform.Find("PlayerBoat").Find("CannonBase").position, go_TargetBoat.transform.up, f_MouseXInput * f_CannonTurnSensitivity * Time.deltaTime);
                }

                //Cannon Tilt
                float f_CamHeight = 2.5f;
                float f_CannonRotation;
                //Controller input
                if (f_ControllerYInput != 0)
                {
                    f_CannonRotation = f_CannonRotationY;
                    f_CannonRotationY += f_ControllerYInput * f_CannonTiltSensitivity * Time.deltaTime;
                    f_CannonRotationY = Mathf.Clamp(f_CannonRotationY, f_MinimumTilt, f_MaximumTilt);
                    f_CannonRotation -= f_CannonRotationY;
                    tm_Cannon.transform.Find("CannonTop").RotateAround(go_TargetBoat.transform.Find("PlayerBoat").Find("CannonBase").position +
                                                                    new Vector3(0, .5f, 0), tm_Cannon.right, -f_CannonRotation);
                }
                //Mouse input
                else if (f_MouseYInput != 0)
                {
                    f_CannonRotation = f_CannonRotationY;
                    f_CannonRotationY += f_MouseYInput * f_CannonTiltSensitivity * Time.deltaTime;
                    f_CannonRotationY = Mathf.Clamp(f_CannonRotationY, f_MinimumTilt, f_MaximumTilt);
                    f_CannonRotation -= f_CannonRotationY;
                    tm_Cannon.transform.Find("CannonTop").RotateAround(go_TargetBoat.transform.Find("PlayerBoat").Find("CannonBase").position +
                                                                    new Vector3(0, .5f, 0), tm_Cannon.right, -f_CannonRotation);
                }
                //change camera height based on tilt of camera
                f_CamHeight += (-f_CannonRotationY / -10.0f) + f_CameraHeight;
                ref_Smoothfollow.height = f_CamHeight;

                //Laser
                if (b_LaserButton)
                {
                    //show laser
                    Vector3 vec3_LaserStart = tm_Cannon.transform.Find("CannonTop").transform.Find("Barrel").transform.position +
                                         tm_Cannon.transform.Find("CannonTop").transform.Find("Barrel").transform.TransformDirection(0f, 0f, 2.0f);
                    Vector3 vec3_Dir = (tm_Cannon.transform.Find("CannonTop").transform.Find("Barrel").transform.forward);
                    //if a raycast hits something
                    if (Physics.Raycast(vec3_LaserStart, vec3_Dir, out rch_LaserHit, 500.0f))
                    {
                        lr_Laser.SetVertexCount(2);
                        lr_Laser.SetPosition(0, vec3_LaserStart);
                        lr_Laser.SetPosition(1, rch_LaserHit.point);
                    }
                    //if the raycast didn't hit something, make a laser a set length
                    else
                    {
                        Ray r = new Ray(vec3_LaserStart, vec3_Dir);
                        lr_Laser.SetVertexCount(2);
                        lr_Laser.SetPosition(0, vec3_LaserStart);
                        lr_Laser.SetPosition(1, r.GetPoint(500.0f));
                    }
                    //slow down b_Aiming
                    if (!b_Aiming)
                    {
                        b_Aiming = true;
                        f_CannonTurnSensitivity *= .25f;
                        f_CannonTiltSensitivity *= .25f;
                        ref_Smoothfollow.distance -= 1;
                    }
                }
                //When user releases aim button, reset variables to default values
                else if (b_Aiming)
                {
                    b_Aiming = false;
                    f_CannonTurnSensitivity *= 4;
                    f_CannonTiltSensitivity *= 4;
                    ref_Smoothfollow.distance += 1;
                    lr_Laser.SetVertexCount(0);
                }

                //Cannon Fire
                if (f_CannonTimer <= f_ReloadTime)
                {
                    f_CannonTimer += Time.deltaTime;
                }
                if (b_FireCannonButton)
                {
                    if (f_CannonTimer >= f_ReloadTime)
                    {
                        f_CannonTimer = 0.0f;
                        Instantiate(go_BubbleLaunchSound, tm_Cannon.transform.Find("CannonTop").transform.Find("Barrel").transform.position +
                                    tm_Cannon.transform.Find("CannonTop").transform.Find("Barrel").transform.TransformDirection(0f, 0f, 2.5f), 
                                    Quaternion.identity);
                        Instantiate(tm_Projectile, tm_Cannon.transform.Find("CannonTop").transform.Find("Barrel").transform.position +
                                    tm_Cannon.transform.Find("CannonTop").transform.Find("Barrel").transform.TransformDirection(0f, 0f, 2.5f),
                                    tm_Cannon.transform.Find("CannonTop").transform.Find("Barrel").transform.rotation);
                    }
                }
            }
        }
	}
}