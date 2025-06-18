using UnityEngine;

public class DeathScript : MonoBehaviour
{
	Rigidbody body;
	Vector3 upward;
	Vector3 forward;
	Vector3 right;
	Vector3 velocity;
	Vector3 rotation;
	Quaternion rotationState;
	Quaternion turnQuaternion;
	Vector3 planetPosition;
	Vector3 toPlayer;

	public GameObject player;
	public GameObject planet;
	public float g = 9.81f;
	public float speed = 4f;

	MainScreenScript mainScript;

    void Start()
    {
        body = GetComponent<Rigidbody>();
	planetPosition = planet.transform.position;

	GameObject mainControl = GameObject.Find("MainControl");
	mainScript = mainControl.GetComponent<MainScreenScript>();
    }

    void FixedUpdate()
    {
        upward = this.transform.position - planetPosition;				//Get directions
	upward.Normalize();
	right = Vector3.Cross(upward, this.transform.forward);	//Unity uses lefthand rule!
	if(right == Vector3.zero) { right = Vector3.Cross(upward, this.transform.up); }
	right.Normalize();
	forward = Vector3.Cross(right, upward);			//Unity uses lefthand rule!
	rotationState.SetLookRotation(forward, upward);					//Rectify

	toPlayer = player.transform.position - this.transform.position;			//Turn to player
	toPlayer.Normalize();
	toPlayer -= upward*Vector3.Dot(upward, toPlayer);
	toPlayer.Normalize();
	rotationState = Quaternion.FromToRotation(forward, toPlayer)*rotationState;

	body.MoveRotation(rotationState);

	velocity = toPlayer*(speed + mainScript.difficulty);							//Move to player
	velocity -= forward*Vector3.Dot(body.linearVelocity, forward);
	velocity -= right*Vector3.Dot(body.linearVelocity, right);
	velocity -= upward*g*Time.fixedDeltaTime;
	body.AddForce(velocity, ForceMode.VelocityChange);
    }
}






