using UnityEngine;
using UnityEngine.SceneManagement;

public class NewControl : MonoBehaviour
{
	Rigidbody body;
	Vector3 upward;
	Vector3 forward;
	Vector3 right;
	Vector3 velocity;
	Vector3 rotation;
	Quaternion rotationState;
	bool isGrounded = false;
	Vector3 planetPosition;

	public GameObject planet;
	public float g = 9.81f;
	public GameObject mainCamera;
	public float speed = 5f;
	public float midairAdjustSpeed = 0.05f;
	public float sprintMultiplier = 1.5f;
	public float jumpSpeed = 5f;
	public float mouseYSensitivity = 3f;
	public float mouseXSensitivity = 2f;
	public bool invertLook = false;

	Renderer thisRenderer;
	AudioSource thisAudio;
	int numGems = 0;

	bool doLaunch = false;
	bool isLaunched = false;
	float launchCounter;

	Vector3 groundNormal;
	bool isSloped = false;
	public float wallJumpSpeed = 1f;

	public GameObject deathGuy;
	public float deathReleaseTimer = 60;		//Each world has its own
	bool isDeathReleased = false;

	float mouseMovement = 0;
	Quaternion oldRotationState;
	float rotationAngle;

	public float maxSlope = 45;

	public GameObject characterModel;
	public bool changeExposure = true;

	bool touchShip = false;
	//Scene shipScene;
	GameObject shipSceneHolder;
	public GameObject thisPlanetHolder;
	public int planetNumber;		//For use by gem collector
	MainScreenScript mainScript;

	public GameObject gameOverScreen;
	float gameOverCount = 2;
	public GameObject skyCam;

	//Sun Positioning System
	public GameObject sunlight;
	public float dayRate = 0.02f;
	float sunAngle;
	float sunAngleAbsolute = 0;

   void Start()
   {
	body = GetComponent<Rigidbody>();
	planetPosition = planet.transform.position;
	thisRenderer = GetComponent<Renderer>();
	thisAudio = GetComponent<AudioSource>();
	Cursor.lockState = CursorLockMode.Locked;		//Not working?
	Cursor.visible = false;
	
	//shipScene = SceneManager.GetSceneByName("ShipScene");
	shipSceneHolder = GameObject.Find("ShipSceneHolder");

	GameObject mainControl = GameObject.Find("MainControl");
	mainScript = mainControl.GetComponent<MainScreenScript>();

	gameOverScreen.SetActive(false);

	upward = this.transform.position - planetPosition;				//Initialize directions
	upward.Normalize();
	right = Vector3.Cross(upward, this.transform.forward);	//Unity uses lefthand rule!
	if(right == Vector3.zero) { right = Vector3.Cross(upward, this.transform.up); }
	right.Normalize();
	forward = Vector3.Cross(right, upward);			//Unity uses lefthand rule!
   }

   void FixedUpdate()
   {
	upward = this.transform.position - planetPosition;				//Update directions
	upward.Normalize();
	right = Vector3.Cross(upward, forward);	//Unity uses lefthand rule!
	if(right == Vector3.zero) { right = Vector3.Cross(upward, this.transform.up); }
	right.Normalize();
	forward = Vector3.Cross(right, upward);			//Unity uses lefthand rule!

	rotationState = Quaternion.AngleAxis(mouseMovement, upward);			//Turn Left/Right
	mouseMovement = 0;
	forward = rotationState*forward;
	right = rotationState*right;

	if(doLaunch) {
		doLaunch = false;
		isLaunched = true;
		body.AddTorque(right*10+forward*4, ForceMode.VelocityChange);
	}
	else if(isLaunched) {
		velocity = -upward*g*Time.fixedDeltaTime; 
		launchCounter -= Time.fixedDeltaTime;
		if(launchCounter <= 0) { mainScript.EndGame(); }
	}
	else {
		velocity = forward*Input.GetAxis("Forward");					//Translation
		velocity += right*Input.GetAxis("Horizontal");
		//velocity += upward*Input.GetAxis("Vertical");
		velocity = Vector3.Normalize(velocity);
		if(isGrounded) {
			velocity *= speed;
			if(Input.GetKey(KeyCode.LeftShift)) { velocity *= sprintMultiplier; }
			velocity -= body.linearVelocity;
			if(Input.GetKey(KeyCode.Space)) { velocity += upward*jumpSpeed; }
		}
		else {
			velocity *= midairAdjustSpeed;
			velocity -= upward*g*Time.fixedDeltaTime;
			if(isSloped) {
				if(Input.GetKey(KeyCode.Space)) { velocity += groundNormal*wallJumpSpeed; }
			}
		}
	}

	body.AddForce(velocity, ForceMode.VelocityChange);

	if(!isLaunched) {
		rotationState.SetLookRotation(forward, upward);					//Rectify
		oldRotationState.SetLookRotation(this.transform.forward, this.transform.up);
		rotationState = rotationState * Quaternion.Inverse(oldRotationState);
		rotationState.ToAngleAxis(out rotationAngle, out rotation);
		if(rotationAngle > 180) {
			rotationAngle = 360 - rotationAngle;
			rotation = -rotation;
		}
		rotationAngle *= Mathf.PI/180;
		body.AddTorque(rotationAngle*rotation/Time.fixedDeltaTime - body.angularVelocity, ForceMode.VelocityChange);
	}

	if(isLaunched) { thisRenderer.material.color = Color.blue; 
		characterModel.GetComponent<Renderer>().material.color = Color.red;
		if(gameOverCount <= 0) {
			gameOverScreen.SetActive(true);
		}
		else { gameOverCount -= Time.fixedDeltaTime; }

		
	}
	else if(isGrounded) { thisRenderer.material.color = Color.red; }
	else { thisRenderer.material.color = Color.green; }

	isGrounded = false;	//Having dealt with ground, prepare to do so next time
   }

