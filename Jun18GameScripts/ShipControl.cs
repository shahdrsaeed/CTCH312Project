using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipControl : MonoBehaviour
{
	Rigidbody body;
	Vector3 upward = Vector3.up;
	Vector3 forward;
	Vector3 right;
	Vector3 velocity;
	Vector3 rotation;
	Quaternion rotationState;
	bool isGrounded = false;

	public float g = 9.81f;
	public GameObject mainCamera;
	public float speed = 5f;
	public float midairAdjustSpeed = 0.05f;
	public float sprintMultiplier = 1.5f;
	public float jumpSpeed = 5f;
	public float mouseYSensitivity = 3f;
	public float mouseXSensitivity = 2f;
	public bool invertLook = false;

	Vector3 groundNormal;
	bool isSloped = false;
	public float wallJumpSpeed = 1f;
	float mouseMovement = 0;

	Renderer characterRenderer;
	public GameObject playerModel;

	Quaternion oldRotationState;
	float rotationAngle;

	public float maxSlope = 45;

	bool touchPortal1 = false;
	bool touchPortal2 = false;
	bool touchPortal3 = false;
	bool touchPortalHope = false;

	//Scene shipScene;
	GameObject shipSceneHolder;
	//Scene portal1Scene;
	GameObject portal1Holder;
	//Scene portal2Scene;
	GameObject portal2Holder;
	//Scene portal3Scene;
	GameObject portal3Holder;
	MainScreenScript mainScript;
	bool portal2Locked = true;
	bool portal3Locked = true;
	public GameObject portal2Object;
	public Material portal2UnlockedMaterial;
	public GameObject portal3Object;
	public Material portal3UnlockedMaterial;

	public int fewGemNum = 50;
	public int manyGemNum = 100;

    void Start()
    {
	body = GetComponent<Rigidbody>();
	Cursor.lockState = CursorLockMode.Locked;		//Not working?
	Cursor.visible = false;
	
	characterRenderer = playerModel.GetComponent<Renderer>();

	//shipScene = SceneManager.GetSceneByName("ShipScene");		//Get Scene Data
	shipSceneHolder = GameObject.Find("ShipSceneHolder");
	//portal1Scene = SceneManager.GetSceneByName("tutorialScene");
	portal1Holder = GameObject.Find("TutorialHolder");
	//portal2Scene = SceneManager.GetSceneByName("Planet2Scene");
	portal2Holder = GameObject.Find("Planet2Holder");
	//portal3Scene = SceneManager.GetSceneByName("Planet3Scene");
	portal3Holder = GameObject.Find("Planet3Holder");
	GameObject mainControl = GameObject.Find("MainControl");
	mainScript = mainControl.GetComponent<MainScreenScript>();

				//Initialize directions
	right = Vector3.Cross(upward, this.transform.forward);	//Unity uses lefthand rule!
	if(right == Vector3.zero) { right = Vector3.Cross(upward, this.transform.up); }
	right.Normalize();
	forward = Vector3.Cross(right, upward);			//Unity uses lefthand rule!
    }

    void FixedUpdate()
    {
				//Update directions
	right = Vector3.Cross(upward, forward);	//Unity uses lefthand rule!
	if(right == Vector3.zero) { right = Vector3.Cross(upward, this.transform.up); }
	right.Normalize();
	forward = Vector3.Cross(right, upward);			//Unity uses lefthand rule!

	rotationState = Quaternion.AngleAxis(mouseMovement, upward);			//Turn Left/Right
	mouseMovement = 0;
	forward = rotationState*forward;
	right = rotationState*right;

		velocity = forward*Input.GetAxis("Forward");					//Translation
		velocity += right*Input.GetAxis("Horizontal");
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

	body.AddForce(velocity, ForceMode.VelocityChange);

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

	isGrounded = false;		//Having dealt with ground, prepare to deal with it next time

    }

   void Update()
   {
	mouseMovement += Input.GetAxis("Mouse X")*mouseXSensitivity;

	rotation.x = -Input.GetAxis("Mouse Y")*mouseYSensitivity;			//Look Up/Down
	mainCamera.transform.Rotate(rotation.x, 0, 0);

	if(Input.GetKeyUp(KeyCode.Q)) {							//Portal activation
		if(touchPortal1) {
			mainScript.currentWorld = 0;
			portal1Holder.SetActive(true);
			//SceneManager.SetActiveScene(portal1Scene);
			RenderSettings.skybox = mainScript.atmoSky;
			shipSceneHolder.SetActive(false);
		}
		if(touchPortal2) {
			if(portal2Locked) {
				if(mainScript.numGems[0] >= fewGemNum) {
					portal2Locked = false;
					Renderer portal2Renderer = portal2Object.GetComponent<Renderer>();
					portal2Renderer.material = portal2UnlockedMaterial;
				}
			}
			else {
				mainScript.currentWorld = 1;
				portal2Holder.SetActive(true);
				//SceneManager.SetActiveScene(portal2Scene);
				RenderSettings.skybox = mainScript.atmoSky;
				shipSceneHolder.SetActive(false);
			}
		}
		if(touchPortal3) {
			if(portal3Locked) {
				if(mainScript.numGems[0] >= manyGemNum || mainScript.numGems[1] >= fewGemNum) {
					portal3Locked = false;
					Renderer portal3Renderer = portal3Object.GetComponent<Renderer>();
					portal3Renderer.material = portal3UnlockedMaterial;
				}
			}
			else {
				mainScript.currentWorld = 2;
				portal3Holder.SetActive(true);
				//SceneManager.SetActiveScene(portal3Scene);
				RenderSettings.skybox = mainScript.starSky;
				shipSceneHolder.SetActive(false);
			}
		}
		if(touchPortalHope) {
			if(mainScript.numGems[1] >= manyGemNum || mainScript.numGems[2] >= fewGemNum) {
				mainScript.WinGame();
			}
		}
	}

   }

   void OnCollisionEnter(Collision collisionObject)
   {

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
	if(trigger.name == "Portal1") {
		touchPortal1 = true;
	}
	if(trigger.name == "Portal2") {
		touchPortal2 = true;
	}
	if(trigger.name == "Portal3") {
		touchPortal3 = true;
	}
	if(trigger.name == "PortalHope") {
		touchPortalHope = true;
	}
   }

   void OnTriggerExit(Collider trigger)
   {
	if(trigger.name == "Portal1") {
		touchPortal1 = false;
	}
	if(trigger.name == "Portal2") {
		touchPortal2 = false;
	}
	if(trigger.name == "Portal3") {
		touchPortal3 = false;
	}
	if(trigger.name == "PortalHope") {
		touchPortalHope = false;
	}
   }
}
