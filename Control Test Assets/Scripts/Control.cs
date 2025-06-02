using UnityEngine;

public class Control : MonoBehaviour
{
	Rigidbody body;
	Vector3 upward;
	Vector3 forward;
	Vector3 right;
	Vector3 velocity;
	Vector3 rotation;
	Quaternion rotationState;
	Quaternion turnQuaternion;
	bool isGrounded = false;
	Vector3 planetPosition;

	public GameObject planet;
	public float g = 9.81f;
	public GameObject mainCamera;
	public float speed = 0.02f;
	public float midairAdjustSpeed = 0.01f;
	public float sprintMultiplier = 2f;
	public float jumpSpeed = 2f;
	public float mouseSensitivity = 3f;
	public bool invertLook = false;

	Renderer thisRenderer;


    void Start()
    {
	body = GetComponent<Rigidbody>();
	planetPosition = planet.transform.position;
	thisRenderer = GetComponent<Renderer>();
    }

    void FixedUpdate()
    {
	upward = this.transform.position - planetPosition;				//Get directions
	upward.Normalize();
	right = Vector3.Cross(upward, this.transform.forward);	//Unity uses lefthand rule!
	if(right == Vector3.zero) { right = Vector3.Cross(upward, this.transform.up);}
	right.Normalize();
	forward = Vector3.Cross(right, upward);			//Unity uses lefthand rule!

	velocity = forward*Input.GetAxis("Forward");					//Translation
	velocity += right*Input.GetAxis("Horizontal");
	//velocity += upward*Input.GetAxis("Vertical");
	velocity = Vector3.Normalize(velocity);
	//if(isGrounded) {
		velocity *= speed;
		if(Input.GetKey(KeyCode.LeftShift)) { velocity *= sprintMultiplier; }
		velocity -= forward*Vector3.Dot(body.linearVelocity, forward);
		velocity -= right*Vector3.Dot(body.linearVelocity, right);
		//velocity -= upward*Vector3.Dot(body.linearVelocity, upward);

		velocity -= upward*g*Time.fixedDeltaTime;
	//}
	/*else {
		velocity *= midairAdjustSpeed;
		velocity -= upward*g*Time.fixedDeltaTime;
	}*/

	if(isGrounded) {
		if(Input.GetKey(KeyCode.Space)) { velocity += upward*jumpSpeed; }
	}

	body.AddForce(velocity, ForceMode.VelocityChange);

	rotationState.SetLookRotation(forward, upward);					//Rectify
	body.MoveRotation(rotationState);

	if (isGrounded) { thisRenderer.material.color = Color.red; }
	else { thisRenderer.material.color = Color.green; }
	
    }

   void Update()
   {
	rotation = upward*Input.GetAxis("Mouse X")*mouseSensitivity;			//Turn Left/Right
	body.AddTorque(rotation - body.angularVelocity/(2*Mathf.PI), ForceMode.VelocityChange);

	rotation.x = -Input.GetAxis("Mouse Y")*mouseSensitivity;			//Look Up/Down
	mainCamera.transform.Rotate(rotation.x, 0, 0);
   }

   void OnCollisionEnter()
   {
	isGrounded = true;
   }

   void OnCollisionExit()
   {
	isGrounded = false;
   }

}