   void Update()
   {
	mouseMovement += Input.GetAxis("Mouse X")*mouseXSensitivity;

	rotation.x = -Input.GetAxis("Mouse Y")*mouseYSensitivity;			//Look Up/Down
	mainCamera.transform.Rotate(rotation.x, 0, 0);

	if(Input.GetKeyUp(KeyCode.Escape) && gameOverCount <= 0) { mainScript.EndGame(); }

	if(!isDeathReleased) {
		deathReleaseTimer -= Time.deltaTime;					//Countdown to release death
		if(deathReleaseTimer <= 30 * mainScript.difficulty) {
			deathGuy.SetActive(true);
			isDeathReleased = true;
			print("Run!");
		}
	}

	if(Input.GetKeyUp(KeyCode.Q) && touchShip) {					//Enter ship
		mainScript.currentWorld = -1;
		shipSceneHolder.SetActive(true);
		//SceneManager.SetActiveScene(shipScene);
		RenderSettings.skybox = mainScript.blackSky;
		thisPlanetHolder.SetActive(false);
	}

	//Sun Positioning System
 	sunlight.transform.Rotate(0, dayRate, 0, Space.World);
	sunAngle = Vector3.Angle(-sunlight.transform.forward, upward);
	sunAngleAbsolute -= 10*dayRate*Time.deltaTime;			//Temp
	RenderSettings.skybox.SetFloat("_Rotation", sunAngleAbsolute);
	if(sunAngle > 120) { sunAngle = 0 ;}				//Fade calc, temp
	else { sunAngle = (120 - sunAngle)/120; }
	RenderSettings.skybox.SetFloat("_Blend", sunAngle);
	if(changeExposure) { RenderSettings.skybox.SetFloat("_Exposure", 0.7f*sunAngle+0.3f); }
   }

   void OnCollisionEnter(Collision collisionObject)
   {
	if(collisionObject.gameObject.name == "Death") {					//Death Launch
		skyCam.SetActive(false);	//Kill minimap
		mainScript.isAlive = false;	//Prevent main menu
		velocity = this.transform.position - collisionObject.transform.position;
		if(velocity == Vector3.zero) { velocity = forward; }
		else {
			velocity.Normalize();
			velocity -= upward*Vector3.Dot(velocity, upward);
		}
		velocity += upward*0.1f;
		velocity.Normalize();
		velocity *= 30;
		launchCounter = 6;
		doLaunch = true;
	}
   }

   void OnCollisionStay(Collision collisionObject)
   {
	groundNormal = Vector3.zero;
	isSloped = true;

	int numContacts = collisionObject.contactCount;
	ContactPoint[] contactArray = new ContactPoint[numContacts];
	collisionObject.GetContacts(contactArray);//Returns int numContacts
	Vector3 contactNormal;
	float contactAngle;

	for(int i = 0; i<numContacts; i++) {
		contactNormal = contactArray[i].normal;
		contactAngle = Vector3.Angle(contactNormal, upward);
		if(contactAngle <= maxSlope) { isGrounded = true; isSloped = false; }
		groundNormal += contactNormal;
	}
	groundNormal /= numContacts;
	contactAngle = Vector3.Angle(groundNormal, upward);
	if(contactAngle <= maxSlope) { isGrounded = true; isSloped = false; }
   }

   void OnCollisionExit()
   {
	isGrounded = false;
	isSloped = false;
   }

   void OnTriggerEnter(Collider trigger)
   {
	if(trigger.tag == "Gem") {
		trigger.gameObject.SetActive(false);
		numGems++;
		mainScript.numGems[planetNumber]++;
		thisAudio.Play();
		print("Gems: " + numGems);
	}
	if(trigger.tag == "Ship") { touchShip = true; }
   }

   void OnTriggerExit(Collider trigger)
   {
	if(trigger.tag == "Ship") { touchShip = false; }
   }

}
